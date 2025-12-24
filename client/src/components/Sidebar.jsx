import { useEffect, useRef } from 'react'
import { NavLink, useNavigate } from 'react-router-dom'
import { useAuthStore } from '../stores/useAuthStore'
// import MyTasksSidebar from './MyTasksSidebar'    
import ProjectSidebar from './ProjectsSidebar'
import WorkspaceDropdown from './WorkspaceDropdown'
import { FolderOpenIcon, LayoutDashboardIcon, SettingsIcon, UsersIcon, LogOutIcon } from 'lucide-react'
import toast from 'react-hot-toast'

const Sidebar = ({ isSidebarOpen, setIsSidebarOpen }) => {
    const navigate = useNavigate();
    const { logout } = useAuthStore();

    const menuItems = [
        { name: 'Dashboard', href: '/', icon: LayoutDashboardIcon },
        { name: 'Projects', href: '/projects', icon: FolderOpenIcon },
        { name: 'Team', href: '/team', icon: UsersIcon },
    ]

    const sidebarRef = useRef(null);

    useEffect(() => {
        function handleClickOutside(event) {
            if (sidebarRef.current && !sidebarRef.current.contains(event.target)) {
                setIsSidebarOpen(false);
            }
        }
        document.addEventListener("mousedown", handleClickOutside);
        return () => document.removeEventListener("mousedown", handleClickOutside);
    }, [setIsSidebarOpen]);

    const handleLogout = async () => {
        try {
            await logout();
            toast.success('Logged out successfully');
            navigate('/login');
        } catch (error) {
            toast.error('Logout failed');
        }
    };

    return (
        <div ref={sidebarRef} className={`z-10 bg-white dark:bg-zinc-900 min-w-68 flex flex-col h-screen border-r border-gray-200 dark:border-zinc-800 max-sm:absolute transition-all ${isSidebarOpen ? 'left-0' : '-left-full'} `} >
            <WorkspaceDropdown />
            <hr className='border-gray-200 dark:border-zinc-800' />
            <div className='flex-1 overflow-y-scroll no-scrollbar flex flex-col'>
                <div>
                    <div className='p-4'>
                        {menuItems.map((item) => (
                            <NavLink to={item.href} key={item.name} className={({ isActive }) => `flex items-center gap-3 py-2 px-4 text-gray-800 dark:text-zinc-100 cursor-pointer rounded transition-all  ${isActive ? 'bg-gray-100 dark:bg-zinc-900 dark:bg-gradient-to-br dark:from-zinc-800 dark:to-zinc-800/50  dark:ring-zinc-800' : 'hover:bg-gray-50 dark:hover:bg-zinc-800/60'}`} >
                                <item.icon size={16} />
                                <p className='text-sm truncate'>{item.name}</p>
                            </NavLink>
                        ))}
                        <button className='flex w-full items-center gap-3 py-2 px-4 text-gray-800 dark:text-zinc-100 cursor-pointer rounded hover:bg-gray-50 dark:hover:bg-zinc-800/60 transition-all'>
                            <SettingsIcon size={16} />
                            <p className='text-sm truncate'>Settings</p>
                        </button>
                        <button 
                            onClick={handleLogout}
                            className='flex w-full items-center gap-3 py-2 px-4 text-red-600 dark:text-red-400 cursor-pointer rounded hover:bg-red-50 dark:hover:bg-red-900/20 transition-all'
                        >
                            <LogOutIcon size={16} />
                            <p className='text-sm truncate'>Logout</p>
                        </button>
                    </div>
                    {/* <MyTasksSidebar /> */}
                    <ProjectSidebar />
                </div>


            </div>

        </div>
    )
}

export default Sidebar
