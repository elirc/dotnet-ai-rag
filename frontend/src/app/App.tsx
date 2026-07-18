import { Navigate, Route, Routes } from 'react-router-dom';
import { AppLayout } from '../components/AppLayout';
import { ProtectedRoute } from '../components/ProtectedRoute';
import { DashboardPage } from '../pages/DashboardPage';
import { AgentsPage } from '../pages/AgentsPage';
import { AdminSettingsPage } from '../pages/AdminSettingsPage';
import { AdminUsersPage } from '../pages/AdminUsersPage';
import { EvalsPage } from '../pages/EvalsPage';
import { IncidentsPage } from '../pages/IncidentsPage';
import { KnowledgeBasePage } from '../pages/KnowledgeBasePage';
import { LogsPage } from '../pages/LogsPage';
import { LoginPage } from '../pages/LoginPage';
import { PlaceholderPage } from '../pages/PlaceholderPage';
import { TicketsPage } from '../pages/TicketsPage';

export function App() {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />
      <Route
        element={
          <ProtectedRoute>
            <AppLayout />
          </ProtectedRoute>
        }
      >
        <Route path="/" element={<Navigate to="/dashboard" replace />} />
        <Route path="/dashboard" element={<DashboardPage />} />
        <Route path="/tickets" element={<TicketsPage />} />
        <Route path="/incidents" element={<IncidentsPage />} />
        <Route path="/logs" element={<LogsPage />} />
        <Route path="/knowledge-base" element={<KnowledgeBasePage />} />
        <Route path="/rag-assistant" element={<PlaceholderPage title="RAG Assistant" description="Knowledge documents are seeded; chunking, retrieval scoring, and cited answers are the next production-depth step." />} />
        <Route path="/agents" element={<AgentsPage />} />
        <Route path="/agents/runs" element={<AgentsPage />} />
        <Route path="/evals" element={<EvalsPage />} />
        <Route path="/admin/users" element={<AdminUsersPage />} />
        <Route path="/admin/settings" element={<AdminSettingsPage />} />
      </Route>
      <Route path="*" element={<Navigate to="/dashboard" replace />} />
    </Routes>
  );
}
