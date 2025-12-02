# Technology Stack

## Frontend Technologies

### Core Framework
- **React 19.1.1**
  - Latest version with improved performance and concurrent features
  - Component-based architecture for reusable UI elements
  - Hooks for state management and side effects
  - Virtual DOM for efficient rendering

### Build Tool
- **Vite 7.1.2**
  - Lightning-fast development server with Hot Module Replacement (HMR)
  - Optimized production builds with code splitting
  - Native ES modules support
  - Plugin-based architecture

### Styling
- **Tailwind CSS 4.1.12**
  - Utility-first CSS framework
  - Highly customizable design system
  - Built-in responsive design utilities
  - Dark mode support
  - JIT (Just-In-Time) compilation for optimal performance
  - Vite plugin integration (`@tailwindcss/vite`)

### State Management
- **Redux Toolkit 2.8.2**
  - Official, opinionated Redux toolset
  - Simplified store configuration
  - Built-in Immer for immutable updates
  - Redux DevTools integration
  - Reduced boilerplate code

- **React Redux 9.2.0**
  - Official React bindings for Redux
  - `useSelector` and `useDispatch` hooks
  - Performance optimizations with shallow equality checks

### Routing
- **React Router DOM 7.8.1**
  - Declarative routing for React applications
  - Nested routes support
  - URL parameter handling
  - Programmatic navigation
  - Layout route support

### UI Components & Icons
- **Lucide React 0.540.0**
  - Beautiful, consistent icon library
  - Tree-shakeable for optimal bundle size
  - 1000+ icons
  - Customizable size, color, and stroke width

### Data Visualization
- **Recharts 3.1.2**
  - Composable charting library built on React components
  - Responsive charts
  - Used for project analytics and progress visualization
  - Line charts, bar charts, pie charts, area charts

### Utilities
- **date-fns 4.1.0**
  - Modern JavaScript date utility library
  - Immutable and pure functions
  - Tree-shakeable
  - Used for date formatting and manipulation in tasks and projects

- **React Hot Toast 2.6.0**
  - Lightweight toast notification library
  - Customizable notifications
  - Promise-based API
  - Accessible and keyboard navigable

## Development Tools

### Linting & Code Quality
- **ESLint 9.33.0**
  - JavaScript/React linting
  - Custom configuration with `@eslint/js`
  - React-specific rules with `eslint-plugin-react-hooks` and `eslint-plugin-react-refresh`

### Type Checking (Development)
- **@types/react 19.1.10**
- **@types/react-dom 19.1.7**
  - TypeScript type definitions for React
  - Improves IDE autocomplete and type safety

### Build Plugins
- **@vitejs/plugin-react 5.0.0**
  - Official Vite plugin for React
  - Fast Refresh support
  - JSX transformation

## Package Manager
- **npm**
  - Recommended package manager for this project
  - Lock file: `package-lock.json`

## Browser Compatibility
- Modern browsers (Chrome, Firefox, Safari, Edge)
- ES2020+ features
- No legacy browser support required

## Development Environment
- **Node.js**: v18+ recommended
- **Development Server**: Vite dev server on `http://localhost:5173`
- **Hot Module Replacement**: Instant updates without full page reload

## Production Build
- **Build Command**: `npm run build`
- **Output**: Optimized static files in `dist/` directory
- **Code Splitting**: Automatic chunk splitting for optimal loading
- **Minification**: JavaScript and CSS minification
- **Tree Shaking**: Unused code elimination

## Key Technology Choices & Rationale

### Why React 19?
- Industry-standard for building interactive UIs
- Large ecosystem and community support
- Excellent performance with concurrent features
- Strong developer tooling

### Why Vite?
- Significantly faster than webpack-based tools
- Native ES modules for instant server start
- Optimized build output
- Excellent developer experience

### Why Redux Toolkit?
- Centralized state management for complex application state
- Predictable state updates
- Excellent debugging with DevTools
- Scales well as application grows

### Why Tailwind CSS?
- Rapid UI development with utility classes
- Consistent design system
- Smaller CSS bundle size (unused styles purged)
- Easy to maintain and customize

### Why React Router?
- De facto standard for React routing
- Declarative routing matches React's philosophy
- Excellent documentation and community support

## Future Technology Considerations

### When Backend is Integrated
- **Axios** or **Fetch API**: HTTP client for API calls
- **RTK Query**: Data fetching and caching (part of Redux Toolkit)
- **React Query**: Alternative to RTK Query for server state management

### Authentication
- **JWT**: Token-based authentication
- **OAuth 2.0**: Third-party authentication (Google, GitHub, etc.)

### Real-time Features
- **Socket.io**: WebSocket library for real-time updates
- **SignalR**: For ASP.NET Core backend integration

### Testing (Recommended)
- **Vitest**: Fast unit testing framework (Vite-native)
- **React Testing Library**: Component testing
- **Playwright**: End-to-end testing
- **MSW (Mock Service Worker)**: API mocking

### Performance Monitoring
- **Sentry**: Error tracking and performance monitoring
- **Web Vitals**: Core Web Vitals measurement

### Deployment
- **Vercel**: Optimized for Vite/React applications
- **Netlify**: Alternative deployment platform
- **Docker**: Containerization for consistent deployments

## Version Management
All dependencies are pinned to specific versions to ensure consistency across environments. Regular updates should be performed with careful testing.

## License
All technologies used are open-source with permissive licenses (MIT, Apache 2.0, etc.)
