export const getToken = () => {
  return localStorage.getItem('token');
};

export const isAuthenticated = (): boolean => {
  return !!getToken();
};

export const logout = () => {
  localStorage.removeItem('token');
};