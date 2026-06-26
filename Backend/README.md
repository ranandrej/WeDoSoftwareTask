# Backend

.NET 8 Web API za TrainingApp — auth, workout CRUD, mesečni progress i PDF export.

## Šta je korišćeno

- **ASP.NET Core 8** — REST API, JWT auth, rate limiting na login/register
- **Entity Framework Core** + **PostgreSQL** — baza i migracije
- **Swagger** — testiranje endpointa
- **QuestPDF** — generisanje monthly progress PDF-a
- **xUnit + Moq + FluentAssertions** — unit testovi servisa

## Struktura (Clean Architecture)

Projekat je podeljen po slojevima. Zavisnosti idu ka unutra — `Domain` ne zna ni za šta drugo.

```
API              → HTTP, kontroleri, middleware
Application      → biznis logika, servisi, DTO-ovi, interfejsi
Infrastructure   → EF Core, repozitorijumi, JWT, PDF
Domain           → entiteti (User, Workout), enumi
Test             → testovi Application sloja
```

**Kako teče request:**

1. Kontroler primi HTTP zahtev (`WorkoutsController`, `AuthController`...)
2. Pozove servis iz `Application` (npr. `WorkoutService`)
3. Servis validira podatke, radi logiku, zove repozitorijum preko interfejsa (`IWorkoutRepository`)
4. `Infrastructure` implementira te interfejse — upis u bazu, generisanje tokena itd.
5. Servis vraća `Result<T>` — kontroler mapira na `Ok` / `BadRequest` / `Unauthorized`

DI je u `Application/DependencyInjection` i `Infrastructure/DependencyInjection`, registracija u `API/Program.cs`.

## Pokretanje

Iz root-a repoa (Docker):

```bash
docker compose up --build db api
```

Ručno:

```bash
cd Backend/API
dotnet run
```

Swagger: http://localhost:5000/swagger (Docker) ili http://localhost:5048/swagger (lokalno)

Migracije se primenjuju automatski pri startu.

## Testovi

```bash
cd Backend/Test
dotnet test
```
