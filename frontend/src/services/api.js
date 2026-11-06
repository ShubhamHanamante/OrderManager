const API_BASE = '/api';

export async function sendChatMessage(message, customerId, history = []) {
  const response = await fetch(`${API_BASE}/chat`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      message,
      customerId,
      history: history.map(h => ({
        role: h.role,
        content: h.content
      }))
    })
  });

  if (!response.ok) {
    const errorData = await response.json().catch(() => ({}));
    throw new Error(errorData.message || `Request failed with status ${response.status}`);
  }

  return await response.json();
}

export async function fetchOrderDetails(orderNumber, customerId) {
  const response = await fetch(`${API_BASE}/orders/${orderNumber}?customerId=${customerId}`);
  if (!response.ok) {
    const errorData = await response.json().catch(() => ({}));
    throw new Error(errorData.message || 'Failed to fetch order details');
  }
  return await response.json();
}

export async function fetchRecentOrders(customerId, limit = 5) {
  const response = await fetch(`${API_BASE}/orders/recent?customerId=${customerId}&limit=${limit}`);
  if (!response.ok) {
    const errorData = await response.json().catch(() => ({}));
    throw new Error(errorData.message || 'Failed to fetch recent orders');
  }
  return await response.json();
}

