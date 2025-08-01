import React, { useState } from "react";
import api from "../api/apiService";
import { useAuth } from "../auth/AuthProvider";
import type { User } from "../types/types";

const RegisterPage: React.FC = () => {
    const { login } = useAuth();
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError("");
        try {
            const res = await api.post<User>("/register", { username, password });
            login(res.data);
        } catch {
            setError("Registration failed");
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <h2>Register</h2>
            {error && <div style={{ color: "red" }}>{error}</div>}
            <input
                value={username}
                onChange={e => setUsername(e.target.value)}
                placeholder="Username"
                required
            />
            <input
                type="password"
                value={password}
                onChange={e => setPassword(e.target.value)}
                placeholder="Password"
                required
            />
            <button type="submit">Register</button>
        </form>
    );
};

export default RegisterPage;