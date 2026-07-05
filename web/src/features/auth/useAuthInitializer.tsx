import { useEffect } from "react";
import { useAppDispatch } from "../../store";
import { getProfileApi } from "../../api/userApi";
import { setUser, logout } from "./authSlice";

export const useAuthInitializer = () => {
  const dispatch = useAppDispatch();

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (!token) return;

    getProfileApi()
      .then((data) => {
        dispatch(
          setUser({
            fullName: data.fullName,
            email: data.email,
          })
        );
      })
      .catch(() => {
        dispatch(logout());
      });
  }, [dispatch]);
};