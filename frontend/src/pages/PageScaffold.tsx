import type { ReactNode } from 'react';

type PageScaffoldProps = {
  eyebrow: string;
  title: string;
  action?: ReactNode;
  children: ReactNode;
};

export function PageScaffold({ eyebrow, title, action, children }: PageScaffoldProps) {
  return (
    <section className="dashboard-grid">
      <div className="section-header">
        <div>
          <p className="eyebrow">{eyebrow}</p>
          <h2>{title}</h2>
        </div>
        {action}
      </div>
      {children}
    </section>
  );
}

export function EmptyState({ message }: { message: string }) {
  return <div className="state-box">{message}</div>;
}
