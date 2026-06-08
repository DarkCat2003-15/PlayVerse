import { writeFile } from 'node:fs/promises';

const apiUrl = process.env.API_URL?.trim().replace(/\/+$/, '');

if (!apiUrl) {
  throw new Error('API_URL es obligatoria para construir el sitio en Render.');
}

const runtimeConfig = `window.__PLAYVERSE_CONFIG__ = ${JSON.stringify(
  {
    production: true,
    apiUrl,
  },
  null,
  2,
)};\n`;

await writeFile(new URL('../public/config.js', import.meta.url), runtimeConfig, 'utf8');
console.log(`Configuracion de produccion creada para ${apiUrl}`);
