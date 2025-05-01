// ChatBot.jsx
import React, { useState } from "react";

function ChatBot() {
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
        <div style={{ maxWidth: 600, margin: "0 auto", padding: 20 }}>
            <h2>AgroMind Chatbot</h2>
            <div style={{ minHeight: 200, border: "1px solid #ccc", padding: 10, marginBottom: 10 }}>
                {messages.map((msg, i) => (
                    <div key={i} style={{ marginBottom: 8 }}>
                        <b>You:</b> {msg.user}
                        <br />
                        <b>Bot:</b> {msg.bot}
                    </div>
                ))}
            </div>
            <input
                value={input}
                onChange={e => setInput(e.target.value)}
                style={{ width: "80%", marginRight: 8 }}
                onKeyDown={e => { if (e.key === "Enter") sendMessage(); }}
                placeholder="Ask an agricultural question..."
            />
            <button onClick={sendMessage}>Send</button>
        </div>
    );
}

export default ChatBot;