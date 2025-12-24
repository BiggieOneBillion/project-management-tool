import { useState, useEffect } from 'react';

/**
 * Custom hook for debounced search functionality
 * @param {string} searchTerm - The search term to debounce
 * @param {number} delay - Debounce delay in milliseconds (default: 300ms)
 * @returns {string} - The debounced search term
 */
export function useDebounce(searchTerm, delay = 300) {
  const [debouncedValue, setDebouncedValue] = useState(searchTerm);

  useEffect(() => {
    const handler = setTimeout(() => {
      setDebouncedValue(searchTerm);
    }, delay);

    return () => {
      clearTimeout(handler);
    };
  }, [searchTerm, delay]);

  return debouncedValue;
}

/**
 * Custom hook for searching across projects and tasks
 * @param {Array} projects - Array of projects to search
 * @param {Array} tasks - Array of tasks to search
 * @param {string} searchTerm - The search term
 * @returns {Object} - Object containing filtered projects and tasks
 */
export function useSearch(projects = [], tasks = [], searchTerm = '') {
  const debouncedSearch = useDebounce(searchTerm);

  const filteredProjects = projects.filter(project => {
    if (!debouncedSearch) return false;
    const search = debouncedSearch.toLowerCase();
    return (
      project.name?.toLowerCase().includes(search) ||
      project.description?.toLowerCase().includes(search)
    );
  });

  const filteredTasks = tasks.filter(task => {
    if (!debouncedSearch) return false;
    const search = debouncedSearch.toLowerCase();
    return (
      task.title?.toLowerCase().includes(search) ||
      task.description?.toLowerCase().includes(search)
    );
  });

  return {
    projects: filteredProjects,
    tasks: filteredTasks,
    hasResults: filteredProjects.length > 0 || filteredTasks.length > 0,
    isSearching: debouncedSearch.length > 0
  };
}
