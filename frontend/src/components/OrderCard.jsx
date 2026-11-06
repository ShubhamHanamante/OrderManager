import React, { useState } from 'react';
import {
  Package,
  Truck,
  CreditCard,
  Calendar,
  MapPin,
  Clock,
  ChevronDown,
  ChevronUp,
  ExternalLink,
  CheckCircle2,
  AlertCircle
} from 'lucide-react';

const STATUS_COLORS = {
  Delivered: { bg: '#ecfdf5', text: '#065f46', border: '#a7f3d0' },
  Shipped: { bg: '#eff6ff', text: '#1e40af', border: '#bfdbfe' },
  InTransit: { bg: '#eff6ff', text: '#1e40af', border: '#bfdbfe' },
  Processing: { bg: '#fefce8', text: '#854d0e', border: '#fef08a' },
  Confirmed: { bg: '#f0fdf4', text: '#166534', border: '#bbf7d0' },
  Pending: { bg: '#fff7ed', text: '#9a3412', border: '#fed7aa' },
  Cancelled: { bg: '#fef2f2', text: '#991b1b', border: '#fecaca' }
};

export default function OrderCard({ order }) {
  const [showHistory, setShowHistory] = useState(false);

  if (!order || !order.orderNumber) return null;

  const statusStyle = STATUS_COLORS[order.orderStatus] || STATUS_COLORS.Pending;
  const shipment = order.shipments && order.shipments[0];
  const payment = order.payments && order.payments[0];

  return (
    <div style={{
      background: '#ffffff',
      borderRadius: '14px',
      border: '1px solid #e2e8f0',
      boxShadow: '0 4px 6px -1px rgba(0, 0, 0, 0.05)',
      marginTop: '0.75rem',
      marginBottom: '0.75rem',
      overflow: 'hidden'
    }}>
      {/* Header Banner */}
      <div style={{
        background: '#f8fafc',
        padding: '0.85rem 1.15rem',
        borderBottom: '1px solid #e2e8f0',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        flexWrap: 'wrap',
        gap: '0.5rem'
      }}>
        <div style={{ display: 'flex', alignItems: 'center', gap: '0.6rem' }}>
          <div style={{
            background: '#e2e8f0',
            padding: '0.35rem',
            borderRadius: '8px',
            color: '#334155'
          }}>
            <Package size={18} />
          </div>
          <div>
            <span style={{ fontSize: '0.95rem', fontWeight: 700, color: '#0f172a' }}>
              {order.orderNumber}
            </span>
            <span style={{ fontSize: '0.75rem', color: '#64748b', marginLeft: '0.5rem' }}>
              {order.orderDateUtc ? new Date(order.orderDateUtc).toLocaleDateString(undefined, { dateStyle: 'medium' }) : ''}
            </span>
          </div>
        </div>

        <span style={{
          background: statusStyle.bg,
          color: statusStyle.text,
          border: `1px solid ${statusStyle.border}`,
          padding: '0.2rem 0.65rem',
          borderRadius: '9999px',
          fontSize: '0.75rem',
          fontWeight: 700,
          textTransform: 'uppercase',
          letterSpacing: '0.04em'
        }}>
          {order.orderStatus}
        </span>
      </div>

      <div style={{ padding: '1rem 1.15rem' }}>
        {/* Shipment Tracker Strip */}
        {shipment && (
          <div style={{
            background: '#f0fdf4',
            border: '1px solid #bbf7d0',
            borderRadius: '10px',
            padding: '0.75rem',
            marginBottom: '0.85rem',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'space-between',
            flexWrap: 'wrap',
            gap: '0.5rem'
          }}>
            <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
              <Truck size={18} color="#16a34a" />
              <div>
                <div style={{ fontSize: '0.8rem', fontWeight: 600, color: '#166534' }}>
                  Carrier: {shipment.carrier} • Tracking: <code style={{ background: '#dcfce7', padding: '0.1rem 0.3rem', borderRadius: '4px' }}>{shipment.trackingNumber || 'N/A'}</code>
                </div>
                {shipment.estimatedDeliveryUtc && (
                  <div style={{ fontSize: '0.7rem', color: '#15803d' }}>
                    Estimated Delivery: {new Date(shipment.estimatedDeliveryUtc).toLocaleDateString(undefined, { dateStyle: 'long' })}
                  </div>
                )}
              </div>
            </div>
            {shipment.trackingUrl && (
              <a
                href={shipment.trackingUrl}
                target="_blank"
                rel="noreferrer"
                style={{
                  fontSize: '0.75rem',
                  fontWeight: 600,
                  color: '#15803d',
                  display: 'flex',
                  alignItems: 'center',
                  gap: '0.25rem',
                  textDecoration: 'none'
                }}
              >
                Track Live <ExternalLink size={12} />
              </a>
            )}
          </div>
        )}

        {/* Items List */}
        {order.items && order.items.length > 0 && (
          <div style={{ marginBottom: '0.85rem' }}>
            <div style={{ fontSize: '0.75rem', fontWeight: 700, color: '#475569', textTransform: 'uppercase', letterSpacing: '0.04em', marginBottom: '0.4rem' }}>
              Purchased Items ({order.items.length})
            </div>
            <div style={{ border: '1px solid #f1f5f9', borderRadius: '8px', overflow: 'hidden' }}>
              {order.items.map((item, idx) => (
                <div key={idx} style={{
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'space-between',
                  padding: '0.5rem 0.75rem',
                  borderBottom: idx === order.items.length - 1 ? 'none' : '1px solid #f8fafc',
                  background: idx % 2 === 0 ? '#ffffff' : '#fcfcfd'
                }}>
                  <div>
                    <div style={{ fontSize: '0.85rem', fontWeight: 600, color: '#1e293b' }}>
                      {item.productName}
                    </div>
                    <div style={{ fontSize: '0.7rem', color: '#64748b' }}>
                      SKU: {item.sku} • Qty: {item.quantity} × ₹{item.unitPrice.toFixed(2)}
                    </div>
                  </div>
                  <div style={{ fontSize: '0.85rem', fontWeight: 700, color: '#0f172a' }}>
                    ₹{item.lineTotal.toFixed(2)}
                  </div>
                </div>
              ))}
            </div>
          </div>
        )}

        {/* Pricing Summary & Address */}
        <div style={{
          display: 'grid',
          gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))',
          gap: '0.75rem',
          padding: '0.75rem',
          background: '#f8fafc',
          borderRadius: '10px'
        }}>
          {/* Shipping Address */}
          <div>
            <div style={{ display: 'flex', alignItems: 'center', gap: '0.3rem', fontSize: '0.75rem', fontWeight: 700, color: '#475569', marginBottom: '0.2rem' }}>
              <MapPin size={13} /> Delivery Address
            </div>
            <div style={{ fontSize: '0.8rem', color: '#334155', lineHeight: 1.3 }}>
              {order.shippingAddress || 'Standard delivery address'}
            </div>
          </div>

          {/* Payment Info */}
          {payment && (
            <div>
              <div style={{ display: 'flex', alignItems: 'center', gap: '0.3rem', fontSize: '0.75rem', fontWeight: 700, color: '#475569', marginBottom: '0.2rem' }}>
                <CreditCard size={13} /> Payment ({payment.paymentMethod})
              </div>
              <div style={{ fontSize: '0.8rem', color: '#334155' }}>
                Status: <strong style={{ color: payment.paymentStatus === 'Paid' ? '#16a34a' : '#d97706' }}>{payment.paymentStatus}</strong>
              </div>
              <div style={{ fontSize: '0.7rem', color: '#64748b' }}>
                Ref: {payment.paymentReference}
              </div>
            </div>
          )}

          {/* Price Breakdown */}
          <div style={{ borderLeft: '1px solid #e2e8f0', paddingLeft: '0.75rem' }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: '0.75rem', color: '#64748b' }}>
              <span>Subtotal:</span>
              <span>₹{order.subtotal?.toFixed(2)}</span>
            </div>
            {order.shipping > 0 && (
              <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: '0.75rem', color: '#64748b' }}>
                <span>Shipping:</span>
                <span>₹{order.shipping?.toFixed(2)}</span>
              </div>
            )}
            {order.tax > 0 && (
              <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: '0.75rem', color: '#64748b' }}>
                <span>Tax:</span>
                <span>₹{order.tax?.toFixed(2)}</span>
              </div>
            )}
            <div style={{
              display: 'flex',
              justifyContent: 'space-between',
              fontSize: '0.9rem',
              fontWeight: 800,
              color: '#0f172a',
              marginTop: '0.3rem',
              paddingTop: '0.3rem',
              borderTop: '1px solid #cbd5e1'
            }}>
              <span>Total Paid:</span>
              <span>₹{order.total?.toFixed(2)}</span>
            </div>
          </div>
        </div>

        {/* Status History Accordion */}
        {order.statusHistory && order.statusHistory.length > 0 && (
          <div style={{ marginTop: '0.75rem' }}>
            <button
              onClick={() => setShowHistory(!showHistory)}
              style={{
                background: 'none',
                border: 'none',
                color: '#2563eb',
                fontSize: '0.75rem',
                fontWeight: 600,
                display: 'flex',
                alignItems: 'center',
                gap: '0.3rem',
                cursor: 'pointer',
                padding: '0.2rem 0'
              }}
            >
              <Clock size={13} />
              {showHistory ? 'Hide Status History' : `View Status History (${order.statusHistory.length} events)`}
              {showHistory ? <ChevronUp size={13} /> : <ChevronDown size={13} />}
            </button>

            {showHistory && (
              <div style={{
                marginTop: '0.5rem',
                borderLeft: '2px solid #e2e8f0',
                marginLeft: '0.5rem',
                paddingLeft: '0.75rem',
                display: 'flex',
                flexDirection: 'column',
                gap: '0.5rem'
              }}>
                {order.statusHistory.map((h, i) => (
                  <div key={i} style={{ fontSize: '0.75rem' }}>
                    <div style={{ display: 'flex', alignItems: 'center', gap: '0.4rem' }}>
                      <span style={{ fontWeight: 700, color: '#1e293b' }}>{h.status}</span>
                      <span style={{ color: '#94a3b8' }}>•</span>
                      <span style={{ color: '#64748b' }}>
                        {new Date(h.changedAtUtc).toLocaleString(undefined, { dateStyle: 'short', timeStyle: 'short' })}
                      </span>
                    </div>
                    {h.statusDescription && (
                      <div style={{ color: '#475569', marginTop: '0.1rem' }}>
                        {h.statusDescription}
                      </div>
                    )}
                  </div>
                ))}
              </div>
            )}
          </div>
        )}
      </div>
    </div>
  );
}
