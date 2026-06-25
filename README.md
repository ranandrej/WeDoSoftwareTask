# TrainingTracker

Aplikacija za praćenje treninga (Angular + .NET + PostgreSQL).

## Opcija 1 — Docker (preporučeno)

**Potrebno:**
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

**Pokretanje** (iz root foldera repoa):

```bash
docker compose up --build
```

**URL-ovi:**
- Frontend: http://localhost:4200
- Backend (Swagger): http://localhost:5000/swagger
- PostgreSQL: `localhost:5432` (user: `postgres`, pass: `pass123`, baza: `training-app`)

Prvi put registruj novog korisnika preko frontend-a.

---

## Opcija 2 — Ručno (bez Dockera)

**Potrebno:**
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 22+](https://nodejs.org/)
- [PostgreSQL 16](https://www.postgresql.org/download/)

**1. Baza**

Pokreni PostgreSQL i kreiraj bazu `training-app`.  
Connection string u `Backend/API/appsettings.Development.json`:

```
Host=localhost;Port=5432;Database=training-app;Username=postgres;Password=pass123
```

Prilagodi user/password ako kod tebe nisu isti.

**2. Backend**

```bash
cd Backend/API
dotnet run
```

API: http://localhost:5048  
Swagger: http://localhost:5048/swagger  
(Migracije se primenjuju automatski pri startu.)

**3. Frontend**

U `Frontend/src/app/environments/enviroment.development.ts` podesi:

```typescript
apiUrl: 'http://localhost:5048/api'
```

Zatim:

```bash
cd Frontend
npm install
npm start
```

Frontend: http://localhost:4200
