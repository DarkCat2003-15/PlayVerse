type PlayVerseRuntimeConfig = {
  production?: boolean;
  apiUrl?: string;
};

const runtimeConfig = (
  globalThis as typeof globalThis & {
    __PLAYVERSE_CONFIG__?: PlayVerseRuntimeConfig;
  }
).__PLAYVERSE_CONFIG__;

export const environment = {
  production: runtimeConfig?.production ?? false,
  apiUrl: runtimeConfig?.apiUrl ?? 'http://localhost:5168/api',
};
