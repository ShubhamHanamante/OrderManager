import React from 'react';
import { Bot, User, Cpu, ShieldCheck, Sparkles, ChevronDown } from 'lucide-react';

export default function Header({ activeCustomer, onSelectCustomer, customers }) {
  return (
    <header style={{
      background: '#ffffff',
      borderBottom: '1px solid #e2e8f0',
      padding: '0.85rem 1.5rem',
      position: 'sticky',
      top: 0,
      zIndex: 50,
      boxShadow: '0 1px 3px 0 rgba(0, 0, 0, 0.05)'
    }}>
      <div style={{
        maxWidth: '1200px',
        margin: '0 auto',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        flexWrap: 'wrap',
        gap: '1rem'
      }}>
        {/* Brand */}
        <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem' }}>
          <div style={{
            background: 'linear-gradient(135deg, #2563eb, #4f46e5)',
            color: '#fff',
            width: '42px',
            height: '42px',
            borderRadius: '12px',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            boxShadow: '0 4px 6px -1px rgba(37, 99, 235, 0.25)'
          }}>
            <Bot size={24} />
          </div>
          <div>
            <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
              <h1 style={{ fontSize: '1.15rem', fontWeight: 700, color: '#0f172a', letterSpacing: '-0.02em' }}>
                OrderManager AI
              </h1>
              <span style={{
                background: '#ecfdf5',
                color: '#059669',
                fontSize: '0.7rem',
                fontWeight: 600,
                padding: '0.15rem 0.5rem',
                borderRadius: '9999px',
                border: '1px solid #a7f3d0',
                display: 'flex',
                alignItems: 'center',
                gap: '0.25rem'
              }}>
                <span style={{ width: '6px', height: '6px', borderRadius: '9999px', background: '#10b981' }} />
                Online
              </span>
            </div>
            <p style={{ fontSize: '0.8rem', color: '#64748b' }}>
              Autonomous E-Commerce Order Assistant
            </p>
          </div>
        </div>

        {/* Runtime Badges & Customer Switcher */}
        <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem', flexWrap: 'wrap' }}>
          {/* Local Ollama Runtime Badge */}
          <div style={{
            display: 'flex',
            alignItems: 'center',
            gap: '0.4rem',
            background: '#f1f5f9',
            border: '1px solid #cbd5e1',
            padding: '0.35rem 0.75rem',
            borderRadius: '8px',
            fontSize: '0.75rem',
            color: '#334155'
          }}>
            <Cpu size={14} color="#6366f1" />
            <span>Ollama: <strong>qwen2.5:14b</strong></span>
          </div>

          {/* Backend Security Badge */}
          <div style={{
            display: 'flex',
            alignItems: 'center',
            gap: '0.4rem',
            background: '#eff6ff',
            border: '1px solid #bfdbfe',
            padding: '0.35rem 0.75rem',
            borderRadius: '8px',
            fontSize: '0.75rem',
            color: '#1d4ed8'
          }}>
            <ShieldCheck size={14} color="#2563eb" />
            <span>Isolated Backend Tools</span>
          </div>

          {/* Customer Switcher */}
          <div style={{ position: 'relative' }}>
            <div style={{
              display: 'flex',
              alignItems: 'center',
              gap: '0.5rem',
              background: '#ffffff',
              border: '1.5px solid #e2e8f0',
              padding: '0.35rem 0.75rem',
              borderRadius: '10px',
              cursor: 'pointer',
              transition: 'all 0.15s ease'
            }}>
              <div style={{
                background: '#e0e7ff',
                color: '#4338ca',
                width: '28px',
                height: '28px',
                borderRadius: '50%',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                fontWeight: 700,
                fontSize: '0.75rem'
              }}>
                {activeCustomer.name.charAt(0)}
              </div>
              <div style={{ textAlign: 'left' }}>
                <div style={{ fontSize: '0.75rem', fontWeight: 600, color: '#0f172a' }}>
                  {activeCustomer.name}
                </div>
                <div style={{ fontSize: '0.65rem', color: '#64748b' }}>
                  {activeCustomer.code} • {activeCustomer.location}
                </div>
              </div>
              <select
                value={activeCustomer.id}
                onChange={(e) => {
                  const cust = customers.find(c => c.id === Number(e.target.value));
                  if (cust) onSelectCustomer(cust);
                }}
                style={{
                  position: 'absolute',
                  top: 0,
                  left: 0,
                  width: '100%',
                  height: '100%',
                  opacity: 0,
                  cursor: 'pointer'
                }}
              >
                {customers.map((c) => (
                  <option key={c.id} value={c.id}>
                    {c.name} ({c.code}) - {c.location}
                  </option>
                ))}
              </select>
              <ChevronDown size={14} color="#64748b" />
            </div>
          </div>
        </div>
      </div>
    </header>
  );
}

