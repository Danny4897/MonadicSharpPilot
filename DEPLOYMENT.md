# MonadicSharp Test Environment - Free Deployment Guide

## 🚀 Deployment Options (GRATIS)

### 1. **Railway** (Raccomandato)
```bash
# Installa Railway CLI
npm install -g @railway/cli

# Login e deploy
railway login
railway init
railway up
```

### 2. **Render.com**
- Collega il repo GitHub
- Seleziona "Web Service"
- Build Command: `dotnet publish -c Release -o out`
- Start Command: `dotnet out/backend.dll`

### 3. **Fly.io**
```bash
# Installa flyctl
curl -L https://fly.io/install.sh | sh

# Deploy
fly launch
fly deploy
```

## 🗄️ Database Options (GRATIS)

### 1. **SQLite** (Per test leggeri)
- Incluso nel container
- Ideale per demo e test

### 2. **PlanetScale** (MySQL compatibile)
- 5GB gratuiti
- Ottime performance

### 3. **Railway PostgreSQL**
- 512MB gratuiti
- Integrato con il deploy

## 🔧 Environment Variables per Deploy

```bash
ASPNETCORE_ENVIRONMENT=Production
Environment__UseInMemoryDatabase=false
Environment__SeedTestData=true
ConnectionStrings__DefaultConnection=Data Source=/app/data/monadicpilot.db
```

## 📊 Test dell'API

Dopo il deploy, testa gli endpoint:

```bash
# Get all customers
curl https://your-app.railway.app/api/customers

# Create customer
curl -X POST https://your-app.railway.app/api/customers \
  -H "Content-Type: application/json" \
  -d '{
    "companyName": "Test Corp",
    "email": "test@example.com",
    "vatNumber": "IT12345678901",
    "street": "Via Test 123",
    "city": "Milano",
    "postalCode": "20100",
    "country": "Italy"
  }'
```

## 💡 Suggerimenti per il Test

1. **Swagger UI**: Disponibile su `/swagger` in development
2. **Postman Collection**: Importa la collezione API
3. **Frontend Test**: Usa Vercel per il frontend Angular
4. **Monitoring**: Railway/Render forniscono logs gratuiti

## 🔄 CI/CD Gratuito

GitHub Actions per auto-deploy:
```yaml
name: Deploy to Railway
on:
  push:
    branches: [main]
jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2
      - uses: railway-deploy@v1
        with:
          service: your-service
          token: ${{ secrets.RAILWAY_TOKEN }}
```
