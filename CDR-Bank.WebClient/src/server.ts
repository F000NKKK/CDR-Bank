import {
  AngularNodeAppEngine,
  createNodeRequestHandler,
  isMainModule,
  writeResponseToNodeResponse,
} from '@angular/ssr/node';
import express from 'express';
import { dirname, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';
import fetch from 'node-fetch';
import { provideServerRendering } from '@angular/platform-server';

const serverDistFolder = dirname(fileURLToPath(import.meta.url));
const browserDistFolder = resolve(serverDistFolder, '../browser');

const app = express();
const angularApp = new AngularNodeAppEngine();

app.post('/api/cdr-server/v1.0/user/register', express.json(), async (req, res) => {
  try
  {
    const url = 'http://localhost:5262/weather-controller/weather';

    const response = await fetch(url, {
      method: 'GET',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    });

    if (response.ok)
    {
      const data = await response.json();
      res.json(data);
      console.log(data);
    }
    else
    {
     res.json({ data: 'Requested failed' });
     console.error('Requested failed', response.status);
    }
  }
  catch (error)
  {
    console.error('Proxy error:', error);
    res.status(500).json({error: "Ошибка при обращении к внешнему API"});
  }
});

app.patch('/api/cdr-server/v1.0/user/update', express.json(), (req, res) => {
  res.json({answer: "OK!"});
});

app.get('/api/cdr-server/v1.0/user/login', express.json(), (req, res) => {
  return res
});

app.get('/api/cdr-server/v1.0/quick-tasks', express.json(), (req, res) => {
  return res
});

app.get('/api/cdr-server/v1.0/transaction-tasks', express.json(), (req, res) => {
  return res
});

/**
 * Serve static files from /browser
 */
app.use(
  express.static(browserDistFolder, {
    maxAge: '1y',
    index: false,
    redirect: false,
  }),
);

/**
 * Handle all other requests by rendering the Angular application.
 */
app.use('/**', (req, res, next) => {
  angularApp
    .handle(req)
    .then((response) =>
      response ? writeResponseToNodeResponse(response, res) : next(),
    )
    .catch(next);
});

/**
 * Start the server if this module is the main entry point.
 * The server listens on the port defined by the `PORT` environment variable, or defaults to 4000.
 */
if (isMainModule(import.meta.url)) {
  const port = process.env['PORT'] || 4000;
  app.listen(port, () => {
    console.log(`Node Express server listening on http://localhost:${port}`);
  });
}

/**
 * Request handler used by the Angular CLI (for dev-server and during build) or Firebase Cloud Functions.
 */
export const reqHandler = createNodeRequestHandler(app);
