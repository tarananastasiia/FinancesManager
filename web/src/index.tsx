import React from 'react';
import ReactDOM from 'react-dom/client';
import AppRoutes from './routes';
import { store } from './store';
import { Provider } from 'react-redux';
import { BrowserRouter as Router } from 'react-router-dom';
import Header from './header';
import AuthInitializer from './AuthInitializer';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <Provider store={store}>
      <Router>
         <AuthInitializer />
         <Header />
        <AppRoutes />
      </Router>
    </Provider>
  </React.StrictMode>
);