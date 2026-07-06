import { apiClient } from "./apiClient";

export interface LoginRequest {
  email: string;
  password: string;
  rememberMe?: boolean;
}

export interface RegisterRequest {
  email: string;
  password: string;
  fullName: string;
}

export interface AuthResponse {
  token: string;
  refreshToken: string;
  email: string;
  fullName: string;
}

export const loginUserApi = async (
  data: LoginRequest
): Promise<AuthResponse> => {
  const res = await apiClient.post("/api/auth/login", data);
  return res.data;
};

export const registerUserApi = async (
  data: RegisterRequest
): Promise<AuthResponse> => {
  const res = await apiClient.post("/api/auth/register", data);
  return res.data;
};

export const getProfileApi = async () => {
  const res = await apiClient.get("/api/auth/me");
  return res.data;
};