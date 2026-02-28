# Car-comparator-dashboard

[![Deploy Frontend to GitHub Pages](https://github.com/alexandru360/Car-comparator-dashboard/actions/workflows/deploy-pages.yml/badge.svg)](https://github.com/alexandru360/Car-comparator-dashboard/actions/workflows/deploy-pages.yml)
[![Deploy API to Azure App Service](https://github.com/alexandru360/Car-comparator-dashboard/actions/workflows/deploy-api.yml/badge.svg)](https://github.com/alexandru360/Car-comparator-dashboard/actions/workflows/deploy-api.yml)
[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Blazor WebAssembly](https://img.shields.io/badge/Blazor-WebAssembly-512BD4?logo=blazor)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![Azure App Service](https://img.shields.io/badge/Azure-App%20Service-0078D4?logo=microsoftazure)](https://azure.microsoft.com/products/app-service)
[![Live Demo](https://img.shields.io/badge/Live-Demo-brightgreen?logo=github)](https://alexandru360.github.io/Car-comparator-dashboard/)

Car comparator project

## Live Demo

- **Frontend (Blazor WebAssembly):** https://alexandru360.github.io/Car-comparator-dashboard/
- **API (Azure App Service):** https://carcomparator-api.azurewebsites.net

## Deployment

The frontend is automatically deployed to GitHub Pages via GitHub Actions on every push to `main`. The API deployment to Azure is optional and must be triggered manually.

### Frontend → GitHub Pages

The Blazor WebAssembly app is built and deployed to GitHub Pages. The workflow is defined in
`.github/workflows/deploy-pages.yml`.

**Required repository secret:**

| Secret | Description |
|--------|-------------|
| `API_BASE_URL` | Base URL of the deployed API (e.g. `https://carcomparator-api.azurewebsites.net`) |

> **Setup:** Go to **Settings → Pages** and set the source to **GitHub Actions**.

### API → Azure App Service (optional, manual)

The ASP.NET Core API can be deployed to Azure App Service via a manually-triggered workflow
defined in `.github/workflows/deploy-api.yml`. This workflow does **not** run automatically on push
— trigger it from **Actions → Deploy API to Azure App Service → Run workflow** only when needed.

**Required repository secret:**

| Secret | Description |
|--------|-------------|
| `AZURE_WEBAPP_PUBLISH_PROFILE` | Publish profile downloaded from the Azure App Service resource |

> **Setup:** Create an Azure App Service (Free F1 tier is sufficient), download the publish profile,
> and add it as the `AZURE_WEBAPP_PUBLISH_PROFILE` secret in **Settings → Secrets and variables → Actions**.

A `Dockerfile` is also provided in the repository root if you prefer a container-based deployment.

## Local Development

```bash
# Run the API
dotnet run --project src/CarComparator.Api

# Run the frontend
dotnet run --project src/CarComparator.Web
```
