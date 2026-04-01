import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { LoginRequest, loginUserApi, registerUserApi, RegisterRequest } from '../../api/userApi';

interface User {
  fullName: string;
  email: string;
}

interface AuthState {
  user: User | null;
  token: string | null;
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error?: string;
}

const initialState: AuthState = {
  user: null,
  token: localStorage.getItem('token'),
  status: 'idle',
};

// Async thunks
export const loginUser = createAsyncThunk(
  'auth/login',
  async (data: LoginRequest, { rejectWithValue }) => {
    try {
      return await loginUserApi(data); // { token, email, fullName }
    } catch (err: any) {
      return rejectWithValue(err.response?.data?.message || 'Login failed');
    }
  }
);

export const registerUser = createAsyncThunk(
  'auth/register',
  async (data: RegisterRequest, { rejectWithValue }) => {
    try {
      return await registerUserApi(data);
    } catch (err: any) {
      return rejectWithValue(err.response?.data?.[0]?.description || 'Register failed');
    }
  }
);

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    logoutUser: (state) => {
      state.user = null;
      state.token = null;
      state.status = 'idle';
      state.error = undefined;
      localStorage.removeItem('token');
    },
    setUserFromToken: (state, action: { payload: User }) => {
      state.user = action.payload;
    }
  },
  extraReducers: (builder) => {
    builder
      .addCase(loginUser.pending, (state) => {
        state.status = 'loading';
        state.error = undefined;
      })
      .addCase(loginUser.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.payload as string;
      })
      .addCase(loginUser.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.user = { fullName: action.payload.fullName, email: action.payload.email };
        state.token = action.payload.token;
        localStorage.setItem('token', action.payload.token);
      })
      .addCase(registerUser.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.user = { fullName: action.payload.fullName, email: action.payload.email };
        state.token = action.payload.token;
        localStorage.setItem('token', action.payload.token);
      });
  },
});

export const { logoutUser, setUserFromToken } = authSlice.actions;
export default authSlice.reducer;