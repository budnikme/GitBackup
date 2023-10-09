const WEB_API_URL: string = 'https://localhost:7127/api';

const buildUrl = (...resources: (string | number)[]): string => [WEB_API_URL]
  .concat(resources.map(r => r.toString()))
  .join('/');

export const URLS = {
  REPOSITORIES: buildUrl('repositories'),
};
