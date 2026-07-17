import React, { useEffect, useState } from 'react';
import axios from 'axios';

const API_BASE = process.env.REACT_APP_API_URL || 'http://localhost:7071/api';

function App() {
  const [view, setView] = useState('report');
  const [rows, setRows] = useState([]);
  const [loading, setLoading] = useState(false);

  const [players, setPlayers] = useState([]);
  const [assets, setAssets] = useState([]);

  const [playerName, setPlayerName] = useState('');
  const [fullName, setFullName] = useState('');
  const [age, setAge] = useState(18);
  const [level, setLevel] = useState(1);
  const [email, setEmail] = useState('');

  const [assetName, setAssetName] = useState('');
  const [levelRequire, setLevelRequire] = useState(1);

  const [selPlayer, setSelPlayer] = useState('');
  const [selAsset, setSelAsset] = useState('');
  const [quantity, setQuantity] = useState(1);

  useEffect(() => { loadAll(); }, []);

  function loadAll() {
    loadReport();
    loadPlayers();
    loadAssets();
  }

  function loadReport() {
    setLoading(true);
    axios.get(`${API_BASE}/getassetsbyplayer`)
      .then(res => setRows(res.data))
      .catch(err => { console.error(err); alert('Failed to load report.'); })
      .finally(() => setLoading(false));
  }

  function loadPlayers() { axios.get(`${API_BASE}/players`).then(res => setPlayers(res.data)).catch(() => setPlayers([])); }
  function loadAssets() { axios.get(`${API_BASE}/assets`).then(res => setAssets(res.data)).catch(() => setAssets([])); }

  async function handleCreatePlayer(e) {
    e.preventDefault();
    try {
      await axios.post(`${API_BASE}/registerplayer`, { playerName, fullName, age: Number(age), level: Number(level), email });
      alert('Player created');
      setPlayerName(''); setFullName(''); setAge(18); setLevel(1); setEmail('');
      loadPlayers(); loadReport();
    } catch (err) { console.error(err); alert('Create player failed'); }
  }

  async function handleCreateAsset(e) {
    e.preventDefault();
    try {
      await axios.post(`${API_BASE}/createasset`, { assetName, levelRequire: Number(levelRequire) });
      alert('Asset created');
      setAssetName(''); setLevelRequire(1);
      loadAssets(); loadReport();
    } catch (err) { console.error(err); alert('Create asset failed'); }
  }

  async function handleAssign(e) {
    e.preventDefault();
    if (!selPlayer || !selAsset) { alert('Select player and asset'); return; }
    try {
      await axios.post(`${API_BASE}/assignasset`, { playerId: selPlayer, assetId: selAsset, quantity: Number(quantity) });
      alert('Assigned');
      loadReport();
    } catch (err) { console.error(err); alert('Assign failed'); }
  }

  return (
    <div className="container app-container">
      <div className="row mb-3">
        <div className="col">
          <h1 className="h3">BattleGame Admin</h1>
        </div>
        <div className="col-auto">
          <div className="btn-group" role="group">
            <button className="btn btn-outline-primary" onClick={() => setView('report')}>Report</button>
            <button className="btn btn-outline-secondary" onClick={() => setView('createPlayer')}>Create Player</button>
            <button className="btn btn-outline-secondary" onClick={() => setView('createAsset')}>Create Asset</button>
            <button className="btn btn-outline-secondary" onClick={() => setView('assign')}>Assign Asset</button>
          </div>
        </div>
      </div>

      {view === 'report' && (
        <div className="card">
          <div className="card-body">
            <div className="d-flex justify-content-between align-items-center mb-2">
              <h5 className="card-title mb-0">Assets by Player</h5>
              <button className="btn btn-sm btn-primary" onClick={loadReport}>Refresh</button>
            </div>
            {loading ? <p>Loading...</p> : (
              <div className="table-responsive">
                <table className="table table-striped table-bordered">
                  <thead className="table-dark">
                    <tr>
                      <th>No</th>
                      <th>Player name</th>
                      <th>Level</th>
                      <th>Age</th>
                      <th>Asset name</th>
                    </tr>
                  </thead>
                  <tbody>
                    {rows.map(r => (
                      <tr key={r.No ?? r.no}>
                        <td style={{width:60}}>{r.No ?? r.no}</td>
                        <td>{r.PlayerName ?? r.playerName}</td>
                        <td>{r.Level ?? r.level}</td>
                        <td>{r.Age ?? r.age}</td>
                        <td>{r.AssetName ?? r.assetName}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )}
          </div>
        </div>
      )}

      {view === 'createPlayer' && (
        <div className="card">
          <div className="card-body">
            <h5 className="card-title">Create Player</h5>
            <form onSubmit={handleCreatePlayer}>
              <div className="mb-2">
                <label className="form-label">Player Name</label>
                <input className="form-control" value={playerName} onChange={e => setPlayerName(e.target.value)} required />
              </div>
              <div className="mb-2">
                <label className="form-label">Full Name</label>
                <input className="form-control" value={fullName} onChange={e => setFullName(e.target.value)} />
              </div>
              <div className="row">
                <div className="col-6 mb-2">
                  <label className="form-label">Age</label>
                  <input type="number" className="form-control" value={age} onChange={e => setAge(e.target.value)} />
                </div>
                <div className="col-6 mb-2">
                  <label className="form-label">Level</label>
                  <input type="number" className="form-control" value={level} onChange={e => setLevel(e.target.value)} />
                </div>
              </div>
              <div className="mb-2">
                <label className="form-label">Email</label>
                <input className="form-control" value={email} onChange={e => setEmail(e.target.value)} />
              </div>
              <div className="mt-3">
                <button className="btn btn-primary" type="submit">Create</button>
                <button type="button" className="btn btn-link ms-2" onClick={() => setView('report')}>Cancel</button>
              </div>
            </form>
          </div>
        </div>
      )}

      {view === 'createAsset' && (
        <div className="card">
          <div className="card-body">
            <h5 className="card-title">Create Asset</h5>
            <form onSubmit={handleCreateAsset}>
              <div className="mb-2">
                <label className="form-label">Asset Name</label>
                <input className="form-control" value={assetName} onChange={e => setAssetName(e.target.value)} required />
              </div>
              <div className="mb-2">
                <label className="form-label">Level Require</label>
                <input type="number" className="form-control" value={levelRequire} onChange={e => setLevelRequire(e.target.value)} />
              </div>
              <div className="mt-3">
                <button className="btn btn-primary" type="submit">Create</button>
                <button type="button" className="btn btn-link ms-2" onClick={() => setView('report')}>Cancel</button>
              </div>
            </form>
          </div>
        </div>
      )}

      {view === 'assign' && (
        <div className="card">
          <div className="card-body">
            <h5 className="card-title">Assign Asset to Player</h5>
            <form onSubmit={handleAssign}>
              <div className="mb-2">
                <label className="form-label">Player</label>
                <select className="form-select" value={selPlayer} onChange={e => setSelPlayer(e.target.value)}>
                  <option value="">--select--</option>
                  {players.map(p => <option key={p.playerId ?? p.PlayerId} value={p.playerId ?? p.PlayerId}>{p.playerName ?? p.PlayerName}</option>)}
                </select>
              </div>
              <div className="mb-2">
                <label className="form-label">Asset</label>
                <select className="form-select" value={selAsset} onChange={e => setSelAsset(e.target.value)}>
                  <option value="">--select--</option>
                  {assets.map(a => <option key={a.assetId ?? a.AssetId} value={a.assetId ?? a.AssetId}>{a.assetName ?? a.AssetName}</option>)}
                </select>
              </div>
              <div className="mb-2">
                <label className="form-label">Quantity</label>
                <input type="number" className="form-control" value={quantity} onChange={e => setQuantity(e.target.value)} />
              </div>
              <div className="mt-3">
                <button className="btn btn-primary" type="submit">Assign</button>
                <button type="button" className="btn btn-link ms-2" onClick={() => setView('report')}>Cancel</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}

export default App;
