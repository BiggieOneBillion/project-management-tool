// API Configuration
import axios from 'axios';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api/v1',
  headers: {
    'Content-Type': 'application/json',
  },
  timeout: 10000,
});

// Request interceptor - Add JWT token
api.interceptors.request.use(
  (config) => {
    // Get token from localStorage (where auth store persists it)
    const authStorage = sessionStorage.getItem('auth-storage');
    if (authStorage) {
      try {
        const { state } = JSON.parse(authStorage);
        if (state?.accessToken) {
          config.headers.Authorization = `Bearer ${state.accessToken}`;
        }
      } catch (error) {
        console.error('Error parsing auth storage:', error);
      }
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Response interceptor - Handle 401 and refresh token
let isRefreshing = false;
let failedQueue = [];

const processQueue = (error, token = null) => {
  failedQueue.forEach(prom => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });
  failedQueue = [];
};

api.interceptors.response.use(
  (response) => response.data,
  async (error) => {
    const originalRequest = error.config;

    if (error.response) {
      const { status, data } = error.response;

      // Handle 401 Unauthorized
      if (status === 401 && !originalRequest._retry) {
        // Don't retry if this IS the refresh token request itself
        if (originalRequest.url?.includes('/auth/refresh')) {
          isRefreshing = false;
          // Refresh failed, logout user
          const { useAuthStore } = await import('../stores/useAuthStore');
          useAuthStore.getState().logout();
          window.location.href = '/login';
          return Promise.reject(error);
        }

        // If already refreshing, queue this request
        if (isRefreshing) {
          return new Promise((resolve, reject) => {
            failedQueue.push({ resolve, reject });
          })
            .then(token => {
              originalRequest.headers.Authorization = `Bearer ${token}`;
              return api(originalRequest);
            })
            .catch(err => Promise.reject(err));
        }

        originalRequest._retry = true;
        isRefreshing = true;

        try {
          // Try to refresh the token
          const authStorage = localStorage.getItem('auth-storage');
          if (authStorage) {
            const { state } = JSON.parse(authStorage);
            if (state?.refreshToken) {
              // Import dynamically to avoid circular dependency
              const { useAuthStore } = await import('../stores/useAuthStore');
              const newAccessToken = await useAuthStore.getState().refreshAccessToken();

              isRefreshing = false;
              processQueue(null, newAccessToken);

              // Retry the original request with new token
              originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;
              return api(originalRequest);
            }
          }
          
          isRefreshing = false;
          processQueue(new Error('No refresh token'), null);
          throw new Error('No refresh token available');
        } catch (refreshError) {
          isRefreshing = false;
          processQueue(refreshError, null);
          
          // Refresh failed, logout user
          const { useAuthStore } = await import('../stores/useAuthStore');
          useAuthStore.getState().logout();
          window.location.href = '/login';
          return Promise.reject(refreshError);
        }
      }

      // For other errors, return the error data
      return Promise.reject(data);
    } else if (error.request) {
      return Promise.reject({
        success: false,
        message: 'Network error. Please check your connection.',
      });
    } else {
      return Promise.reject({
        success: false,
        message: error.message,
      });
    }
  }
);

export default api;
