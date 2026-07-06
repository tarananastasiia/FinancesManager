import axios from "axios";

const API_URL = process.env.REACT_APP_API_URL;

export const refreshTokenApi = async () => {
  const refreshToken = localStorage.getItem("refreshToken");

  const res = await axios.post(`${API_URL}/api/auth/refresh`, {
    refreshToken,
  });

  return res.data;
};