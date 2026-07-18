export type AuthUser = {
  id: string;
  email: string;
  displayName: string;
  role: 'Admin' | 'Engineer' | 'SupportAnalyst' | 'Viewer';
};

export type AuthResponse = {
  accessToken: string;
  expiresAt: string;
  user: AuthUser;
};

export type DashboardSummary = {
  openTickets: number;
  activeIncidents: number;
  criticalAlerts: number;
  averageResolutionHours: number;
  agentInvestigationsToday: number;
  failedBackgroundJobs: number;
  evalPassRate: number;
  recentAiRecommendations: AiRecommendation[];
};

export type AiRecommendation = {
  id: string;
  title: string;
  source: string;
  status: string;
  createdAt: string;
};

export type PagedResult<T> = {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
};

export type TicketListItem = {
  id: string;
  title: string;
  status: string;
  priority: string;
  category: string;
  assignedToUserId: string | null;
  relatedIncidentId: string | null;
  tags: string[];
  updatedAt: string;
};

export type IncidentListItem = {
  id: string;
  title: string;
  severity: string;
  status: string;
  primaryService: string;
  commanderUserId: string | null;
  detectedAt: string;
  updatedAt: string;
};

export type LogEntry = {
  id: string;
  timestamp: string;
  serviceName: string;
  environment: string;
  level: string;
  message: string;
  traceId: string;
  correlationId: string;
  host: string;
  exceptionType: string | null;
  linkedIncidentId: string | null;
};

export type KnowledgeDocument = {
  id: string;
  title: string;
  sourceType: string;
  tags: string[];
  updatedAt: string;
};

export type AgentRun = {
  id: string;
  agentName: string;
  status: string;
  relatedTicketId: string | null;
  relatedIncidentId: string | null;
  totalToolCalls: number;
  startedAt: string;
};

export type EvalRun = {
  id: string;
  suiteName: string;
  status: string;
  totalTests: number;
  passedTests: number;
  failedTests: number;
  averageScore: number;
  startedAt: string;
};

export type AdminSetting = {
  id: string;
  key: string;
  value: string;
  description: string;
  updatedAt: string;
};

export type HumanApproval = {
  id: string;
  recommendationType: string;
  status: string;
  createdAt: string;
};

export type UserSummary = {
  id: string;
  email: string;
  displayName: string;
  role: string;
  isActive: boolean;
  createdAt: string;
};
