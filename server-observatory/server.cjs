'use strict';
const http = require('node:http');
const fs = require('node:fs');
const path = require('node:path');
const crypto = require('node:crypto');
const host = '127.0.0.1';
const port = Number(process.env.MONITOR_PORT || 8787);
const token = process.env.MONITOR_TOKEN || '';
const viewToken = process.env.MONITOR_VIEW_TOKEN || '';
const demo = process.argv.includes('--demo');
if (!demo && (token.length < 32 || viewToken.length < 32 || token === viewToken)) throw Error('Set distinct MONITOR_TOKEN (collector) and MONITOR_VIEW_TOKEN (viewer), each at least 32 random characters, or run --demo.');
const records = new Map();
const history = [];
const start = Date.now();
function authorized(req, expected) {
 const a = Buffer.from(req.headers.authorization || '');
 const b = Buffer.from('Bearer ' + expected);
 return a.length === b.length && crypto.timingSafeEqual(a,b);
}
function validate(x) {
 if (!x || typeof x !== 'object' || Array.isArray(x)) throw Error('Object required');
 for (const k of ['id','name','server','kind']) if (typeof x[k] !== 'string' || !/^[a-zA-Z0-9_. :\-]{1,100}$/.test(x[k])) throw Error('Invalid ' + k);
 if (!['server','website','database','process'].includes(x.kind)) throw Error('Invalid kind');
 const clean = Object.fromEntries(['id','name','server','kind'].map(k=>[k,x[k]]));
 for (const k of ['cpu','memory','disk','rps','p95','errorRate','connections','threadsRunning','abortedConnectsDelta','abortedClientsDelta','uptimeSeconds','intervalSeconds','memoryMB','logicalProcessors','diskFreeGB']) {
  if (x[k] !== undefined && (typeof x[k] !== 'number' || !Number.isFinite(x[k]) || x[k]<0 || x[k]>1e12 || (['cpu','memory','disk','errorRate'].includes(k) && x[k]>100))) throw Error('Invalid ' + k);
  if (x[k] !== undefined) clean[k]=x[k];
 }
 clean.receivedAt = new Date().toISOString();
 return clean;
}
function demoRows() {
 const tick=Math.floor((Date.now()-start)/15000), wobble=Math.sin(tick/3)*5;
 return [
 {id:'web-erp',name:'ERP Internal',server:'IIS-01',kind:'website',rps:126+wobble,p95:340,errorRate:0.3},
 {id:'web-inventory',name:'Inventory Report',server:'IIS-01',kind:'website',rps:42+wobble,p95:1420,errorRate:2.4},
 {id:'web-api',name:'Integration API',server:'IIS-01',kind:'website',rps:215+wobble,p95:185,errorRate:0.1},
 {id:'srv-01',name:'IIS-01',server:'IIS-01',kind:'server',cpu:78+wobble,memory:87,disk:68},
 {id:'process-summary',name:'sales_summary PID 4321',server:'IIS-01',kind:'process',cpu:38+wobble,memoryMB:220,logicalProcessors:2},
 {id:'db-01',name:'MySQL instance',server:'IIS-01',kind:'database',rps:318+wobble,p95:920,connections:86,errorRate:0.2}
 ].map(x=>({...x,receivedAt:new Date().toISOString()}));
}
function snapshot() {
 const now=Date.now();
 const rows = demo ? demoRows() : [...records.values()];
 return {mode:demo?'demo':'live',storage:'memory',updatedAt:new Date().toISOString(),rows:rows.map(r=>({...r,stale:now-Date.parse(r.receivedAt)>120000})),history};
}
const timer=setInterval(()=>{
 const rows=demo?demoRows():[...records.values()].filter(r=>Date.now()-Date.parse(r.receivedAt)<=120000);
 if(rows.length) history.push({at:new Date().toISOString(),rps:rows.filter(r=>r.kind==='website').reduce((s,r)=>s+(r.rps||0),0)});
 if(history.length>240)history.shift();
 for(const [id,r] of records)if(Date.now()-Date.parse(r.receivedAt)>86400000)records.delete(id);
},15000);
const server=http.createServer((req,res)=>{
 const headers={'X-Content-Type-Options':'nosniff','Cache-Control':'no-store','Referrer-Policy':'no-referrer','X-Frame-Options':'DENY','Content-Security-Policy':"default-src 'self'; script-src 'self'; style-src 'self'; connect-src 'self'; frame-ancestors 'none'; base-uri 'none'; form-action 'none'"};
 const send=(code,data,type='application/json')=>{res.writeHead(code,{...headers,'Content-Type':type});res.end(typeof data==='string'?data:JSON.stringify(data));};
 if(req.headers.host!==`127.0.0.1:${port}` && req.headers.host!==`localhost:${port}`) return send(403,{error:'Invalid host'});
 const url=new URL(req.url,'http://localhost');
 if(url.pathname==='/health' && req.method==='GET')return send(200,{ok:true,mode:demo?'demo':'live'});
 if(url.pathname.startsWith('/api/')) {
  if(!demo && !authorized(req, url.pathname === '/api/ingest' ? token : viewToken))return send(401,{error:'Authentication required'});
  if(url.pathname==='/api/metrics' && req.method==='GET')return send(200,snapshot());
  if(url.pathname==='/api/ingest' && req.method==='POST' && !demo) {
   if(!String(req.headers['content-type']||'').startsWith('application/json'))return send(415,{error:'Use application/json'});
   let body='',bytes=0,oversized=false;
   req.on('data',chunk=>{bytes+=chunk.length;if(bytes>16384){if(!oversized)send(413,{error:'Payload too large'});oversized=true;return;}body+=chunk;});
   req.on('end',()=>{if(oversized)return;try{const row=validate(JSON.parse(body));if(!records.has(row.id)&&records.size>=1000)return send(429,{error:'Resource limit'});records.set(row.id,row);send(202,{accepted:true});}catch(e){send(400,{error:e.message});}});
   return;
  }
  return send(404,{error:'Not found'});
 }
 if(req.method!=='GET')return send(405,{error:'Method not allowed'});
 const files={'/':['index.html','text/html; charset=utf-8'],'/app.js':['app.js','application/javascript'],'/style.css':['style.css','text/css']};
 const file=files[url.pathname];if(!file)return send(404,{error:'Not found'});
 send(200,fs.readFileSync(path.join(__dirname,'public',file[0]),'utf8'),file[1]);
});
server.requestTimeout=10000;server.headersTimeout=10000;server.maxConnections=64;
server.listen(port,host,()=>console.log(`Server Observatory: http://${host}:${port} (${demo?'DEMO — synthetic data':'LIVE — authenticated, memory only'})`));
function stop(){clearInterval(timer);server.close();}process.on('SIGTERM',stop);process.on('SIGINT',stop);
