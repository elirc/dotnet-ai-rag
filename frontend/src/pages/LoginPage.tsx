import { zodResolver } from '@hookform/resolvers/zod';
import { useMutation } from '@tanstack/react-query';
import { useForm } from 'react-hook-form';
import { useLocation, useNavigate } from 'react-router-dom';
import { z } from 'zod';
import { login } from '../app/api';
import { useAuthStore } from '../app/authStore';

const loginSchema = z.object({
  email: z.string().email(),
  password: z.string().min(8),
});

type LoginForm = z.infer<typeof loginSchema>;

export function LoginPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const setSession = useAuthStore((state) => state.setSession);
  const from = (location.state as { from?: string } | null)?.from ?? '/dashboard';
  const form = useForm<LoginForm>({
    resolver: zodResolver(loginSchema),
    defaultValues: {
      email: 'admin@example.com',
      password: 'Password123!',
    },
  });

  const mutation = useMutation({
    mutationFn: (values: LoginForm) => login(values.email, values.password),
    onSuccess: (session) => {
      setSession(session);
      navigate(from, { replace: true });
    },
  });

  return (
    <main className="login-page">
      <section className="login-panel" aria-labelledby="login-title">
        <div>
          <p className="eyebrow">NetAiOps</p>
          <h1 id="login-title">Sign in to operations</h1>
          <p className="muted">Use a seeded role account to explore the Phase 1 foundation.</p>
        </div>
        <form className="login-form" onSubmit={form.handleSubmit((values) => mutation.mutate(values))}>
          <label>
            Email
            <input type="email" autoComplete="email" {...form.register('email')} />
            {form.formState.errors.email && <span>{form.formState.errors.email.message}</span>}
          </label>
          <label>
            Password
            <input type="password" autoComplete="current-password" {...form.register('password')} />
            {form.formState.errors.password && <span>{form.formState.errors.password.message}</span>}
          </label>
          {mutation.error && <p className="form-error">{mutation.error.message}</p>}
          <button type="submit" disabled={mutation.isPending}>
            {mutation.isPending ? 'Signing in...' : 'Sign in'}
          </button>
        </form>
        <div className="seed-list">
          <strong>Seed users</strong>
          <span>admin@example.com / engineer@example.com / support@example.com / viewer@example.com</span>
          <span>Password: Password123!</span>
        </div>
      </section>
    </main>
  );
}
