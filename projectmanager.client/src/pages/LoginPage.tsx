import React, { useState } from "react";
import api from "../api/apiService";
import { useAuth } from "../auth/AuthProvider";
import type { User } from "../types/types";
import { Link } from "react-router-dom";

const LoginPage: React.FC = () => {
    const { login } = useAuth();
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setError("");
        try {
            const res = await api.post<User>("/auth/login", { username, password });
            login(res.data);
        } catch {
            setError("Invalid credentials");
        }
    };

    return (
        <div style={{ maxWidth: 400, margin: "2rem auto", padding: 24, border: "1px solid #ccc", borderRadius: 8 }}>
            <h2>Login</h2>
            <form onSubmit={handleSubmit} autoComplete="off">
                <div style={{ marginBottom: 12 }}>
                    <label htmlFor="username" style={{ display: "block", marginBottom: 4 }}>Username</label>
                    <input
                        id="username"
                        type="text"
                        value={username}
                        onChange={e => setUsername(e.target.value)}
                        placeholder="Username"
                        required
                        style={{ width: "100%", padding: 8 }}
                    />
                </div>
                <div style={{ marginBottom: 12 }}>
                    <label htmlFor="password" style={{ display: "block", marginBottom: 4 }}>Password</label>
                    <input
                        id="password"
                        type="password"
                        value={password}
                        onChange={e => setPassword(e.target.value)}
                        placeholder="Password"
                        required
                        style={{ width: "100%", padding: 8 }}
                    />
                </div>
                {error && <div style={{ color: "red", marginBottom: 12 }}>{error}</div>}
                <button type="submit" style={{ width: "100%", padding: 10 }}>Login</button>
                <p>Don't have an account? <Link to="/register">Register here</Link></p>
            </form>
        </div>
    );
};

export default LoginPage;