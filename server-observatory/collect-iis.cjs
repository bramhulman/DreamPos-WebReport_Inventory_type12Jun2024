'use strict';
const fs = require('node:fs/promises');
const path = require('node:path');
const crypto = require('node:crypto');
const LIMIT=1024*1024;
const MAX_SITES=100;
function parse(buffer,initialFields){
 let fields=initialFields||[],count=0,errors=0,latestTime=0;
 const durations=[];
 for(const line of buffer.toString('utf8').split('\n')){
  if(line.startsWith('#Fields:')){fields=line.slice(8).trim().split(/\s+/);continue;}
  if(!line.trim()||line.startsWith('#'))continue;
  const cols=line.trim().split(/\s+/),get=name=>cols[fields.indexOf(name)];
  const stamp=Date.parse(get('date')+'T'+get('time')+'Z');
  const status=Number(get('sc-status'));
  if(!Number.isFinite(stamp)||!Number.isInteger(status)||status<100||status>599)continue;
  count++;if(status>=500)errors++;
  latestTime=Math.max(latestTime,stamp);
  const elapsed=Number(get('time-taken'));if(get('time-taken')!==undefined&&get('time-taken')!=='-'&&Number.isFinite(elapsed)&&elapsed>=0)durations.push(elapsed);
 }
 durations.sort((a,b)=>a-b);
 return {fields,count,errors,latestTime,p95:durations.length?durations[Math.ceil(durations.length*.95)-1]:undefined};
}
async function collectSite(site,previous,now=Date.now()){
 const entries=await fs.readdir(site.logDirectory,{withFileTypes:true});
 if(entries.length>10000)throw Error('Log directory exceeds inventory limit');
 const files=[];
 for(const entry of entries){if(entry.isFile()&&/^u_ex\d+.*\.log$/i.test(entry.name)){const full=path.join(site.logDirectory,entry.name);const stat=await fs.stat(full);files.push({full,stat});}}
 files.sort((a,b)=>b.stat.mtimeMs-a.stat.mtimeMs);
 if(!files.length)return {warning:'No W3C logs found',state:null};
 const {full,stat}=files[0];
 const signature=crypto.createHash('sha256').update(site.id+'\0'+site.name+'\0'+site.server+'\0'+path.resolve(site.logDirectory)).digest('hex');
 const next={file:full,created:stat.birthtimeMs,signature,offset:stat.size,at:now,fields:[]};
 // New file, truncation, downtime, and first run establish a baseline, never report catch-up as live RPS.
 const elapsed=previous?(now-previous.at)/1000:0;
 if(!previous||previous.signature!==signature||previous.file!==full||previous.created!==stat.birthtimeMs||stat.size<previous.offset||elapsed<5||elapsed>120||stat.size-previous.offset>LIMIT){
  return {state:next,warning:'Baseline renewed (initial, rotated, backlog or invalid interval); no rate emitted'};
 }
 const handle=await fs.open(full,'r');
 try{
  let fields=previous.fields||[];
  if(!fields.length){const header=Buffer.alloc(Math.min(65536,stat.size));const {bytesRead}=await handle.read(header,0,header.length,0);const matches=header.subarray(0,bytesRead).toString('utf8').match(/^#Fields:.*$/gm);if(matches?.length)fields=matches[0].slice(8).trim().split(/\s+/);}
  if(!['date','time','sc-status'].every(f=>fields.includes(f)))throw Error('Required W3C fields missing');
  const buffer=Buffer.alloc(stat.size-previous.offset);
  const {bytesRead}=await handle.read(buffer,0,buffer.length,previous.offset);
  const end=buffer.subarray(0,bytesRead).lastIndexOf(10);
  const complete=end<0?Buffer.alloc(0):buffer.subarray(0,end+1);
  const result=parse(complete,fields);
  next.offset=previous.offset+complete.length;next.fields=result.fields;
  if(result.count && (now-result.latestTime>120000 || result.latestTime-now>30000))return {state:next,warning:'Log timestamps stale or clock skew; rate omitted'};
  if(!result.count)return {state:next,warning:'No completed requests observed; log freshness cannot be confirmed'};
  const metrics={id:site.id,name:site.name,server:site.server,kind:'website',intervalSeconds:elapsed,rps:result.count/elapsed,errorRate:100*result.errors/result.count};
  if(result.p95!==undefined)metrics.p95=result.p95;
  return {state:next,metrics};
 }finally{await handle.close();}
}
async function main(){
 const configFile=process.argv[2];if(!configFile)throw Error('Usage: node collect-iis.cjs <config.local.json>');
 const config=JSON.parse(await fs.readFile(configFile,'utf8'));
 const endpoint=new URL(config.endpoint||'http://127.0.0.1:8787/api/ingest');
 if(endpoint.protocol!=='https:'&&!(endpoint.protocol==='http:'&&['localhost','127.0.0.1','[::1]'].includes(endpoint.hostname)))throw Error('Remote endpoint requires HTTPS');
 const token=process.env.MONITOR_TOKEN;if(!token||token.length<32)throw Error('Collector MONITOR_TOKEN required');
 if(!Array.isArray(config.sites)||!config.sites.length||config.sites.length>MAX_SITES)throw Error('Configure 1 to 100 sites');
 const ids=new Set();for(const site of config.sites){for(const key of ['id','name','server'])if(typeof site[key]!=='string'||!/^[a-zA-Z0-9_. :\-]{1,100}$/.test(site[key]))throw Error('Invalid site '+key);if(ids.has(site.id))throw Error('Duplicate site id');ids.add(site.id);if(typeof site.logDirectory!=='string'||!path.isAbsolute(site.logDirectory))throw Error('Absolute logDirectory required');}
 const statePath=path.resolve(path.dirname(configFile),config.stateFile||'.iis-state.json');
 const lock=await fs.open(statePath+'.lock','wx');
 try{
  let state={};try{state=JSON.parse(await fs.readFile(statePath,'utf8'));}catch(e){if(e.code!=='ENOENT')throw e;}
  let failures=0;const deadline=Date.now()+45000;
  for(const site of config.sites){
   if(Date.now()>=deadline){console.error('Run budget reached; remaining sites skipped');failures++;break;}
   try{
    const result=await collectSite(site,state[site.id]);
    if(result.warning)console.warn(site.id+': '+result.warning);
    if(result.metrics){const res=await fetch(endpoint,{method:'POST',headers:{Authorization:'Bearer '+token,'Content-Type':'application/json'},body:JSON.stringify(result.metrics),signal:AbortSignal.timeout(8000)});if(!res.ok)throw Error('Ingestion HTTP '+res.status);}
    if(result.state)state[site.id]=result.state;
   }catch(e){failures++;console.error(site.id+': collection failed ('+(e.code||'check local configuration and endpoint')+')');}
  }
  await fs.writeFile(statePath+'.tmp',JSON.stringify(state),{mode:0o600});await fs.rename(statePath+'.tmp',statePath);
  if(failures)process.exitCode=1;
 }finally{await lock.close();await fs.unlink(statePath+'.lock');}
}
module.exports={parse,collectSite};
if(require.main===module)main().catch(e=>{console.error(e.message);process.exitCode=1;});
