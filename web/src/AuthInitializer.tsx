// src/AuthInitializer.tsx
import { useEffect } from 'react';
import { useAppDispatch } from './store';
import { getProfileApi } from './api/userApi';
import { setUserFromToken, logoutUser } from './features/auth/authSlice';

const AuthInitializer: React.FC = () => {
  const dispatch = useAppDispatch();

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (!token) return;

    // Fetch user profile from backend
    getProfileApi()
      .then((data) => {
        dispatch(setUserFromToken({ fullName: data.fullName, email: data.email }));
      })
      .catch(() => {
        // Invalid token → logout
        dispatch(logoutUser());
      });
  }, [dispatch]);

  return null; // does not render anything
};

export default AuthInitializer;