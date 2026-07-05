import React, { useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../store';
import { loginUser } from '../features/auth/authSlice';
import { useNavigate } from 'react-router-dom';
import LoginForm from '../features/auth/components/LoginForm';

const LoginPage: React.FC = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const { status, error } = useAppSelector((state) => state.auth);

  const handleSubmit = async (data: {
    email: string;
    password: string;
    rememberMe: boolean;
  }) => {
    const result = await dispatch(loginUser(data));

    if (loginUser.fulfilled.match(result)) {
      navigate("/profile");
    }
  };

  return (
    <LoginForm
      onSubmit={handleSubmit}
      loading={status === "loading"}
      error={error}
    />
  );
};

export default LoginPage;