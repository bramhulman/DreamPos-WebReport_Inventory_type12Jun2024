'use strict';
const assert=require('node:assert/strict');
const fs=require('node:fs/promises');
const os=require('node:os');
const path=require('node:path');
const {parse,collectSite}=require('./collect-iis.cjs');
(async()=>{
 const header='#Fields: date time sc-status time-taken\n';
 const parsed=parse(Buffer.from(header+'2026-10-15 05:00:00 200 20\n2026-10-15 05:00:01 500 1000\n2026-10-15 05:00:02 404 30\n'));
 assert.equal(parsed.count,3);assert.equal(parsed.errors,1);assert.equal(parsed.p95,1000);
 assert.equal(parse(Buffer.from(header+'invalid bad 200 10\n')).count,0);
 const dir=await fs.mkdtemp(path.join(os.tmpdir(),'observatory-iis-'));
 try{
 const file=path.join(dir,'u_ex261015.log');await fs.writeFile(file,header);
 const site={id:'test',name:'Test',server:'Test',logDirectory:dir};
 const now=Date.parse('2026-10-15T05:01:00Z');
 const baseline=await collectSite(site,null,now);assert.equal(baseline.metrics,undefined);
 await fs.appendFile(file,'2026-10-15 05:01:30 200 20\n2026-10-15 05:01:31 503 1000\n2026-10-15 05:01:32 200');
 const snapshot=await collectSite(site,baseline.state,now+60000);assert.equal(snapshot.metrics.rps,2/60);assert.equal(snapshot.metrics.errorRate,50);assert.equal(snapshot.metrics.p95,1000);
 await fs.appendFile(file,' 30\n');const partial=await collectSite(site,snapshot.state,now+90000);assert.equal(partial.metrics.rps,1/30);
 const empty=await collectSite(site,partial.state,now+120000);assert.equal(empty.metrics,undefined);
 await fs.writeFile(file,header);const truncated=await collectSite(site,partial.state,now+120000);assert.equal(truncated.metrics,undefined);
 await fs.appendFile(file,'2020-01-01 00:00:00 200 10\n');const stale=await collectSite(site,truncated.state,now+150000);assert.equal(stale.metrics,undefined);
 console.log('IIS tests passed: field parsing, p95, 5xx, baseline, partial lines, empty/stale data and truncation.');
 }finally{await fs.rm(dir,{recursive:true,force:true});}
})().catch(e=>{console.error(e);process.exitCode=1;});
