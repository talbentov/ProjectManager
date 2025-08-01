import React, { useEffect, useState } from "react";
import api from "../api/apiService";
import type { SimpleProject, DetailedProject, Task, CreateProjectDto, CreateTaskDto, UpdateTaskDto } from "../types/types";

const ProjectsPage: React.FC = () => {
    const [projects, setProjects] = useState<SimpleProject[]>([]);
    const [selectedProject, setSelectedProject] = useState<DetailedProject | null>(null);
    const [newProject, setNewProject] = useState("");
    const [error, setError] = useState("");

    useEffect(() => {
        api.get<SimpleProject[]>("/projects")
            .then(res => setProjects(res.data))
            .catch(() => setError("Failed to load projects"));
    }, []);

    const fetchProjectDetails = async (id: number) => {
        try {
            const res = await api.get<DetailedProject>(`/projects/${id}`);
            setSelectedProject(res.data);
        } catch {
            setError("Failed to load project details");
        }
    };

    const createProject = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!newProject.trim()) return setError("Project title required");
        try {
            const dto: CreateProjectDto = { title: newProject };
            const res = await api.post<SimpleProject>("/projects", dto);
            setProjects([...projects, res.data]);
            setNewProject("");
        } catch {
            setError("Failed to create project");
        }
    };

    const deleteProject = async (id: number) => {
        try {
            await api.delete(`/projects/${id}`);
            setProjects(projects.filter(p => p.id !== id));
            if (selectedProject?.id === id) setSelectedProject(null);
        } catch {
            setError("Failed to delete project");
        }
    };

    // Task handlers
    const addTask = async (title: string, dueDate?: string) => {
        if (!selectedProject) return;
        if (!title.trim()) return setError("Task title required");
        try {
            const dto: CreateTaskDto = { title, dueDate };
            const res = await api.post<Task>(`/projects/${selectedProject.id}/tasks`, dto);
            setSelectedProject({
                ...selectedProject,
                tasks: [...selectedProject.tasks, res.data]
            });
        } catch {
            setError("Failed to add task");
        }
    };

    const updateTask = async (task: Task) => {
        if (!selectedProject) return;
        try {
            const dto: UpdateTaskDto = {
                title: task.title,
                dueDate: task.dueDate,
                isCompleted: task.isCompleted
            };
            await api.put(`/projects/${selectedProject.id}/tasks/${task.id}`, dto);
            setSelectedProject({
                ...selectedProject,
                tasks: selectedProject.tasks.map(t => t.id === task.id ? { ...task } : t)
            });
        } catch {
            setError("Failed to update task");
        }
    };

    const deleteTask = async (taskId: number) => {
        if (!selectedProject) return;
        try {
            await api.delete(`/projects/${selectedProject.id}/tasks/${taskId}`);
            setSelectedProject({
                ...selectedProject,
                tasks: selectedProject.tasks.filter(t => t.id !== taskId)
            });
        } catch {
            setError("Failed to delete task");
        }
    };

    const toggleTask = (task: Task) => {
        updateTask({ ...task, isCompleted: !task.isCompleted });
    };

    return (
        <div>
            <h1>Projects</h1>
            {error && <div style={{ color: "red" }}>{error}</div>}
            <form onSubmit={createProject}>
                <input
                    value={newProject}
                    onChange={e => setNewProject(e.target.value)}
                    placeholder="New project"
                    required
                />
                <button type="submit">Add Project</button>
            </form>
            <ul>
                {projects.map(project => (
                    <li key={project.id}>
                        <span style={{ cursor: "pointer" }} onClick={() => fetchProjectDetails(project.id)}>
                            {project.title}
                        </span>
                        <button onClick={() => deleteProject(project.id)}>Delete</button>
                    </li>
                ))}
            </ul>
            {selectedProject && (
                <ProjectDetails
                    project={selectedProject}
                    addTask={addTask}
                    updateTask={updateTask}
                    deleteTask={deleteTask}
                    toggleTask={toggleTask}
                />
            )}
        </div>
    );
};

const ProjectDetails: React.FC<{
    project: DetailedProject;
    addTask: (title: string, dueDate?: string) => void;
    updateTask: (task: Task) => void;
    deleteTask: (taskId: number) => void;
    toggleTask: (task: Task) => void;
}> = ({ project, addTask, updateTask, deleteTask, toggleTask }) => {
    const [newTask, setNewTask] = useState("");
    const [newDueDate, setNewDueDate] = useState("");
    const [editTaskId, setEditTaskId] = useState<number | null>(null);
    const [editTitle, setEditTitle] = useState("");
    const [editDueDate, setEditDueDate] = useState("");

    return (
        <div>
            <h2>{project.title}</h2>
            <form onSubmit={e => {
                e.preventDefault();
                addTask(newTask, newDueDate);
                setNewTask("");
                setNewDueDate("");
            }}>
                <input
                    value={newTask}
                    onChange={e => setNewTask(e.target.value)}
                    placeholder="New task"
                    required
                />
                <input
                    type="date"
                    value={newDueDate}
                    onChange={e => setNewDueDate(e.target.value)}
                />
                <button type="submit">Add Task</button>
            </form>
            <ul>
                {project.tasks.map(task => (
                    <li key={task.id}>
                        {editTaskId === task.id ? (
                            <>
                                <input value={editTitle} onChange={e => setEditTitle(e.target.value)} />
                                <input type="date" value={editDueDate} onChange={e => setEditDueDate(e.target.value)} />
                                <button onClick={() => {
                                    updateTask({ ...task, title: editTitle, dueDate: editDueDate });
                                    setEditTaskId(null);
                                }}>Save</button>
                                <button onClick={() => setEditTaskId(null)}>Cancel</button>
                            </>
                        ) : (
                            <>
                                <span
                                    style={{
                                        textDecoration: task.isCompleted ? "line-through" : "none",
                                        cursor: "pointer"
                                    }}
                                    onClick={() => toggleTask(task)}
                                >
                                    {task.title}
                                </span>
                                {task.dueDate && <span>(Due: {task.dueDate})</span>}
                                <button onClick={() => {
                                    setEditTaskId(task.id);
                                    setEditTitle(task.title);
                                    setEditDueDate(task.dueDate || "");
                                }}>Edit</button>
                                <button onClick={() => deleteTask(task.id)}>Delete</button>
                            </>
                        )}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default ProjectsPage;