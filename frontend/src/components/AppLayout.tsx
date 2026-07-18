import { NavLink, Outlet, useNavigate } from 'react-router-dom';
import { useAuthStore } from '../app/authStore';
import type { AuthUser } from '../app/types';

type NavItem = {
  path: string;
  label: string;
  roles?: AuthUser['role'][];
};

const navItems: NavItem[] = [
  { path: '/dashboard', label: 'Dashboard' },
  { path: '/tickets', label: 'Tickets' },
  { path: '/incidents', label: 'Incidents' },
  { path: '/logs', label: 'Logs' },
  { path: '/knowledge-base', label: 'Knowledge Base' },
  { path: '/rag-assistant', label: 'RAG Assistant' },
  { path: '/agents', label: 'Agents', roles: ['Admin', 'Engineer'] },
  { path: '/evals', label: 'Evals', roles: ['Admin'] },
  { path: '/admin/users', label: 'Users', roles: ['Admin'] },
  { path: '/admin/settings', label: 'Settings', roles: ['Admin'] },
] as const;

export function AppLayout() {
  const user = useAuthStore((state) => state.user);
  const clearSession = useAuthStore((state) => state.clearSession);
  const navigate = useNavigate();

  const logout = () => {
    clearSession();
    navigate('/login', { replace: true });
  };

  return (
    <div className="app-shell">
      <aside className="sidebar">
        <div>
          <div className="brand">NetAiOps</div>
          <div className="brand-subtitle">AI Operations Console</div>
        </div>
        <nav className="nav-list" aria-label="Primary">
          {navItems
            .filter((item) => !item.roles || item.roles.includes(user?.role ?? 'Viewer'))
            .map((item) => (
              <NavLink key={item.path} to={item.path} className={({ isActive }) => (isActive ? 'active' : undefined)}>
                {item.label}
              </NavLink>
            ))}
        </nav>
      </aside>
      <div className="main-frame">
        <header className="topbar">
          <div>
            <p className="eyebrow">Internal Engineering Support</p>
            <h1>Operations workspace</h1>
          </div>
          <div className="user-menu">
            <div>
              <strong>{user?.displayName}</strong>
              <span>{user?.role}</span>
            </div>
            <button type="button" onClick={logout}>
              Sign out
            </button>
          </div>
        </header>
        <main className="content">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
