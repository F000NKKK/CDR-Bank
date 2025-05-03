import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  {
    path: '**',
    renderMode: RenderMode.Prerender
  },
  {
    path: 'api/cdr-server/v1.0/user/register',
    renderMode: RenderMode.Server
  }
];
