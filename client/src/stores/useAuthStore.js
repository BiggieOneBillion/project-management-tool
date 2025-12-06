import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';
import { authService } from '../services';
import toast from 'react-hot-toast';

export const useAuthStore = create(
  persist(
    (set, get) => ({
      // State
      user: null,
      accessToken: null,
      refreshToken: null,
      isAuthenticated: false,
      loading: false,
      error: null,

      // Actions
      clearError: () => set({ error: null }),

      // Login
      login: async (email, password) => {
        set({ loading: true, error: null });
        try {
          const response = await authService.login(email, password);
          const { accessToken, refreshToken, user } = response.data;

          set({
            user,
            accessToken,
            refreshToken,
            isAuthenticated: true,
            loading: false,
            error: null,
          });

          return response.data;
        } catch (error) {
          const errorMessage = error.response?.data?.message || error.message || 'Login failed';
          set({
            loading: false,
            error: errorMessage,
            isAuthenticated: false,
          });
          throw new Error(errorMessage);
        }
      },

      // Register
      register: async (name, email, password, invitationToken = null) => {
        set({ loading: true, error: null });
        try {
          const response = await authService.register(name, email, password, invitationToken);
          const { accessToken, refreshToken, user } = response.data;

          set({
            user,
            accessToken,
            refreshToken,
            isAuthenticated: true,
            loading: false,
            error: null,
          });

          return response.data;
        } catch (error) {
          const errorMessage = error.response?.data?.message || error.message || 'Registration failed';
          set({
            loading: false,
            error: errorMessage,
            isAuthenticated: false,
          });
          throw new Error(errorMessage);
        }
      },

      // Logout
      logout: async () => {
        set({ loading: true });
        try {
          await authService.logout();
        } catch (error) {
          console.error('Logout error:', error);
        } finally {
          // Clear auth state
          set({
            user: null,
            accessToken: null,
            refreshToken: null,
            isAuthenticated: false,
            loading: false,
            error: null,
          });

          // Clear all Zustand stores from storage
          localStorage.removeItem('auth-storage');
          sessionStorage.removeItem('workspace-storage');
          sessionStorage.removeItem('project-storage');
          sessionStorage.removeItem('task-storage');
          sessionStorage.removeItem('theme-storage');
          sessionStorage.removeItem('comment-storage');
        }
      },

      // Refresh access token
      refreshAccessToken: async () => {
        const { refreshToken } = get();
        if (!refreshToken) {
          throw new Error('No refresh token available');
        }

        try {
          const response = await authService.refreshToken(refreshToken);
          const { accessToken: newAccessToken, refreshToken: newRefreshToken, user } = response.data;

          set({
            accessToken: newAccessToken,
            refreshToken: newRefreshToken,
            user,
            isAuthenticated: true,
          });

          return newAccessToken;
        } catch (error) {
          // If refresh fails, logout user
          get().logout();
          throw error;
        }
      },

      // Get current user
      getCurrentUser: async () => {
        set({ loading: true, error: null });
        try {
          const response = await authService.getCurrentUser();
          set({
            user: response.data,
            loading: false,
          });
          return response.data;
        } catch (error) {
          set({
            loading: false,
            error: error.response?.data?.message || error.message,
          });
          throw error;
        }
      },

      // Check if user is authenticated (useful for initial load)
      checkAuth: () => {
        const { accessToken, user } = get();
        return !!(accessToken && user);
      },
    }),
    {
      name: 'auth-storage',
      storage: createJSONStorage(() => localStorage), // Use localStorage for auth (persists across sessions)
      partialize: (state) => ({
        // Only persist these fields
        user: state.user,
        accessToken: state.accessToken,
        refreshToken: state.refreshToken,
        isAuthenticated: state.isAuthenticated,
      }),
    }
  )
);
