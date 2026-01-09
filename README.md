# TroyWingsApp — Premium Registration Page

Modern, luxury registration UI for .NET 8 MVC (Razor) using Bootstrap 5.3 + Bootstrap Icons + Google Fonts. Includes SEO/OG/JSON-LD placeholders, glassmorphism, responsive layout, client + server validation, and MySQL persistence.

## 🔧 Tech Stack & Tools

![.NET 8](https://img.shields.io/badge/.NET%208-512BD4?logo=dotnet&logoColor=white&style=for-the-badge)
![ASP.NET MVC](https://img.shields.io/badge/ASP.NET%20MVC-512BD4?logo=dotnet&logoColor=white&style=for-the-badge)
![Bootstrap 5](https://img.shields.io/badge/Bootstrap%205-7952B3?logo=bootstrap&logoColor=white&style=for-the-badge)
![Bootstrap Icons](https://img.shields.io/badge/Bootstrap%20Icons-7952B3?logo=bootstrap&logoColor=white&style=for-the-badge)
![Google Fonts](https://img.shields.io/badge/Google%20Fonts-4285F4?logo=googlefonts&logoColor=white&style=for-the-badge)
![JavaScript](https://img.shields.io/badge/JavaScript-F7DF1E?logo=javascript&logoColor=111&style=for-the-badge)
![CSS3](https://img.shields.io/badge/CSS3-1572B6?logo=css3&logoColor=white&style=for-the-badge)
![MySQL](https://img.shields.io/badge/MySQL-4479A1?logo=mysql&logoColor=white&style=for-the-badge)
![EF Core](https://img.shields.io/badge/EF%20Core-512BD4?logo=dotnet&logoColor=white&style=for-the-badge)

- Bootstrap 5.3.3 (CDN) + Bootstrap Icons (CDN)
- Google Fonts (Playfair Display, Inter)
- Custom CSS/JS under `wwwroot` (gradients, glass cards, animations, focus/hover states)
- EF Core + Pomelo MySQL provider for persistence

## 📂 Project Layout

- `TroyWingsApp/Views/Home/Index.cshtml` — Razor view, head meta/SEO, JSON-LD, header brand, footer, left brand card, right registration form, Bootstrap validation hooks.
- `TroyWingsApp/wwwroot/css/registration.css` — Luxury theme (nav brand, hero cards, gradient background with noise/vignette, responsive grid, date picker/icon tinting, hover/focus states, reduced-motion support).
- `TroyWingsApp/wwwroot/js/registration.js` — Minimal script to show validation summary and apply Bootstrap `was-validated`.
- `TroyWingsApp/Models/Registration.cs` — Data model with annotations.
- `TroyWingsApp/Data/ApplicationDbContext.cs` — EF Core DbContext (MySQL).
- `TroyWingsApp/Program.cs` — Service registration for MySQL + EnsureCreated.

## 🧭 UX & Layout Highlights

- Two-card layout: left brand story, right registration form.
- Premium background: deep navy/charcoal gradient with vignette + subtle noise and glow accents.
- Cards: rounded, glassy, shadow, hover glow; input focus glow; button hover lift.
- Animations: fade/slide on load, micro-interactions; honors `prefers-reduced-motion`.
- Header: simple brand-only link back to home. Footer anchored to bottom via flex.

## ✅ Form & Validation

- Fields: Name, Father’s Name, Date of Birth, Contact Number, Address (textarea).
- HTML5 + Bootstrap validation: `required`, `minlength/maxlength`, India phone pattern `^(\\+?91[- ]?)?[6-9]\\d{9}$`.
- Inline `.invalid-feedback` and top summary alert `#validationSummary`.
- Helpers and placeholders styled for contrast; date input and picker icon tinted.
- Server-side validation via data annotations on `Registration` model.

## 🌐 SEO & Social

- Head includes: `<title>`, meta description, canonical placeholder, OG tags.
- JSON-LD `Organization` schema with placeholder URL/logo/address — replace with real values.

## 🚀 Run Locally

From `TroyWingsApp/`:

- `dotnet run` (or `dotnet watch run` for live reload)
- Open the printed URL (e.g., `http://localhost:5230/`).
- macOS HTTPS trust (optional): `dotnet dev-certs https --trust`, then rerun and use HTTPS URL.

### Database setup (MySQL)

1) Ensure MySQL is running and reachable at `127.0.0.1:3306`.
2) Create the database (example):
   ```sql
   CREATE DATABASE troywings_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
   ```
3) Connection string (set in `appsettings*.json`):
   ```json
   "ConnectionStrings": {
     "Default": "Server=127.0.0.1;Port=3306;Database=troywings_db;User=root;Password=Arbab@321123;"
   }
   ```
4) On startup the app will `EnsureCreated()` the schema if it does not exist. For migrations-based flow, add a migration and run `dotnet ef database update` (requires the `dotnet-ef` tool).

## 🧪 Commands Cheat Sheet

- Scaffold : `dotnet new mvc -n TroyWingsApp`
- Restore : `dotnet restore`
- Build: `dotnet build`
- Run: `dotnet run`
- Live reload: `dotnet watch run`
- Update workloads : `dotnet workload update`

## 🎛️ Customization

- Palette: tweak CSS variables in `wwwroot/css/registration.css` (`--accent`, `--bg-*`, etc.).
- Branding: update copy/icons, brand name, and links in `Index.cshtml`.
- SEO: replace canonical, OG tags, and JSON-LD placeholders with production values.
- Backend: adjust or extend the controller/action as needed; persistence is wired to MySQL via EF Core.
- Secrets: move DB credentials into user secrets or environment variables for production; current settings are for local dev only.

## 🧩 Accessibility & Responsiveness

- Labels and inputs with clear focus rings; color contrast tuned for low-light palette.
- Responsive: side-by-side on desktop, stacked on mobile.
- Large tap targets on mobile; form first on small screens.

## 🛠️ Troubleshooting

- CSS not updating: hard refresh (`Cmd/Ctrl+Shift+R`).
- Dev cert warning: run `dotnet dev-certs https --trust` once.
- Workload warning: `dotnet workload update`.
