import React from "react";
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import { AuthProvider, useAuth } from "./auth/AuthProvider";
import ProjectsPage from "./pages/ProjectsPage";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";

const PrivateRoute: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const { user } = useAuth();
    return user ? <>{children}</> : <Navigate to="/api/auth/register" />;
};

const App: React.FC = () => (
    <AuthProvider>
        <Router>
            <Routes>
                <Route path="/api/auth/login" element={<LoginPage />} />
                <Route path="/api/auth/register" element={<RegisterPage />} /> 
                <Route path="/api/projects" element={<ProjectsPage />} /> 
                <Route path="/" element={
                    <PrivateRoute>
                        <ProjectsPage />
                    </PrivateRoute>
                } />
            </Routes>
        </Router>
    </AuthProvider>
);

export default App;