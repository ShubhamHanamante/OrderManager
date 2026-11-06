import React from 'react';
import { Bot, User, Wrench, CheckCircle, AlertCircle } from 'lucide-react';
import OrderCard from './OrderCard';
import OrderSummaryList from './OrderSummaryList';

// Simple markdown formatter for bold, bullets, code, and linebreaks
function formatContent(text) {
  if (!text) return null;

  // Split lines
  const lines = text.split('\n');
  return lines.map((line, idx) => {
    // Bullet points
    if (line.trim().startsWith('- ') || line.trim().startsWith('* ')) {
      const content = line.trim().substring(2);
      return (
        <li key={idx} style={{ marginLeft: '1.25rem', marginBottom: '0.25rem' }}>
          {renderFormattedInline(content)}
        </li>
      );
    }

    // Numbered list
    const matchNumbered = line.trim().match(/^(\d+)\.\s+(.*)/);
    if (matchNumbered) {
      return (
        <li key={idx} style={{ marginLeft: '1.25rem', marginBottom: '0.25rem' }}>
          {renderFormattedInline(matchNumbered[2])}
        </li>
      );
    }

    if (line.trim() === '') {
      return <div key={idx} style={{ height: '0.5rem' }} />;
    }

    return (
      <p key={idx} style={{ marginBottom: '0.35rem', lineHeight: 1.55 }}>
        {renderFormattedInline(line)}
      </p>
    );
  });
}

function renderFormattedInline(str) {
  // Bold **text**
  const parts = str.split(/(\*\*.*?\*\*|`.*?`)/g);
  return parts.map((part, i) => {
    if (part.startsWith('**') && part.endsWith('**')) {
      return <strong key={i} style={{ fontWeight: 700, color: '#0f172a' }}>{part.slice(2, -2)}</strong>;
    }
    if (part.startsWith('`') && part.endsWith('`')) {
      return (
        <code key={i} style={{
          background: '#f1f5f9',
          color: '#0f172a',
          padding: '0.1rem 0.35rem',
          borderRadius: '4px',
          fontFamily: 'JetBrains Mono, monospace',
          fontSize: '0.85em'
        }}>
          {part.slice(1, -1)}
        </code>
      );
    }
    return part;
  });
}

export default function ChatMessage({ message, onSelectOrder }) {
  const isUser = message.role === 'user';

  return (
    <div
      className="animate-fade-in"
      style={{
        display: 'flex',
        justifyContent: isUser ? 'flex-end' : 'flex-start',
        marginBottom: '1.25rem',
        padding: '0 0.5rem'
      }}
    >
      <div style={{
        display: 'flex',
        gap: '0.75rem',
        maxWidth: isUser ? '80%' : '88%',
        flexDirection: isUser ? 'row-reverse' : 'row'
      }}>
        {/* Avatar */}
        <div style={{
          width: '34px',
          height: '34px',
          borderRadius: '10px',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          flexShrink: 0,
          background: isUser ? '#2563eb' : '#ffffff',
          color: isUser ? '#ffffff' : '#4f46e5',
          border: isUser ? 'none' : '1px solid #e2e8f0',
          boxShadow: '0 1px 3px rgba(0,0,0,0.06)'
        }}>
          {isUser ? <User size={18} /> : <Bot size={19} />}
        </div>

        {/* Content Bubble */}
        <div style={{ flex: 1, minWidth: 0 }}>
          {/* Tool Execution Badges */}
          {!isUser && message.toolsUsed && message.toolsUsed.length > 0 && (
            <div style={{
              display: 'flex',
              flexWrap: 'wrap',
              gap: '0.4rem',
              marginBottom: '0.5rem'
            }}>
              {message.toolsUsed.map((t, i) => (
                <span
                  key={i}
                  style={{
                    background: '#f0fdf4',
                    border: '1px solid #bbf7d0',
                    color: '#15803d',
                    fontSize: '0.7rem',
                    fontWeight: 600,
                    padding: '0.15rem 0.5rem',
                    borderRadius: '6px',
                    display: 'inline-flex',
                    alignItems: 'center',
                    gap: '0.3rem',
                    fontFamily: 'JetBrains Mono, monospace'
                  }}
                >
                  <Wrench size={11} />
                  <span>{t.toolName}()</span>
                  {t.success && <CheckCircle size={11} color="#16a34a" />}
                </span>
              ))}
            </div>
          )}

          <div style={{
            background: isUser ? '#2563eb' : '#ffffff',
            color: isUser ? '#ffffff' : '#334155',
            padding: '0.85rem 1.15rem',
            borderRadius: isUser ? '16px 16px 4px 16px' : '16px 16px 16px 4px',
            border: isUser ? 'none' : '1px solid #e2e8f0',
            boxShadow: '0 1px 3px 0 rgba(0, 0, 0, 0.04)',
            fontSize: '0.9rem'
          }}>
            {formatContent(message.content)}
          </div>

          {/* Structured Data Embedded Cards */}
          {!isUser && message.structuredData && (
            <>
              {/* Single Order Card */}
              {message.structuredData.orderNumber && (
                <OrderCard order={message.structuredData} />
              )}

              {/* Order List Summaries */}
              {Array.isArray(message.structuredData) && message.structuredData.length > 0 && message.structuredData[0].orderNumber && (
                <OrderSummaryList
                  orders={message.structuredData}
                  onSelectOrder={onSelectOrder}
                />
              )}
            </>
          )}

          {/* Timestamp */}
          <div style={{
            fontSize: '0.65rem',
            color: '#94a3b8',
            marginTop: '0.3rem',
            textAlign: isUser ? 'right' : 'left',
            padding: '0 0.2rem'
          }}>
            {message.timestamp ? new Date(message.timestamp).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) : ''}
          </div>
        </div>
      </div>
    </div>
  );
}

