'use strict';
let token='',kind='all',latest=null,busy=false;
const $=s=>document.querySelector(s);
const escape=s=>String(s).replace(/[&<>"']/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
const fmt=n=>n===undefined?'—':Number(n).toLocaleString('id-ID',{maximumFractionDigits:1});
const bad=r=>(r.cpu>=80||r.memory>=85||r.disk>=85||r.p95>=1000||r.errorRate>=1);
function render(){
 const data=latest;if(!data)return;
 $('#mode').textContent=data.mode==='demo'?'MODE DEMO — Seluruh angka adalah simulasi. Belum tersambung ke server atau database Anda.':'MODE LIVE — Snapshot collector. Metrik lebih dari 2 menit ditandai kedaluwarsa.';
 $('#updated').textContent='Snapshot '+new Date(data.updatedAt).toLocaleTimeString('id-ID')+' · browser refresh 30 detik';
 const live=data.rows.filter(r=>!r.stale),web=live.filter(r=>r.kind==='website');
 const stats=[['Layanan terpantau',live.length,'Dari '+data.rows.length+' layanan terdaftar'],['Trafik website',fmt(web.reduce((s,r)=>s+(r.rps||0),0)),'Request per detik · snapshot terkini'],['Perlu perhatian',live.filter(bad).length,'Ambang awal untuk investigasi'],['Data kedaluwarsa',data.rows.filter(r=>r.stale).length,'Tidak ada snapshot selama > 2 menit']];
 $('#stats').innerHTML=stats.map(([a,b,c])=>`<div class="stat"><small>${a}</small><strong>${b}</strong><p>${c}</p></div>`).join('');
 const alerts=data.rows.filter(r=>r.stale||bad(r));
 $('#alerts').innerHTML=alerts.length?alerts.slice(0,5).map(r=>`<div class="alert">${escape(r.name)}<small>${r.stale?'Collector tidak mengirim data > 2 menit. Periksa koneksi dan task.':[(r.memory>=85?'RAM tinggi: periksa pool dan pola alokasi.':''),(r.cpu>=80?'CPU tinggi: korelasikan dengan trafik.':''),(r.disk>=85?'Disk hampir penuh: periksa pertumbuhan file.':''),(r.p95>=1000?'Latensi p95 tinggi: telusuri endpoint/query.':''),(r.errorRate>=1?'Error meningkat: periksa aplikasi dan dependensi.':'')].filter(Boolean).join(' ')}</small></div>`).join(''):'<div class="empty">Belum ada sinyal peringatan dari data tersedia.</div>';
 const q=$('#search').value.toLowerCase();
 const rows=data.rows.filter(r=>(kind==='all'||r.kind===kind)&&(r.name+' '+r.server).toLowerCase().includes(q)).sort((a,b)=>kind==='process'?(b.cpu??-1)-(a.cpu??-1):0);
 $('#count').textContent=rows.length+' LAYANAN';
 $('#rows').innerHTML=rows.length?rows.map(r=>`<tr><td>${escape(r.name)}<small>${escape(r.server)}</small></td><td>${escape(r.kind)}</td><td><span class="pill ${r.stale?'stale':bad(r)?'warn':''}">${r.stale?'Kedaluwarsa':bad(r)?'Perhatian':'Normal'}</span></td><td>${fmt(r.rps)}</td><td>${fmt(r.p95)}</td><td>${r.errorRate===undefined?'—':fmt(r.errorRate)+'%'}</td><td>${['cpu','memory','disk'].map(k=>r[k]===undefined?'—':fmt(r[k])+'%').join(' / ')}</td><td>${fmt(r.memoryMB)}</td><td>${[r.connections,r.threadsRunning,r.abortedConnectsDelta].map(fmt).join(' / ')}${r.intervalSeconds===undefined?'':'<small>Interval '+fmt(r.intervalSeconds)+' detik</small>'}</td></tr>`).join(''):'<tr><td colspan="9" class="empty">Belum ada layanan yang cocok. Hubungkan collector atau ubah pencarian.</td></tr>';
 const h=data.history;
 if(h.length<2){$('#chart').innerHTML='<div class="empty">Menunggu minimal dua sampel collector.<br>Grafik mulai muncul setelah sekitar 30 detik.</div>';return;}
 const max=Math.max(1,...h.map(x=>x.rps));const points=h.map((x,i)=>`${40+i/(h.length-1)*620},${155-x.rps/max*125}`).join(' ');
 $('#chart').innerHTML=`<svg viewBox="0 0 700 185" role="img" aria-label="Riwayat total request website per detik"><path d="M40 30H665 M40 92H665 M40 155H665" stroke="#e8eee8" fill="none"/><text x="0" y="34">${fmt(max)}</text><text x="14" y="159">0</text><polygon points="40,155 ${points} 660,155" fill="#edf4de"/><polyline points="${points}" fill="none" stroke="#7c9f56" stroke-width="2.5"/><text x="40" y="180">${new Date(h[0].at).toLocaleTimeString('id-ID')}</text><text x="585" y="180">${new Date(h.at(-1).at).toLocaleTimeString('id-ID')}</text></svg>`;
}
async function refresh(){if(busy)return;busy=true;try{const res=await fetch('/api/metrics',{headers:token?{Authorization:'Bearer '+token}:{},cache:'no-store',signal:AbortSignal.timeout(8000)});if(res.status===401){if(!$('#auth').open)$('#auth').showModal();throw Error('Token diperlukan atau tidak valid.');}if(!res.ok)throw Error('HTTP '+res.status);latest=await res.json();render();$('#auth').close();}catch(e){$('#mode').textContent='Koneksi belum tersedia: '+e.message+' Angka sebelumnya bukan snapshot terbaru.';$('#auth-error').textContent=e.message;}finally{busy=false;}}
$('#login').addEventListener('submit',e=>{e.preventDefault();token=$('#token').value;$('#token').value='';refresh();});
$('#refresh').addEventListener('click',refresh);$('#search').addEventListener('input',render);
document.querySelectorAll('nav button').forEach(b=>b.addEventListener('click',()=>{kind=b.dataset.kind;document.querySelectorAll('nav button').forEach(x=>x.classList.toggle('active',x===b));$('#table-title').textContent=kind==='all'?'Semua layanan':b.textContent.trim();render();}));
refresh();setInterval(()=>{if(!document.hidden)refresh();},30000);
document.addEventListener('visibilitychange',()=>{if(!document.hidden)refresh();});
