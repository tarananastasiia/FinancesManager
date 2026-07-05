import { useEffect } from "react";
import { useAppDispatch } from "./store";
import { getProfileApi } from "./api/userApi";
import { setToken, setUser, logout } from "./features/auth/authSlice";

const AuthInitializer: React.FC = () => {
  const dispatch = useAppDispatch();

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (!token) return;

    dispatch(setToken(token));

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

  return null;
};

export default AuthInitializer;