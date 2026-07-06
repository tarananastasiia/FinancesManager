import { apiClient } from "./apiClient";

export const createPaymentIntent = async () => {
  const res = await apiClient.post("/api/payments/create-intent");
  return res.data.clientSecret;
};

export const getHistoryApi = async () => {
  const res = await apiClient.get("/api/payments/history");
  return res.data;
};

export const getCardInfoApi = async () => {
  const res = await apiClient.get("/api/payments/cards");
  return res.data;
};