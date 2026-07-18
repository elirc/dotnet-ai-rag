import { create } from 'zustand';
import type { AuthResponse, AuthUser } from './types';

type AuthState = {
  token: string | null;
  expiresAt: string | null;
  user: AuthUser | null;
  setSession: (session: AuthResponse) => void;
  clearSession: () => void;
};

const storageKey = 'netaiops.session';

function loadSession(): Pick<AuthState, 'token' | 'expiresAt' | 'user'> {
  const raw = window.localStorage.getItem(storageKey);
  if (!raw) {
    return { token: null, expiresAt: null, user: null };
  }

  try {
    const parsed = JSON.parse(raw) as AuthResponse;
    if (new Date(parsed.expiresAt).getTime() <= Date.now()) {
      window.localStorage.removeItem(storageKey);
      return { token: null, expiresAt: null, user: null };
    }

    return {
      token: parsed.accessToken,
      expiresAt: parsed.expiresAt,
      user: parsed.user,
    };
  } catch {
    window.localStorage.removeItem(storageKey);
    return { token: null, expiresAt: null, user: null };
  }
}

export const useAuthStore = create<AuthState>((set) => ({
  ...loadSession(),
  setSession: (session) => {
    window.localStorage.setItem(storageKey, JSON.stringify(session));
    set({
      token: session.accessToken,
      expiresAt: session.expiresAt,
      user: session.user,
    });
  },
  clearSession: () => {
    window.localStorage.removeItem(storageKey);
    set({ token: null, expiresAt: null, user: null });
  },
}));
