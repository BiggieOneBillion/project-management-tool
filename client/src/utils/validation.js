/**
 * Validation utilities for form inputs
 */

// Email validation
export const validateEmail = (email) => {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (!email) return 'Email is required';
  if (!emailRegex.test(email)) return 'Please enter a valid email address';
  return null;
};

// Password validation
export const validatePassword = (password) => {
  if (!password) return 'Password is required';
  if (password.length < 8) return 'Password must be at least 8 characters long';
  if (!/[A-Z]/.test(password)) return 'Password must contain at least one uppercase letter';
  if (!/[0-9]/.test(password)) return 'Password must contain at least one number';
  return null;
};

// Password confirmation validation
export const validatePasswordConfirmation = (password, confirmation) => {
  if (!confirmation) return 'Please confirm your password';
  if (password !== confirmation) return 'Passwords do not match';
  return null;
};

// Name validation
export const validateName = (name) => {
  if (!name) return 'Name is required';
  if (name.trim().length < 2) return 'Name must be at least 2 characters long';
  if (name.trim().length > 50) return 'Name must be less than 50 characters';
  return null;
};

// Workspace/Project name validation
export const validateWorkspaceName = (name) => {
  if (!name) return 'Workspace name is required';
  if (name.trim().length < 3) return 'Workspace name must be at least 3 characters long';
  if (name.trim().length > 100) return 'Workspace name must be less than 100 characters';
  return null;
};

export const validateProjectName = (name) => {
  if (!name) return 'Project name is required';
  if (name.trim().length < 3) return 'Project name must be at least 3 characters long';
  if (name.trim().length > 100) return 'Project name must be less than 100 characters';
  return null;
};

// Task title validation
export const validateTaskTitle = (title) => {
  if (!title) return 'Task title is required';
  if (title.trim().length < 3) return 'Task title must be at least 3 characters long';
  if (title.trim().length > 200) return 'Task title must be less than 200 characters';
  return null;
};

// Description validation
export const validateDescription = (description, maxLength = 1000) => {
  if (description && description.length > maxLength) {
    return `Description must be less than ${maxLength} characters`;
  }
  return null;
};

// Date validation
export const validateDate = (date) => {
  if (!date) return null; // Date is optional
  const selectedDate = new Date(date);
  const today = new Date();
  today.setHours(0, 0, 0, 0);
  
  if (isNaN(selectedDate.getTime())) return 'Please enter a valid date';
  if (selectedDate < today) return 'Date cannot be in the past';
  return null;
};

// Date range validation
export const validateDateRange = (startDate, endDate) => {
  if (!startDate || !endDate) return null;
  
  const start = new Date(startDate);
  const end = new Date(endDate);
  
  if (start > end) return 'End date must be after start date';
  return null;
};

// Required field validation
export const validateRequired = (value, fieldName = 'This field') => {
  if (!value || (typeof value === 'string' && !value.trim())) {
    return `${fieldName} is required`;
  }
  return null;
};

// Generic validation runner
export const runValidations = (validators) => {
  const errors = {};
  let hasErrors = false;

  Object.keys(validators).forEach(field => {
    const error = validators[field]();
    if (error) {
      errors[field] = error;
      hasErrors = true;
    }
  });

  return { errors, hasErrors };
};
