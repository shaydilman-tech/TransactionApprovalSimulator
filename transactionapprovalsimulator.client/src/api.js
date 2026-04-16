const API_BASE = "http://localhost:5000/api";

export async function postTransaction(payload) {
    const res = await fetch(`${API_BASE}/transactions`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
    });

    if (!res.ok) {
        let body;
        try { body = await res.json(); } catch { body = await res.text(); }
        const message = body?.error || body?.message || String(body);
        throw new Error(message || `HTTP ${res.status}`);
    }

    return res.json();
}

export async function getApprovedTransactions() {
    const res = await fetch(`${API_BASE}/transactions/approved`);
    if (!res.ok) throw new Error('Failed to fetch approved transactions');
    return await res.json();
}