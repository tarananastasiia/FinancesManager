import React from "react";
import { AppBar, Toolbar, Typography, Button, Box } from "@mui/material";
import { useSelector, useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import { RootState } from "./store";
import { logoutUser } from "./features/auth/authSlice";

const Header: React.FC = () => {
  const { user } = useSelector((state: RootState) => state.auth);
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem("token");
    dispatch(logoutUser());
    navigate("/login");
  };

console.log(user);
  return (
    <AppBar
      position="static"
      sx={{
        background: "#fff",
        boxShadow: "0 4px 12px rgba(0,0,0,0.08)",
        borderBottom: "1px solid #E0E0E0",
      }}
    >
      <Toolbar
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          fontFamily: "'Poppins', sans-serif",
          py: 1.5,
          px: { xs: 2, md: 6 },
        }}
      >
        <Typography
          variant="h5"
          sx={{
            color: "#4B3F72",
            fontWeight: 700,
            letterSpacing: "0.5px",
            cursor: "pointer",
            "&:hover": { color: "#7E57C2" },
            transition: "color 0.2s ease",
          }}
          onClick={() => navigate("/")}
        >
          Finances Manager
        </Typography>

        <Box sx={{ display: "flex", alignItems: "center" }}>
          {user ? (
            <>
              <Typography
                component="span"
                sx={{
                  mr: 4,
                  color: "#6B5AA3",
                  fontWeight: 600,
                  fontSize: "0.95rem",
                  letterSpacing: "0.5px",
                }}
              >
                Hello, {user.fullName} 👋
              </Typography>

              <Button
                onClick={handleLogout}
                sx={{
                  color: "#fff",
                  background: "linear-gradient(90deg, #FF8A65, #FFB74D)",
                  "&:hover": {
                    background: "linear-gradient(90deg, #FF9E80, #FFC97A)",
                    transform: "translateY(-2px)",
                    boxShadow: "0 6px 18px rgba(255,140,90,0.35)",
                  },
                  borderRadius: 2,
                  fontWeight: 600,
                  textTransform: "none",
                  px: 3.5,
                  py: 1,
                  transition: "all 0.25s ease",
                  boxShadow: "0 3px 8px rgba(0,0,0,0.05)",
                }}
              >
                Logout
              </Button>
            </>
          ) : (
            <>
              <Button
                onClick={() => navigate("/login")}
                sx={{
                  color: "#fff",
                  mr: 2,
                  background: "linear-gradient(90deg, #7E57C2, #9C6BFF)",
                  "&:hover": {
                    background: "linear-gradient(90deg, #9061D1, #AB7FFF)",
                    transform: "translateY(-2px)",
                    boxShadow: "0 6px 18px rgba(125,80,255,0.3)",
                  },
                  borderRadius: 2,
                  fontWeight: 600,
                  textTransform: "none",
                  px: 3.5,
                  py: 1,
                  transition: "all 0.25s ease",
                  boxShadow: "0 3px 8px rgba(0,0,0,0.04)",
                }}
              >
                Login
              </Button>

              <Button
                onClick={() => navigate("/register")}
                sx={{
                  color: "#fff",
                  background: "linear-gradient(90deg, #FF8A65, #FFB74D)",
                  "&:hover": {
                    background: "linear-gradient(90deg, #FF9E80, #FFC97A)",
                    transform: "translateY(-2px)",
                    boxShadow: "0 6px 18px rgba(255,140,90,0.35)",
                  },
                  borderRadius: 2,
                  fontWeight: 600,
                  textTransform: "none",
                  px: 3.5,
                  py: 1,
                  transition: "all 0.25s ease",
                  boxShadow: "0 3px 8px rgba(0,0,0,0.04)",
                }}
              >
                Register
              </Button>
            </>
          )}
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default Header;