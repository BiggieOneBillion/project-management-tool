import { useState } from "react";
import { useCreateWorkspace } from "../hooks";
import toast from "react-hot-toast";
import { X } from "lucide-react";
import { useAuthStore } from "../stores/useAuthStore";
import { useNavigate } from "react-router-dom";


const Setup = () => {
    const [formData, setFormData] = useState({
        name: '',
        description: '',
        imageUrl: '',
    });
    
    const { mutate: createWorkspace, isPending } = useCreateWorkspace();
    const { user } = useAuthStore();
    
    const handleChange = (e) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value,
        });
    };

    const navigate = useNavigate();
    
    const handleSubmit = (e) => {
        e.preventDefault();
        
        if (!formData.name.trim()) {
            toast.error('Workspace name is required');
            return;
        }

        // Generate slug from name
        const slug = formData.name.toLowerCase().replace(/\s+/g, '-').replace(/[^a-z0-9-]/g, '');

        createWorkspace({
            name: formData.name,
            slug: slug,
            description: formData.description || null,
            ownerId: user?.id,
        }, {
            onSuccess: () => {
                // Reset form
                setFormData({ name: '', description: '', imageUrl: '' });
                toast.success('Workspace created successfully!');
                // Navigate to dashboard - WorkspaceProtectedRoute will now allow access
                navigate('/');
            },
            onError: (error) => {
                toast.error(error?.response?.data?.message || 'Failed to create workspace');
            }
        });
    };
    
    return (
    <div className="flex flex-col h-screen lg:flex-col justify-between items-start lg:items-center lg:justify-center gap-6 ">
        <div className="space-y-4 flex flex-col items-center justify-center">
            <h2 className="text-2xl font-semibold text-gray-900 dark:text-white">Set Up Your Workspace</h2>
           <p className="text-center text-gray-500 dark:text-gray-400">You have to create a workspace to get started</p>
        </div>
        <div className="bg-white dark:bg-gray-800 rounded-lg shadow-xl w-full max-w-md mx-4 border border-gray-200 dark:border-gray-700">
                {/* Header */}
                <div className="flex items-center justify-between p-6 border-b border-gray-200 dark:border-gray-700">
                <h2 className="text-xl font-semibold text-gray-900 dark:text-white">
                    Create Workspace
                </h2>
                {/* <button
                    onClick={onClose}
                    className="text-gray-400 hover:text-gray-600 dark:hover:text-gray-300 transition-colors"
                >
                    <X size={24} />
                </button> */}
                </div>

                {/* Form */}
                <form onSubmit={handleSubmit} className="p-6 space-y-4">
                {/* Workspace Name */}
                <div>
                    <label htmlFor="name" className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                    Workspace Name *
                    </label>
                    <input
                    type="text"
                    id="name"
                    name="name"
                    value={formData.name}
                    onChange={handleChange}
                    placeholder="e.g., My Company"
                    className="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent dark:bg-gray-700 dark:text-white"
                    required
                    />
                </div>

                {/* Description */}
                <div>
                    <label htmlFor="description" className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                    Description
                    </label>
                    <textarea
                    id="description"
                    name="description"
                    value={formData.description}
                    onChange={handleChange}
                    placeholder="What's this workspace for?"
                    rows={3}
                    className="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent dark:bg-gray-700 dark:text-white resize-none"
                    />
                </div>

                {/* Image URL */}
                <div>
                    <label htmlFor="imageUrl" className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                    Image URL (optional)
                    </label>
                    <input
                    type="url"
                    id="imageUrl"
                    name="imageUrl"
                    value={formData.imageUrl}
                    onChange={handleChange}
                    placeholder="https://example.com/logo.png"
                    className="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent dark:bg-gray-700 dark:text-white"
                    />
                    <p className="mt-1 text-xs text-gray-500 dark:text-gray-400">
                    Leave empty to use default image
                    </p>
                </div>

                {/* Buttons */}
                <div className="flex gap-3 pt-4">
                    {/* <button
                    type="button"
                    onClick={onClose}
                    className="flex-1 px-4 py-2 border border-gray-300 dark:border-gray-600 text-gray-700 dark:text-gray-300 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors"
                    >
                    Cancel
                    </button> */}
                    <button
                    // onClick={() => alert('Creating workspace...')}
                    type="submit"
                    disabled={isPending}
                    className="flex-1 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
                    >
                    {isPending ? 'Creating...' : 'Create Workspace'}
                    </button>
                </div>
                </form>
            </div>
    </div>
    );
};

export default Setup;