import React, { useState } from "react";
import {
  Box,
  Button,
  Checkbox,
  Container,
  FormControlLabel,
  Paper,
  TextField,
  Typography,
  Alert,
} from "@mui/material";

interface LoginFormProps {
  onSubmit: (data: {
    email: string;
    password: string;
    rememberMe: boolean;
  }) => void;
  loading?: boolean;
  error?: string;
}

const LoginForm: React.FC<LoginFormProps> = ({
  onSubmit,
  loading,
  error,
}) => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [rememberMe, setRememberMe] = useState(false);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit({ email, password, rememberMe });
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
          {/* Title */}
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
            Member Login
          </Typography>

          {/* Subtitle */}
          <Typography
            align="center"
            sx={{
              color: "#7A6A86",
              fontSize: "0.9rem",
              mb: 4,
              letterSpacing: "0.4px",
            }}
          >
            Secure Financial Platform
          </Typography>

          {/* Error */}
          {error && (
            <Alert severity="error" sx={{ mb: 2 }}>
              {error}
            </Alert>
          )}

          {/* Form */}
          <Box component="form" onSubmit={handleSubmit}>
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

            <FormControlLabel
              control={
                <Checkbox
                  checked={rememberMe}
                  onChange={(e) =>
                    setRememberMe(e.target.checked)
                  }
                  sx={{
                    color: "#B89AC6",
                    "&.Mui-checked": {
                      color: "#B89AC6",
                    },
                  }}
                />
              }
              label="Remember this device"
              sx={{
                mt: 1,
                mb: 3,
                color: "#5E4A68",
              }}
            />

            <Button
              type="submit"
              fullWidth
              disabled={loading}
              variant="contained"
              sx={{
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
              {loading ? "Signing in..." : "Sign In"}
            </Button>
          </Box>
        </Paper>
      </Container>
    </Box>
  );
};

export default LoginForm;