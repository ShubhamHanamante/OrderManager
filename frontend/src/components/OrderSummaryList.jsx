import React from 'react';
import { Package, Truck, Calendar, ChevronRight } from 'lucide-react';

const STATUS_COLORS = {
  Delivered: { bg: '#ecfdf5', text: '#065f46', border: '#a7f3d0' },
  Shipped: { bg: '#eff6ff', text: '#1e40af', border: '#bfdbfe' },
  InTransit: { bg: '#eff6ff', text: '#1e40af', border: '#bfdbfe' },
  Processing: { bg: '#fefce8', text: '#854d0e', border: '#fef08a' },
  Confirmed: { bg: '#f0fdf4', text: '#166534', border: '#bbf7d0' },
  Pending: { bg: '#fff7ed', text: '#9a3412', border: '#fed7aa' },
  Cancelled: { bg: '#fef2f2', text: '#991b1b', border: '#fecaca' }
};

export default function OrderSummaryList({ orders, onSelectOrder }) {
  if (!orders || !Array.isArray(orders) || orders.length === 0) return null;

  return (
    <div style={{
      marginTop: '0.75rem',
      marginBottom: '0.75rem',
      display: 'flex',
      flexDirection: 'column',
      gap: '0.5rem'
    }}>
      {orders.map((order, idx) => {
        const statusStyle = STATUS_COLORS[order.orderStatus] || STATUS_COLORS.Pending;
        return (
          <div
            key={idx}
            onClick={() => onSelectOrder && onSelectOrder(order.orderNumber)}
            style={{
              background: '#ffffff',
              border: '1px solid #e2e8f0',
              borderRadius: '10px',
              padding: '0.75rem 1rem',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'space-between',
              cursor: onSelectOrder ? 'pointer' : 'default',
              transition: 'all 0.15s ease',
              boxShadow: '0 1px 3px rgba(0, 0, 0, 0.04)'
            }}
            onMouseEnter={(e) => {
              if (onSelectOrder) {
                e.currentTarget.style.borderColor = '#93c5fd';
                e.currentTarget.style.transform = 'translateY(-1px)';
              }
            }}
            onMouseLeave={(e) => {
              if (onSelectOrder) {
                e.currentTarget.style.borderColor = '#e2e8f0';
                e.currentTarget.style.transform = 'none';
              }
            }}
          >
            <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem' }}>
              <div style={{
                background: '#f1f5f9',
                color: '#475569',
                padding: '0.4rem',
                borderRadius: '8px'
              }}>
                <Package size={16} />
              </div>
              <div>
                <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
                  <span style={{ fontSize: '0.85rem', fontWeight: 700, color: '#0f172a' }}>
                    {order.orderNumber}
                  </span>
                  <span style={{
                    background: statusStyle.bg,
                    color: statusStyle.text,
                    border: `1px solid ${statusStyle.border}`,
                    fontSize: '0.65rem',
                    fontWeight: 700,
                    padding: '0.1rem 0.45rem',
                    borderRadius: '9999px',
                    textTransform: 'uppercase'
                  }}>
                    {order.orderStatus}
                  </span>
                </div>
                <div style={{ fontSize: '0.75rem', color: '#64748b', marginTop: '0.15rem' }}>
                  {order.orderDateUtc ? new Date(order.orderDateUtc).toLocaleDateString(undefined, { dateStyle: 'medium' }) : ''}
                  {order.carrier && ` • ${order.carrier}`}
                  {order.trackingNumber && ` (${order.trackingNumber})`}
                </div>
              </div>
            </div>

            <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem' }}>
              <div style={{ textAlign: 'right' }}>
                <div style={{ fontSize: '0.9rem', fontWeight: 700, color: '#0f172a' }}>
                  ₹{order.total?.toFixed(2)}
                </div>
                {order.itemCount > 0 && (
                  <div style={{ fontSize: '0.7rem', color: '#64748b' }}>
                    {order.itemCount} item{order.itemCount > 1 ? 's' : ''}
                  </div>
                )}
              </div>
              {onSelectOrder && <ChevronRight size={16} color="#94a3b8" />}
            </div>
          </div>
        );
      })}
    </div>
  );
}
