import React from "react";
import { Navigate } from "react-router-dom";
import { selectIsAuthenticated } from "../authSlice";
import { useAppSelector } from "../../../store";

interface Props {
  children: JSX.Element;
}

const ProtectedRoute: React.FC<Props> = ({ children }) => {
  const isAuth = useAppSelector(selectIsAuthenticated);

  if (!isAuth) {
    return <Navigate to="/login" replace />;
  }

  return children;
};

export default ProtectedRoute;