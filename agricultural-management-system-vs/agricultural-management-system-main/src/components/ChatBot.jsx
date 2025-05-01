import React, { useState } from "react";

function ChatBot() {
    const [open, setOpen] = useState(false);
    const [input, setInput] = useState("");
    const [messages, setMessages] = useState([]);

    const sendMessage = async () => {
        if (!input.trim()) return;
        setMessages([...messages, { user: input, bot: "..." }]);
        try {
            const res = await fetch("http://localhost:5000/generate", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ prompt: input }),
            });
            if (!res.ok) throw new Error("Server error");
            const data = await res.json();
            setMessages((prev) =>
                prev.slice(0, -1).concat({ user: input, bot: data.response })
            );
        } catch (err) {
            setMessages((prev) =>
                prev.slice(0, -1).concat({ user: input, bot: "Sorry, I couldn't reach the AI server." })
            );
        }
        setInput("");
    };

    return (
        <>
            {/* Floating Chat Icon */}
            <button
                onClick={() => setOpen((o) => !o)}
                style={{
                    position: "fixed",
                    bottom: 24,
                    right: 24,
                    zIndex: 1000,
                    borderRadius: "50%",
                    width: 56,
                    height: 56,
                    background: "#1976d2",
                    color: "#fff",
                    border: "none",
                    fontSize: 28,
                    boxShadow: "0 2px 8px rgba(0,0,0,0.2)",
                    cursor: "pointer"
                }}
                aria-label="Open chat"
            >
                💬
            </button>
            {/* Chat Window */}
            {open && (
                <div
                    style={{
                        position: "fixed",
                        bottom: 90,
                        right: 24,
                        width: 350,
                        maxHeight: 500,
                        background: "#fff",
                        borderRadius: 8,
                        boxShadow: "0 2px 16px rgba(0,0,0,0.3)",
                        zIndex: 1000,
                        display: "flex",
                        flexDirection: "column"
                    }}
                >
                    <div style={{ padding: 12, borderBottom: "1px solid #eee", background: "#1976d2", color: "#fff", borderTopLeftRadius: 8, borderTopRightRadius: 8 }}>
                        AgroMind Chatbot
                        <button onClick={() => setOpen(false)} style={{ float: "right", background: "none", border: "none", color: "#fff", fontSize: 18, cursor: "pointer" }}>×</button>
                    </div>
                    <div style={{ flex: 1, overflowY: "auto", padding: 12 }}>
                        {messages.map((msg, i) => (
                            <div key={i} style={{ marginBottom: 8 }}>
                                <b>You:</b> {msg.user}
                                <br />
                                <b>Bot:</b> {msg.bot}
                            </div>
                        ))}
                    </div>
                    <div style={{ display: "flex", borderTop: "1px solid #eee", padding: 8 }}>
                        <input
                            value={input}
                            onChange={e => setInput(e.target.value)}
                            style={{ flex: 1, marginRight: 8, borderRadius: 4, border: "1px solid #ccc", padding: 6 }}
                            onKeyDown={e => { if (e.key === "Enter") sendMessage(); }}
                            placeholder="Ask an agricultural question..."
                        />
                        <button onClick={sendMessage} style={{ borderRadius: 4, background: "#1976d2", color: "#fff", border: "none", padding: "6px 12px" }}>Send</button>
                    </div>
                </div>
            )}
        </>
    );
}

export default ChatBot;
