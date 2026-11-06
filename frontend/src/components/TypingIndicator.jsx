import React from 'react';
import { Bot, Sparkles } from 'lucide-react';

export default function TypingIndicator() {
  return (
    <div
      className="animate-fade-in"
      style={{
        display: 'flex',
        alignItems: 'center',
        gap: '0.75rem',
        marginBottom: '1rem',
        padding: '0 0.5rem'
      }}
    >
      <div style={{
        width: '34px',
        height: '34px',
        borderRadius: '10px',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        background: '#ffffff',
        color: '#4f46e5',
        border: '1px solid #e2e8f0',
        boxShadow: '0 1px 3px rgba(0,0,0,0.06)'
      }}>
        <Bot size={19} />
      </div>

      <div style={{
        background: '#ffffff',
        border: '1px solid #e2e8f0',
        padding: '0.65rem 1rem',
        borderRadius: '16px 16px 16px 4px',
        boxShadow: '0 1px 3px rgba(0,0,0,0.04)',
        display: 'flex',
        alignItems: 'center',
        gap: '0.5rem',
        fontSize: '0.8rem',
        color: '#64748b'
      }}>
        <Sparkles size={14} color="#6366f1" className="animate-spin" />
        <span>Executing AI backend tools...</span>
        <div style={{ display: 'flex', gap: '3px', marginLeft: '0.3rem' }}>
          <span className="typing-dot-1" style={{ width: '5px', height: '5px', borderRadius: '50%', background: '#6366f1' }} />
          <span className="typing-dot-2" style={{ width: '5px', height: '5px', borderRadius: '50%', background: '#6366f1' }} />
          <span className="typing-dot-3" style={{ width: '5px', height: '5px', borderRadius: '50%', background: '#6366f1' }} />
        </div>
      </div>
    </div>
  );
}

