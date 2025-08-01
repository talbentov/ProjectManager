
export interface User {
    username: string;
    token: string;
}

export interface SimpleProject {
    id: number;
    title: string;
    description?: string;
    creationDate: string;
    taskCount: number;
    completedTaskCount: number;
}

export interface DetailedProject {
    id: number;
    title: string;
    description?: string;
    creationDate: string;
    tasks: Task[];
}

export interface Task {
    id: number;
    title: string;
    dueDate?: string;
    isCompleted: boolean;
    projectId: number;
}

export interface CreateProjectDto {
    title: string;
    description?: string;
}

export interface CreateTaskDto {
    title: string;
    dueDate?: string;
}

export interface UpdateTaskDto {
    title: string;
    dueDate?: string;
    isCompleted: boolean;
}