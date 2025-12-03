import { create } from 'zustand';

export const useThemeStore = create((set) => ({
  // State
  theme: 'light',

  // Actions
  toggleTheme: () => {
    set((state) => {
      const newTheme = state.theme === 'light' ? 'dark' : 'light';
      localStorage.setItem('theme', newTheme);
      document.documentElement.classList.toggle('dark');
      return { theme: newTheme };
    });
  },

  setTheme: (theme) => {
    set({ theme });
  },

  loadTheme: () => {
    const theme = localStorage.getItem('theme');
    if (theme) {
      set({ theme });
      if (theme === 'dark') {
        document.documentElement.classList.add('dark');
      }
    }
  },
}));
