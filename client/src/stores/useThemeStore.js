import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';

export const useThemeStore = create(
  persist(
    (set) => ({
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
}),
    {
      name: 'theme-storage',
      storage: createJSONStorage(() => sessionStorage),
    }
  )
);
