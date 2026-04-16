import { useEffect, useState } from 'react';
import './App.css';
import { postTransaction, getApprovedTransactions } from './api';

function App() {
    const [country, setCountry] = useState('Israel');
    const [hour, setHour] = useState('20');
    const [minute, setMinute] = useState('00');
    const [loading, setLoading] = useState(false);
    const [approved, setApproved] = useState([]);

    const [index, setIndex] = useState(0);
    const pageSize = 3;

    function next() {
        if (index + pageSize < approved.length) {
            setIndex(index + pageSize);
        }
    }

    function prev() {
        if (index - pageSize >= 0) {
            setIndex(index - pageSize);
        }
    }

    const visible = approved.slice(index, index + pageSize);

    async function onOk() {
        setLoading(true);
        try {
            const payload = {
                amount: 0,
                description: '',
                country,
                scheduledHour: parseInt(hour || '0', 10),
                scheduledMinute: parseInt(minute || '0', 10)
            };

            const result = await postTransaction(payload);
            alert('Transaction created. Id: ' + result.id);

            setTimeout(() => {
                refreshApproved();
            }, 1000);

        } catch (err) {
            const msg = err?.message || JSON.stringify(err);
            alert('Failed: ' + msg);
        } finally {
            setLoading(false);
        }
    }

    function onCancel() {
        setCountry('Israel');
        setHour('20');
        setMinute('00');
    }

    useEffect(() => {
        refreshApproved(); // first attempt

        // retry after 1 second in case backend wasn't ready
        setTimeout(() => {
            refreshApproved();
        }, 2000);
    }, []);

    async function refreshApproved() {
        try {
            const data = await getApprovedTransactions();
            data.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));
            setApproved(data);
        } catch (err) {
            console.error("Failed to refresh approved transactions", err);
        }
    }

    return (
        <div className="app-container">
            <header className="app-header">
                <img src="/shva.png" alt="shva logo" className="app-logo" />
            </header>

            <div className="main-layout">
                <div className="controls">
                <label className="control-label">Country</label>
                <select value={country} onChange={e => setCountry(e.target.value)}>
                    <option>Israel</option>
                    <option>United States</option>
                    <option>Japan</option>
                    <option>Italy</option>
                    <option>France</option>
                    <option>India</option>
                </select>

                <div className="time-picker">
                    <label className="control-label">Enter time</label>
                    <div className="time-inputs">
                            <input className="time-box hour" type="number" min="0" max="23" value={hour} onChange={e => {
                                const val = e.target.value;
                                setHour(val.padStart(2, '0'));
                            }} />
                        <span className="time-sep">:</span>
                            <input className="time-box minute" type="number" min="0" max="59" value={minute} onChange={e => {
                                const val = e.target.value;
                                setMinute(val.padStart(2, '0'));
                            }}
 />
                    </div>
                    <div className="time-actions">
                        <button onClick={onCancel} disabled={loading}>Cancel</button>
                        <button onClick={onOk} disabled={loading}>OK</button>
                    </div>
                </div>
                </div>
                {/* NEW RIGHT SIDE */}
                <div className="content-image">
                    <img src="/content.png" alt="content" className="content" />
                </div>
            </div>
            {/* NEW BOTTOM */}
            <div className="cards-section">

            <h2 className="approved-title">Approved Transactions</h2>

            <div className="cards-container">
                <button
                    className="arrow left"
                    onClick={prev}
                    disabled={index === 0}
                >
                    ‹
                </button>

                <div className="cards">
                    {visible.map(tx => {
                        const date = new Date(tx.scheduledAt);
                        const hh = String(date.getHours()).padStart(2, '0');
                        const mm = String(date.getMinutes()).padStart(2, '0');

                        return (
                            <div key={tx.id} className="card">
                                <div className="time">Time: {hh}:{mm}</div>
                                <div className="zone">Time Zone: {tx.country}</div>
                            </div>
                        );
                    })}
                </div>

                <button
                    className="arrow right"
                    onClick={next}
                    disabled={index + pageSize >= approved.length}
                >
                    ›
                </button>
                </div>
            </div>
        </div>
    );
}

export default App;