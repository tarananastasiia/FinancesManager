import { createSlice, createAsyncThunk, PayloadAction } from "@reduxjs/toolkit";
import {
  LoginRequest,
  loginUserApi,
  registerUserApi,
  RegisterRequest,
} from "../../api/userApi";

interface User {
  fullName: string;
  email: string;
}

interface AuthState {
  token: string | null;
  user: User | null;
  status: "idle" | "loading" | "failed";
  error: string | null;
}

const initialState: AuthState = {
  token: localStorage.getItem("token"),
  user: null,
  status: "idle",
  error: null,
};

export const loginUser = createAsyncThunk(
  "auth/login",
  async (data: LoginRequest, { rejectWithValue }) => {
    try {
      return await loginUserApi(data);
    } catch (err: any) {
      return rejectWithValue(err.response?.data?.message || "Login failed");
    }
  }
);

export const registerUser = createAsyncThunk(
  "auth/register",
  async (data: RegisterRequest, { rejectWithValue }) => {
    try {
      return await registerUserApi(data);
    } catch (err: any) {
      return rejectWithValue(err.response?.data?.[0]?.description || "Register failed");
    }
  }
);

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    logout: (state) => {
      state.token = null;
      state.user = null;
      state.error = null;
      localStorage.removeItem("token");
    },
    setToken: (state, action: PayloadAction<string>) => {
      state.token = action.payload;
    },
    setUser: (state, action: PayloadAction<User>) => {
      state.user = action.payload;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(loginUser.pending, (state) => {
        state.status = "loading";
        state.error = null;
      })
      .addCase(loginUser.fulfilled, (state, action: any) => {
        state.status = "idle";
        state.token = action.payload.token;
        state.user = {
          email: action.payload.email,
          fullName: action.payload.fullName,
        };
        localStorage.setItem("token", action.payload.token);
      })
      .addCase(loginUser.rejected, (state, action: any) => {
        state.status = "failed";
        state.error = (action.payload as string) || "Login failed";
      })
      .addCase(registerUser.rejected, (state, action: any) => {
        state.status = "failed";
        state.error = (action.payload as string) || "Register failed";
      });
  },
});

export const hydrateAuth = () => (dispatch: any) => {
  const token = localStorage.getItem("token");
  if (token) dispatch(setToken(token));
};

export const { logout, setToken, setUser } = authSlice.actions;
export default authSlice.reducer;

export const selectToken = (state: any) => state.auth.token;

export const selectIsAuthenticated = (state: any) =>
  Boolean(state.auth.token);

export const selectUser = (state: any) => state.auth.user;

export const selectAuthStatus = (state: any) => state.auth.status;

export const selectAuthError = (state: any) => state.auth.error;