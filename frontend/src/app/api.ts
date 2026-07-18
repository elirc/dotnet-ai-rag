import type {
  AdminSetting,
  AgentRun,
  AuthResponse,
  DashboardSummary,
  EvalRun,
  HumanApproval,
  IncidentListItem,
  KnowledgeDocument,
  LogEntry,
  PagedResult,
  TicketListItem,
  UserSummary,
} from './types';

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL ?? 'https://localhost:7223/api';

type RequestOptions = {
  token?: string | null;
  body?: unknown;
  method?: 'GET' | 'POST';
};

async function request<T>(path: string, options: RequestOptions = {}): Promise<T> {
  const response = await fetch(`${apiBaseUrl}${path}`, {
    method: options.method ?? (options.body ? 'POST' : 'GET'),
    headers: {
      'Content-Type': 'application/json',
      ...(options.token ? { Authorization: `Bearer ${options.token}` } : {}),
    },
    body: options.body ? JSON.stringify(options.body) : undefined,
  });

  if (!response.ok) {
    const fallback = `Request failed with ${response.status}`;
    const problem = await response.json().catch(() => ({ message: fallback }));
    throw new Error(problem.message ?? fallback);
  }

  return response.json() as Promise<T>;
}

export function login(email: string, password: string) {
  return request<AuthResponse>('/auth/login', {
    body: { email, password },
  });
}

export function getDashboardSummary(token: string) {
  return request<DashboardSummary>('/dashboard/summary', { token });
}

export function getTickets(token: string) {
  return request<PagedResult<TicketListItem>>('/tickets?pageSize=50', { token });
}

export function getIncidents(token: string) {
  return request<PagedResult<IncidentListItem>>('/incidents?pageSize=50', { token });
}

export function getLogs(token: string) {
  return request<PagedResult<LogEntry>>('/logs?pageSize=50', { token });
}

export function getKnowledgeDocuments(token: string) {
  return request<KnowledgeDocument[]>('/knowledge-documents', { token });
}

export function getAgentRuns(token: string) {
  return request<AgentRun[]>('/agents/runs', { token });
}

export function getApprovals(token: string) {
  return request<HumanApproval[]>('/approvals', { token });
}

export function getEvalRuns(token: string) {
  return request<EvalRun[]>('/evals/runs', { token });
}

export function getUsers(token: string) {
  return request<UserSummary[]>('/users', { token });
}

export function getAdminSettings(token: string) {
  return request<AdminSetting[]>('/admin/settings', { token });
}

export function runIncidentInvestigation(token: string, incidentId: string) {
  return request<AgentRun>(`/incidents/${incidentId}/run-investigation`, { token, method: 'POST' });
}
