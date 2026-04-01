import axios from "axios";

const API = process.env.REACT_APP_API_URL;

axios.defaults.headers.common["Authorization"] =
  `Bearer ${localStorage.getItem("token")}`;

export const createPaymentIntent = async () => {
  const res = await axios.post(
    `${API}/api/payments/create-intent`
  );

  return res.data.clientSecret;
};

export const getHistory = async () => {
  const res = await axios.get(
    `${API}/api/payments/history`
  );

  return res.data;
};

export const getHistoryApi = async () => {
  const res = await axios.get(`${API}/api/payments/history`);
  return res.data;
};

export const getCardInfoApi = async () => {
  const res = await axios.get(`${API}/api/payments/cards`);
  return res.data;
};