// src/features/auth/useAuthInitializer.ts
import { useEffect } from "react";
import { useAppDispatch } from "../../store";
import { getProfileApi } from "../../api/userApi";
import { setUserFromToken, logoutUser } from "./authSlice";

export const useAuthInitializer = () => {
  const dispatch = useAppDispatch();

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (!token) return; // no token, nothing to do

    // Fetch user profile from API
    getProfileApi()
      .then((data) => {
        dispatch(setUserFromToken({ fullName: data.fullName, email: data.email }));
      })
      .catch(() => {
        // Token invalid or API failed → logout
        dispatch(logoutUser());
      });
  }, [dispatch]);
};