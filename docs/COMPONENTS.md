# Components Documentation

This document provides a comprehensive overview of all React components in the project management application.

## Component Hierarchy

```
App
└── Routes
    └── Layout
        ├── Navbar
        │   └── WorkspaceDropdown
        ├── Sidebar
        └── Outlet (Page Components)
            ├── Dashboard
            │   ├── StatsGrid
            │   ├── RecentActivity
            │   ├── TasksSummary
            │   └── MyTasksSidebar
            ├── Projects
            │   ├── ProjectCard (multiple)
            │   ├── CreateProjectDialog
            │   └── ProjectsSidebar
            ├── ProjectDetails
            │   ├── ProjectOverview
            │   ├── ProjectTasks
            │   │   ├── CreateTaskDialog
            │   │   └── Task rows
            │   ├── ProjectAnalytics
            │   ├── ProjectCalendar
            │   ├── ProjectSettings
            │   └── AddProjectMember
            ├── TaskDetails
            │   └── Task information display
            └── Team
                └── InviteMemberDialog
```

---

## Layout Components

### Layout.jsx
**Purpose**: Main application shell that wraps all pages

**Features**:
- Renders `Navbar` and `Sidebar`
- Uses `<Outlet />` from React Router for nested routes
- Provides consistent layout across all pages

**Props**: None (uses React Router context)

**State**: None

---

### Navbar.jsx
**Purpose**: Top navigation bar with workspace selector and user actions

**Features**:
- Displays current workspace
- Contains `WorkspaceDropdown` for switching workspaces
- User profile section
- Notifications (UI only)

**Props**: None (connects to Redux)

**Redux State**:
- `currentWorkspace` from workspace slice

**Key Components Used**:
- `WorkspaceDropdown`
- Lucide icons (Bell, Search, User)

---

### Sidebar.jsx
**Purpose**: Left navigation sidebar with main menu items

**Features**:
- Navigation links (Dashboard, Projects, Team)
- Active route highlighting
- Responsive collapse/expand

**Props**: None

**Navigation Items**:
- Dashboard (`/`)
- Projects (`/projects`)
- Team (`/team`)

**Icons Used**: Home, FolderKanban, Users (from Lucide)

---

### WorkspaceDropdown.jsx
**Purpose**: Dropdown to switch between workspaces

**Features**:
- Lists all available workspaces
- Highlights current workspace
- Switches workspace on selection
- Create new workspace option

**Props**: None (connects to Redux)

**Redux State**:
- `workspaces` - all workspaces
- `currentWorkspace` - active workspace

**Redux Actions**:
- `setCurrentWorkspace(workspaceId)` - switches workspace

---

## Page Components

### Dashboard.jsx
**Purpose**: Main dashboard overview page

**Features**:
- Displays workspace statistics
- Shows recent activity
- Task summary
- My tasks sidebar

**Components Used**:
- `StatsGrid` - Key metrics
- `RecentActivity` - Recent workspace activity
- `TasksSummary` - Task status breakdown
- `MyTasksSidebar` - User's assigned tasks

**Redux State**:
- `currentWorkspace` - for workspace-specific data

---

### Projects.jsx
**Purpose**: Lists all projects in the current workspace

**Features**:
- Grid of project cards
- Filter by status (All, Active, Planning)
- Create new project
- Search projects

**Components Used**:
- `ProjectCard` - Individual project display
- `CreateProjectDialog` - Modal for creating projects
- `ProjectsSidebar` - Project filters and actions

**Redux State**:
- `currentWorkspace.projects` - all projects

**Redux Actions**:
- `addProject(projectData)` - creates new project

---

### ProjectDetails.jsx
**Purpose**: Detailed view of a single project with tabs

**Features**:
- Tab navigation (Overview, Tasks, Analytics, Calendar, Settings)
- Project header with name, status, progress
- Conditional rendering based on active tab

**Components Used**:
- `ProjectOverview` - Project summary
- `ProjectTasks` - Task management
- `ProjectAnalytics` - Charts and metrics
- `ProjectCalendar` - Timeline view
- `ProjectSettings` - Project configuration
- `AddProjectMember` - Add members to project

**Props**: Receives project ID via URL params

**Redux State**:
- `currentWorkspace.projects` - finds project by ID

**State Management**:
- Local state for active tab

---

### TaskDetails.jsx
**Purpose**: Detailed view of a single task

**Features**:
- Task title, description, status
- Assignee information
- Due date
- Comments section
- Edit task details
- Delete task

**Props**: Receives task ID via URL params

**Redux State**:
- `currentWorkspace.projects` - finds task within projects

**Redux Actions**:
- `updateTask(taskData)` - updates task
- `deleteTask(taskId)` - removes task

---

### Team.jsx
**Purpose**: Workspace team management page

**Features**:
- Lists all workspace members
- Member roles (Admin, Member)
- Invite new members
- Remove members
- Member statistics

**Components Used**:
- `InviteMemberDialog` - Modal for inviting members

**Redux State**:
- `currentWorkspace.members` - all members

---

## Feature Components

### StatsGrid.jsx
**Purpose**: Displays key workspace statistics in a grid

**Features**:
- Total projects count
- Active tasks count
- Team members count
- Completion rate

**Props**: None (connects to Redux)

**Redux State**:
- `currentWorkspace` - calculates stats from workspace data

**Calculations**:
- Counts projects, tasks, members
- Calculates average project progress

---

### ProjectCard.jsx
**Purpose**: Card component displaying project summary

**Features**:
- Project name and description
- Status badge
- Priority indicator
- Progress bar
- Team members avatars
- Due date
- Click to navigate to project details

**Props**:
```typescript
{
  project: Project  // Project object
}
```

**Styling**:
- Tailwind CSS with hover effects
- Color-coded priority (High: red, Medium: yellow, Low: green)
- Color-coded status (Active: blue, Planning: purple)

---

### ProjectOverview.jsx
**Purpose**: Overview tab in project details

**Features**:
- Project description
- Key dates (start, end)
- Team lead
- Project members
- Progress visualization
- Quick stats

**Props**:
```typescript
{
  project: Project  // Project object
}
```

---

### ProjectTasks.jsx
**Purpose**: Task management tab in project details

**Features**:
- Task list with filters (All, Todo, In Progress, Done)
- Task type filters (Feature, Bug, Task, Improvement)
- Create new task
- Edit task inline
- Delete tasks (bulk)
- Sort by due date, priority, status
- Search tasks

**Components Used**:
- `CreateTaskDialog` - Modal for creating tasks

**Props**:
```typescript
{
  project: Project  // Project object
}
```

**Redux Actions**:
- `addTask(taskData)` - creates task
- `updateTask(taskData)` - updates task
- `deleteTask(taskIds)` - deletes task(s)

---

### ProjectAnalytics.jsx
**Purpose**: Analytics tab showing project metrics

**Features**:
- Task completion chart (Recharts)
- Task distribution by type (pie chart)
- Task distribution by status (bar chart)
- Progress over time (line chart)
- Team productivity metrics

**Props**:
```typescript
{
  project: Project  // Project object
}
```

**Libraries Used**:
- Recharts for data visualization

**Charts**:
- `LineChart` - Progress over time
- `BarChart` - Tasks by status
- `PieChart` - Tasks by type

---

### ProjectCalendar.jsx
**Purpose**: Calendar view of project tasks and milestones

**Features**:
- Monthly calendar view
- Task due dates highlighted
- Click date to view tasks
- Navigate between months
- Color-coded by priority

**Props**:
```typescript
{
  project: Project  // Project object
}
```

**Libraries Used**:
- date-fns for date manipulation

---

### ProjectSettings.jsx
**Purpose**: Project configuration and settings

**Features**:
- Edit project name, description
- Change project status
- Update priority
- Set start/end dates
- Delete project
- Archive project

**Props**:
```typescript
{
  project: Project  // Project object
}
```

**Redux Actions**:
- `updateWorkspace(workspaceData)` - updates project in workspace

---

### CreateProjectDialog.jsx
**Purpose**: Modal dialog for creating new projects

**Features**:
- Form with validation
- Project name (required)
- Description
- Priority selection
- Status selection
- Start/end dates
- Team lead selection
- Cancel/Submit actions

**Props**:
```typescript
{
  isOpen: boolean
  onClose: () => void
}
```

**Redux Actions**:
- `addProject(projectData)` - creates project

**Validation**:
- Name is required
- End date must be after start date

**Toast Notifications**:
- Success: "Project created successfully"
- Error: "Failed to create project"

---

### CreateTaskDialog.jsx
**Purpose**: Modal dialog for creating new tasks

**Features**:
- Form with validation
- Task title (required)
- Description
- Type selection (Feature, Bug, Task, Improvement, Other)
- Priority selection (High, Medium, Low)
- Status selection (Todo, In Progress, Done)
- Assignee selection
- Due date picker
- Cancel/Submit actions

**Props**:
```typescript
{
  isOpen: boolean
  onClose: () => void
  projectId: string
}
```

**Redux Actions**:
- `addTask(taskData)` - creates task

**Validation**:
- Title is required
- Assignee must be project member

---

### AddProjectMember.jsx
**Purpose**: Add members to a project

**Features**:
- Lists workspace members not in project
- Search members
- Add member to project
- Bulk add

**Props**:
```typescript
{
  project: Project
}
```

**Redux State**:
- `currentWorkspace.members` - available members

---

### InviteMemberDialog.jsx
**Purpose**: Invite new members to workspace

**Features**:
- Email input
- Role selection (Admin, Member)
- Optional message
- Send invitation

**Props**:
```typescript
{
  isOpen: boolean
  onClose: () => void
}
```

**Validation**:
- Valid email format
- Email not already in workspace

---

### TasksSummary.jsx
**Purpose**: Summary of tasks by status

**Features**:
- Pie chart of task distribution
- Task counts (Todo, In Progress, Done)
- Percentage breakdown

**Props**: None (connects to Redux)

**Redux State**:
- `currentWorkspace.projects` - aggregates all tasks

**Libraries Used**:
- Recharts for pie chart

---

### RecentActivity.jsx
**Purpose**: Displays recent workspace activity

**Features**:
- Activity feed
- User actions (created task, updated project, etc.)
- Timestamps (relative time with date-fns)
- User avatars

**Props**: None (connects to Redux)

**Note**: Currently displays mock data; will integrate with activity log when backend is ready

---

### MyTasksSidebar.jsx
**Purpose**: Sidebar showing user's assigned tasks

**Features**:
- Lists tasks assigned to current user
- Grouped by project
- Due date display
- Priority indicators
- Click to navigate to task details

**Props**: None (connects to Redux)

**Redux State**:
- `currentWorkspace.projects` - filters tasks by assignee

---

### ProjectsSidebar.jsx
**Purpose**: Sidebar for project filtering and actions

**Features**:
- Filter by status
- Filter by priority
- Sort options
- Create project button

**Props**:
```typescript
{
  onFilterChange: (filters) => void
}
```

---

## Common Patterns

### Redux Connection
Most components connect to Redux using:
```javascript
import { useSelector, useDispatch } from 'react-redux';

const currentWorkspace = useSelector(state => state.workspace.currentWorkspace);
const dispatch = useDispatch();
```

### Dialog/Modal Pattern
Dialogs use controlled open/close state:
```javascript
const [isOpen, setIsOpen] = useState(false);

<CreateProjectDialog 
  isOpen={isOpen} 
  onClose={() => setIsOpen(false)} 
/>
```

### Toast Notifications
Success/error feedback using react-hot-toast:
```javascript
import toast from 'react-hot-toast';

toast.success('Action completed!');
toast.error('Action failed!');
```

### Navigation
Programmatic navigation using React Router:
```javascript
import { useNavigate } from 'react-router-dom';

const navigate = useNavigate();
navigate('/projectsDetail', { state: { projectId } });
```

---

## Styling Conventions

### Tailwind Classes
- **Cards**: `bg-white dark:bg-gray-800 rounded-lg shadow-md p-6`
- **Buttons**: `bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-md`
- **Inputs**: `border border-gray-300 rounded-md px-3 py-2 focus:ring-2 focus:ring-blue-500`
- **Badges**: `px-2 py-1 rounded-full text-xs font-semibold`

### Color Coding
- **Priority High**: Red (`bg-red-100 text-red-800`)
- **Priority Medium**: Yellow (`bg-yellow-100 text-yellow-800`)
- **Priority Low**: Green (`bg-green-100 text-green-800`)
- **Status Active**: Blue (`bg-blue-100 text-blue-800`)
- **Status Planning**: Purple (`bg-purple-100 text-purple-800`)

---

## Component Best Practices

1. **Single Responsibility**: Each component has one clear purpose
2. **Prop Validation**: TypeScript types recommended for props
3. **Reusability**: Shared components extracted (e.g., ProjectCard)
4. **State Management**: Redux for global state, local state for UI-only state
5. **Error Handling**: Toast notifications for user feedback
6. **Accessibility**: Semantic HTML, ARIA labels where needed
7. **Performance**: Memoization for expensive computations (can be added)

---

## Future Component Enhancements

1. **Loading States**: Add skeleton loaders for async operations
2. **Error Boundaries**: Wrap components for graceful error handling
3. **Virtualization**: For long lists (tasks, projects)
4. **Drag and Drop**: Reorder tasks, move between statuses
5. **Rich Text Editor**: For task/project descriptions
6. **File Upload**: Attach files to tasks
7. **Real-time Updates**: WebSocket integration for live collaboration
