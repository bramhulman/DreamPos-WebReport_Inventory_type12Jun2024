'use strict';
const {spawn}=require('node:child_process');
const {randomBytes}=require('node:crypto');
const assert=require('node:assert/strict');
const token=randomBytes(32).toString('hex');
const viewToken=randomBytes(32).toString('hex');
async function run(demo,port){
 const child=spawn(process.execPath,['server.cjs',...(demo?['--demo']:[])],{cwd:__dirname,env:{...process.env,MONITOR_TOKEN:token,MONITOR_VIEW_TOKEN:viewToken,MONITOR_PORT:String(port)},stdio:['ignore','pipe','pipe']});
 try{
 await new Promise((resolve,reject)=>{const timer=setTimeout(()=>reject(Error('Startup timeout')),8000);child.once('error',reject);child.once('exit',c=>{clearTimeout(timer);reject(Error('Unexpected exit '+c));});child.stdout.once('data',()=>{clearTimeout(timer);resolve();});});
 const base='http://127.0.0.1:'+port;
 const headers={Authorization:'Bearer '+token,'Content-Type':'application/json'};
 assert.equal((await fetch(base+'/health')).status,200);
 const page=await fetch(base+'/');assert.equal(page.status,200);assert.match(page.headers.get('content-security-policy'),/frame-ancestors 'none'/);
 assert.equal((await fetch(base+'/api/metrics')).status,demo?200:401);
 if(!demo){
 assert.equal((await fetch(base+'/api/metrics',{headers})).status,401);
 assert.equal((await fetch(base+'/api/ingest',{method:'POST',headers:{Authorization:'Bearer '+viewToken,'Content-Type':'application/json'},body:'{}'})).status,401);
 assert.equal((await fetch(base+'/api/ingest',{method:'POST',headers,body:'{"id":"bad"}'})).status,400);
 assert.equal((await fetch(base+'/api/ingest',{method:'POST',headers,body:JSON.stringify({id:'x',name:'Test',server:'Test',kind:'server',cpu:101})})).status,400);
 assert.equal((await fetch(base+'/api/ingest',{method:'POST',headers,body:JSON.stringify({id:'x',name:'Test',server:'Test',kind:'server',cpu:25,secret:'must-not-survive'})})).status,202);
 const data=await (await fetch(base+'/api/metrics',{headers:{Authorization:'Bearer '+viewToken}})).json();assert.equal(data.rows[0].cpu,25);assert.equal(data.rows[0].secret,undefined);assert.equal(data.mode,'live');
 assert.equal((await fetch(base+'/api/ingest',{method:'POST',headers,body:JSON.stringify({padding:'x'.repeat(17000)})})).status,413);
 }else{assert.equal((await (await fetch(base+'/api/metrics')).json()).mode,'demo');assert.equal((await fetch(base+'/api/ingest',{method:'POST',headers,body:'{}'})).status,404);}
 console.log((demo?'DEMO':'LIVE')+' smoke checks passed');
 }finally{child.kill();await new Promise(resolve=>child.exitCode!==null?resolve():child.once('exit',resolve));}
}
(async()=>{await run(false,18787);await run(true,18788);})().catch(e=>{console.error(e);process.exitCode=1;});
