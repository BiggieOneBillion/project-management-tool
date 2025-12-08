import { useState } from "react";
import { Mail, UserPlus } from "lucide-react";
import { useWorkspaceStore } from "../stores/useWorkspaceStore";
import { useSendInvitation } from "../hooks";

const InviteMemberDialog = ({ isDialogOpen, setIsDialogOpen }) => {
    // We assume currentWorkspace is passed or available via context/store if needed, 
    // but the Dialog implies context. 
    // Actually, Team.jsx does NOT pass workspaceId.
    // So we invoke useWorkspaceStore just to get ID.
    const {currentWorkspace} = useWorkspaceStore(state => state); // Import locally if needed or keep import
    // But I removed import. I should re-add it or pass workspaceId as prop.
    // Team.jsx passes `isDialogOpen`. It does NOT pass `workspaceId`.
    // I should probably pass `workspaceId` prop from Team.jsx?
    // Or keep useWorkspaceStore import just for ID.
    // I will keep useWorkspaceStore import for ID.
    
    // START_REPLACEMENT
    // Re-import useWorkspaceStore since I removed it in first chunk? 
    // Ah, I replaced lines 3-4. I should have kept useWorkspaceStore.
    // I will fix imports in next step/or correct this chunk.
    
    // Let's assume I fix imports in one go.
    
    const { mutate: sendInvitation, isPending: isSubmitting, error: mutationError } = useSendInvitation();
    
    const [success, setSuccess] = useState(null);
    const [formData, setFormData] = useState({
        email: "",
        role: "org:member",
    });

    const handleSubmit = (e) => {
        e.preventDefault();
        setSuccess(null);

        // Map UI role to backend enum (use integer values)
        const roleMapping = {
            "org:member": 1,  // WorkspaceRole.MEMBER
            "org:admin": 2,   // WorkspaceRole.ADMIN
        };
        const backendRole = roleMapping[formData.role] || 1;

        let payload = {
            workspaceId: currentWorkspace.id,
            email: formData.email,
            role: backendRole
        }


        sendInvitation(payload, {
            onSuccess: () => {
                setSuccess(`Invitation sent to ${formData.email}`);
                setFormData({ email: "", role: "org:member" });
                setTimeout(() => {
                    setIsDialogOpen(false);
                    setSuccess(null);
                }, 1500);
            }
        });
    };
    
    const error = mutationError?.message;

    if (!isDialogOpen) return null;

    return (
        <div className="fixed inset-0 bg-black/20 dark:bg-black/50 backdrop-blur flex items-center justify-center z-50">
            <div className="bg-white dark:bg-zinc-950 border border-zinc-300 dark:border-zinc-800 rounded-xl p-6 w-full max-w-md text-zinc-900 dark:text-zinc-200">
                {/* Header */}
                <div className="mb-4">
                    <h2 className="text-xl font-bold flex items-center gap-2">
                        <UserPlus className="size-5 text-zinc-900 dark:text-zinc-200" /> Invite Team Member
                    </h2>
                    {currentWorkspace && (
                        <p className="text-sm text-zinc-700 dark:text-zinc-400">
                            Inviting to workspace: <span className="text-blue-600 dark:text-blue-400">{currentWorkspace.name}</span>
                        </p>
                    )}
                </div>

                {/* Form */}
                <form onSubmit={handleSubmit} className="space-y-4">
                    {/* Success Message */}
                    {success && (
                        <div className="p-3 rounded bg-green-50 dark:bg-green-900/20 border border-green-200 dark:border-green-800 text-green-700 dark:text-green-400 text-sm">
                            {success}
                        </div>
                    )}

                    {/* Error Message */}
                    {error && (
                        <div className="p-3 rounded bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 text-red-700 dark:text-red-400 text-sm">
                            {error}
                        </div>
                    )}

                    {/* Email */}
                    <div className="space-y-2">
                        <label htmlFor="email" className="text-sm font-medium text-zinc-900 dark:text-zinc-200">
                            Email Address
                        </label>
                        <div className="relative">
                            <Mail className="absolute left-3 top-1/2 -translate-y-1/2 text-zinc-500 dark:text-zinc-400 w-4 h-4" />
                            <input type="email" value={formData.email} onChange={(e) => setFormData({ ...formData, email: e.target.value })} placeholder="Enter email address" className="pl-10 mt-1 w-full rounded border border-zinc-300 dark:border-zinc-700 dark:bg-zinc-900 text-zinc-900 dark:text-zinc-200 text-sm placeholder-zinc-400 dark:placeholder-zinc-500 py-2 focus:outline-none focus:border-blue-500" required />
                        </div>
                    </div>

                    {/* Role */}
                    <div className="space-y-2">
                        <label className="text-sm font-medium text-zinc-900 dark:text-zinc-200">Role</label>
                        <select value={formData.role} onChange={(e) => setFormData({ ...formData, role: e.target.value })} className="w-full rounded border border-zinc-300 dark:border-zinc-700 dark:bg-zinc-900 text-zinc-900 dark:text-zinc-200 py-2 px-3 mt-1 focus:outline-none focus:border-blue-500 text-sm" >
                            <option value="org:member">Member</option>
                            <option value="org:admin">Admin</option>
                        </select>
                    </div>

                    {/* Footer */}
                    <div className="flex justify-end gap-3 pt-2">
                        <button type="button" onClick={() => setIsDialogOpen(false)} className="px-5 py-2 rounded text-sm border border-zinc-300 dark:border-zinc-700 text-zinc-900 dark:text-zinc-200 hover:bg-zinc-100 dark:hover:bg-zinc-800 transition" >
                            Cancel
                        </button>
                        <button type="submit" disabled={isSubmitting || !currentWorkspace} className="px-5 py-2 rounded text-sm bg-gradient-to-br from-blue-500 to-blue-600 text-white disabled:opacity-50 hover:opacity-90 transition" >
                            {isSubmitting ? "Sending..." : "Send Invitation"}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default InviteMemberDialog;
