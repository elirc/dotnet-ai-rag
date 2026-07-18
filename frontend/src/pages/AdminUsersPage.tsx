import { useQuery } from '@tanstack/react-query';
import { getUsers } from '../app/api';
import { useAuthStore } from '../app/authStore';
import { EmptyState, PageScaffold } from './PageScaffold';

export function AdminUsersPage() {
  const token = useAuthStore((state) => state.token);
  const query = useQuery({ queryKey: ['users'], queryFn: () => getUsers(token!), enabled: Boolean(token) });

  if (query.isLoading) return <EmptyState message="Loading users..." />;
  if (query.isError) return <EmptyState message={`Users unavailable: ${query.error.message}`} />;

  return (
    <PageScaffold eyebrow="Administration" title="Users">
      <div className="data-table">
        <div className="data-row header"><span>Name</span><span>Email</span><span>Role</span><span>Status</span></div>
        {query.data!.map((user) => (
          <div className="data-row four" key={user.id}>
            <strong>{user.displayName}</strong>
            <span>{user.email}</span>
            <span>{user.role}</span>
            <span>{user.isActive ? 'Active' : 'Disabled'}</span>
          </div>
        ))}
      </div>
    </PageScaffold>
  );
}
