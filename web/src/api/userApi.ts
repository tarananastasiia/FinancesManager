import axios from "axios";

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
  email: string;
  fullName: string;
}

axios.defaults.headers.common["Authorization"] =
  `Bearer ${localStorage.getItem("token")}`;
  
const API_URL = process.env.REACT_APP_API_URL;

export const loginUserApi = async (
  data: LoginRequest
): Promise<AuthResponse> => {

  const res = await axios.post(
    `${API_URL}/api/auth/login`,
    data
  );

  return res.data;
};

export const registerUserApi = async (
  data: RegisterRequest
): Promise<AuthResponse> => {

  const res = await axios.post(
    `${API_URL}/api/auth/register`,
    data
  );

  return res.data;
};

export const getProfileApi = async () => {
  const res = await axios.get(`${API_URL}/api/auth/me`);
  return res.data;
};