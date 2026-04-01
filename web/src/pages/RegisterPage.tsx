import React, { useState } from "react";
import {
  Box,
  Button,
  Container,
  Paper,
  TextField,
  Typography,
  Alert,
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import { useAppDispatch } from "../store";
import { registerUser } from "../features/auth/authSlice";

const RegisterPage: React.FC = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const [fullName, setFullName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    setLoading(true);
    setError(null);

    const result = await dispatch(
      registerUser({
        fullName,
        email,
        password,
      })
    );

    setLoading(false);

    if (registerUser.fulfilled.match(result)) {
      navigate("/profile");
    } else {
      setError(
        (result.payload as string) ||
          "Registration failed. Please try again."
      );
    }
  };

  return (
    <Box
      sx={{
        minHeight: "100vh",
        background:
          "linear-gradient(135deg, #F7F4F9, #EDE7F2, #FDFCFB)",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
      }}
    >
      <Container maxWidth="sm">
        <Paper
          elevation={6}
          sx={{
            p: 5,
            borderRadius: 3,
            border: "1px solid rgba(190,160,90,0.3)",
            boxShadow: "0 18px 45px rgba(90,70,110,0.18)",
          }}
        >
          <Typography
            variant="h4"
            align="center"
            sx={{
              fontFamily: "Playfair Display, serif",
              color: "#4B3553",
              fontWeight: 600,
              mb: 1,
            }}
          >
            Create Account
          </Typography>

          <Typography
            align="center"
            sx={{
              color: "#7A6A86",
              fontSize: "0.9rem",
              mb: 4,
              letterSpacing: "0.4px",
            }}
          >
            Join Our Private Financial Platform
          </Typography>

          {error && (
            <Alert severity="error" sx={{ mb: 2 }}>
              {error}
            </Alert>
          )}

          <Box component="form" onSubmit={handleSubmit}>
            <TextField
              fullWidth
              label="Full Name"
              value={fullName}
              onChange={(e) => setFullName(e.target.value)}
              margin="normal"
              required
              sx={{
                "& .MuiOutlinedInput-root": {
                  background: "#FBFAFC",
                },
              }}
            />

            <TextField
              fullWidth
              label="Email"
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              margin="normal"
              required
              sx={{
                "& .MuiOutlinedInput-root": {
                  background: "#FBFAFC",
                },
              }}
            />

            <TextField
              fullWidth
              label="Password"
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              margin="normal"
              required
              sx={{
                "& .MuiOutlinedInput-root": {
                  background: "#FBFAFC",
                },
              }}
            />

            <Button
              type="submit"
              fullWidth
              disabled={loading}
              variant="contained"
              sx={{
                mt: 3,
                py: 1.4,
                borderRadius: 2,
                fontWeight: 600,
                fontSize: "0.95rem",
                background:
                  "linear-gradient(135deg, #E4D2A8, #C8B08A)",
                color: "#3E2F1A",
                boxShadow:
                  "0 6px 14px rgba(180,150,80,0.35)",

                "&:hover": {
                  background:
                    "linear-gradient(135deg, #D9C28F, #B89E74)",
                  boxShadow:
                    "0 8px 18px rgba(180,150,80,0.5)",
                  transform: "translateY(-1px)",
                },
              }}
            >
              {loading ? "Creating Account..." : "Register"}
            </Button>
          </Box>
        </Paper>
      </Container>
    </Box>
  );
};

export default RegisterPage;