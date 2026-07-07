import { apiClient } from "./apiClient";

const API = process.env.REACT_APP_API_URL;

export const getDashboardApi = async()=> {
        const res =
        await apiClient.get('/api/dashboard');
    return res.data;
}