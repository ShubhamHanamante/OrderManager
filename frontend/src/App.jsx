import React, { useState, useEffect, useRef } from 'react';
import { Send, RotateCcw, AlertCircle, ShoppingBag, ShieldAlert } from 'lucide-react';
import Header from './components/Header';
import ChatMessage from './components/ChatMessage';
import TypingIndicator from './components/TypingIndicator';
import QuickPrompts from './components/QuickPrompts';
import { CUSTOMERS, SUGGESTED_PROMPTS } from './data/customers';
import { sendChatMessage } from './services/api';

export default function App() {
  const [activeCustomer, setActiveCustomer] = useState(CUSTOMERS[0]);
  const [messages, setMessages] = useState([]);
  const [inputValue, setInputValue] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [errorBanner, setErrorBanner] = useState(null);

  const messagesEndRef = useRef(null);

  // Initialize conversation greeting whenever active customer changes
  useEffect(() => {
    setMessages([
      {
        id: 'welcome',
        role: 'assistant',
        content: `Hello **${activeCustomer.name}**! 👋 Welcome to your Order Support Assistant.\n\nI can help you check live shipment tracking, order status, purchase history, item breakdowns, or payment details. What would you like to know today?`,
        timestamp: new Date().toISOString()
      }
    ]);
    setErrorBanner(null);
  }, [activeCustomer]);

  // Scroll to bottom on new message
  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages, isLoading]);

  const handleSendMessage = async (textToSend) => {
    const text = (textToSend || inputValue).trim();
    if (!text || isLoading) return;

    setErrorBanner(null);
    setInputValue('');

    const userMessage = {
      id: Date.now().toString(),
      role: 'user',
      content: text,
      timestamp: new Date().toISOString()
    };

    setMessages((prev) => [...prev, userMessage]);
    setIsLoading(true);

    try {
      // Pass previous turns for multi-turn context
      const history = messages
        .filter((m) => m.id !== 'welcome')
        .map((m) => ({ role: m.role, content: m.content }));

      const response = await sendChatMessage(text, activeCustomer.id, history);

      const botMessage = {
        id: (Date.now() + 1).toString(),
        role: 'assistant',
        content: response.message || "I've retrieved your order information.",
        toolsUsed: response.toolsUsed,
        structuredData: response.structuredData,
        timestamp: new Date().toISOString()
      };

      setMessages((prev) => [...prev, botMessage]);
    } catch (err) {
      console.error('Chat error:', err);
      setErrorBanner(err.message || 'Failed to communicate with AI backend.');
      setMessages((prev) => [
        ...prev,
        {
          id: (Date.now() + 1).toString(),
          role: 'assistant',
          content: `⚠️ **Unable to process query:** ${err.message || 'The AI service encountered an issue. Please verify that Ollama is running.'}`,
          timestamp: new Date().toISOString()
        }
      ]);
    } finally {
      setIsLoading(false);
    }
  };

  const handleClearHistory = () => {
    setMessages([
      {
        id: Date.now().toString(),
        role: 'assistant',
        content: `Conversation refreshed for **${activeCustomer.name}**. How can I help you?`,
        timestamp: new Date().toISOString()
      }
    ]);
    setErrorBanner(null);
  };

  return (
    <div style={{ display: 'flex', flexDirection: 'column', height: '100vh', background: '#f8fafc' }}>
      {/* Top Header */}
      <Header
        activeCustomer={activeCustomer}
        onSelectCustomer={setActiveCustomer}
        customers={CUSTOMERS}
      />

      {/* Main Chat Container */}
      <main style={{
        flex: 1,
        maxWidth: '960px',
        width: '100%',
        margin: '0 auto',
        display: 'flex',
        flexDirection: 'column',
        height: 'calc(100vh - 75px)',
        padding: '1rem',
        overflow: 'hidden'
      }}>
        {/* Error Notification Banner */}
        {errorBanner && (
          <div style={{
            background: '#fef2f2',
            border: '1px solid #fecaca',
            color: '#991b1b',
            padding: '0.65rem 1rem',
            borderRadius: '10px',
            fontSize: '0.8rem',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'space-between',
            marginBottom: '0.75rem'
          }}>
            <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
              <AlertCircle size={16} />
              <span>{errorBanner}</span>
            </div>
            <button
              onClick={() => setErrorBanner(null)}
              style={{ background: 'none', border: 'none', color: '#991b1b', cursor: 'pointer', fontWeight: 700 }}
            >
              ✕
            </button>
          </div>
        )}

        {/* Message Feed */}
        <div style={{
          flex: 1,
          overflowY: 'auto',
          paddingRight: '0.5rem',
          paddingBottom: '1rem'
        }}>
          {messages.map((msg) => (
            <ChatMessage
              key={msg.id}
              message={msg}
              onSelectOrder={(orderNum) => handleSendMessage(`Where is my order ${orderNum}?`)}
            />
          ))}

          {isLoading && <TypingIndicator />}
          <div ref={messagesEndRef} />
        </div>

        {/* Bottom Input Area */}
        <div style={{
          background: '#ffffff',
          borderRadius: '16px',
          border: '1px solid #e2e8f0',
          padding: '0.75rem 1rem',
          boxShadow: '0 4px 12px rgba(0, 0, 0, 0.05)',
          display: 'flex',
          flexDirection: 'column',
          gap: '0.5rem'
        }}>
          {/* Quick Prompts */}
          <QuickPrompts
            prompts={SUGGESTED_PROMPTS}
            onSelectPrompt={(prompt) => handleSendMessage(prompt)}
            disabled={isLoading}
          />

          {/* Text Input Row */}
          <form
            onSubmit={(e) => {
              e.preventDefault();
              handleSendMessage();
            }}
            style={{ display: 'flex', alignItems: 'center', gap: '0.65rem' }}
          >
            <input
              type="text"
              placeholder={`Ask about orders, tracking, or payments for ${activeCustomer.name}...`}
              value={inputValue}
              onChange={(e) => setInputValue(e.target.value)}
              disabled={isLoading}
              style={{
                flex: 1,
                border: 'none',
                outline: 'none',
                fontSize: '0.9rem',
                color: '#0f172a',
                padding: '0.4rem 0.2rem'
              }}
            />

            <button
              type="button"
              title="Reset Conversation"
              onClick={handleClearHistory}
              style={{
                background: '#f1f5f9',
                border: 'none',
                color: '#64748b',
                width: '36px',
                height: '36px',
                borderRadius: '8px',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                cursor: 'pointer',
                transition: 'all 0.15s ease'
              }}
            >
              <RotateCcw size={16} />
            </button>

            <button
              type="submit"
              disabled={isLoading || !inputValue.trim()}
              style={{
                background: inputValue.trim() && !isLoading ? '#2563eb' : '#94a3b8',
                border: 'none',
                color: '#ffffff',
                width: '38px',
                height: '38px',
                borderRadius: '10px',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                cursor: inputValue.trim() && !isLoading ? 'pointer' : 'not-allowed',
                transition: 'all 0.15s ease',
                boxShadow: inputValue.trim() && !isLoading ? '0 2px 4px rgba(37, 99, 235, 0.3)' : 'none'
              }}
            >
              <Send size={16} />
            </button>
          </form>
        </div>
      </main>
    </div>
  );
}

