import React from 'react';
import { Sparkles } from 'lucide-react';

export default function QuickPrompts({ prompts, onSelectPrompt, disabled }) {
  if (!prompts || prompts.length === 0) return null;

  return (
    <div style={{
      display: 'flex',
      alignItems: 'center',
      gap: '0.4rem',
      overflowX: 'auto',
      paddingBottom: '0.65rem',
      scrollbarWidth: 'none',
      msOverflowStyle: 'none'
    }}>
      <div style={{
        display: 'flex',
        alignItems: 'center',
        gap: '0.25rem',
        fontSize: '0.7rem',
        fontWeight: 700,
        color: '#6366f1',
        whiteSpace: 'nowrap',
        paddingRight: '0.25rem'
      }}>
        <Sparkles size={12} />
        <span>Suggestions:</span>
      </div>

      {prompts.map((p, i) => (
        <button
          key={i}
          disabled={disabled}
          onClick={() => onSelectPrompt(p)}
          style={{
            background: '#ffffff',
            border: '1px solid #e2e8f0',
            borderRadius: '9999px',
            padding: '0.3rem 0.75rem',
            fontSize: '0.75rem',
            color: '#334155',
            fontWeight: 500,
            whiteSpace: 'nowrap',
            cursor: disabled ? 'not-allowed' : 'pointer',
            transition: 'all 0.15s ease',
            boxShadow: '0 1px 2px rgba(0,0,0,0.03)',
            opacity: disabled ? 0.6 : 1
          }}
          onMouseEnter={(e) => {
            if (!disabled) {
              e.currentTarget.style.borderColor = '#93c5fd';
              e.currentTarget.style.background = '#eff6ff';
              e.currentTarget.style.color = '#1d4ed8';
            }
          }}
          onMouseLeave={(e) => {
            if (!disabled) {
              e.currentTarget.style.borderColor = '#e2e8f0';
              e.currentTarget.style.background = '#ffffff';
              e.currentTarget.style.color = '#334155';
            }
          }}
        >
          {p}
        </button>
      ))}
    </div>
  );
}

