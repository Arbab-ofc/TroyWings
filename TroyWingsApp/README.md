# TroyWingsApp — Registration Experience

Premium registration UI for .NET 8 MVC (Razor) with Bootstrap 5.3, Bootstrap Icons, and Google Fonts. The backend now follows a service-layer architecture with a MySQL stored procedure (`sp_create_registration`) invoked via `MySqlConnector`.

## 🔧 Tech Stack & Tools

![.NET 8](https://img.shields.io/badge/.NET%208-512BD4?logo=dotnet&logoColor=white&style=for-the-badge)
![ASP.NET MVC](https://img.shields.io/badge/ASP.NET%20MVC-512BD4?logo=dotnet&logoColor=white&style=for-the-badge)
![Bootstrap 5](https://img.shields.io/badge/Bootstrap%205-7952B3?logo=bootstrap&logoColor=white&style=for-the-badge)
![Bootstrap Icons](https://img.shields.io/badge/Bootstrap%20Icons-7952B3?logo=bootstrap&logoColor=white&style=for-the-badge)
![Google Fonts](https://img.shields.io/badge/Google%20Fonts-4285F4?logo=googlefonts&logoColor=white&style=for-the-badge)
![JavaScript](https://img.shields.io/badge/JavaScript-F7DF1E?logo=javascript&logoColor=111&style=for-the-badge)
![CSS3](https://img.shields.io/badge/CSS3-1572B6?logo=css3&logoColor=white&style=for-the-badge)
![MySQL](https://img.shields.io/badge/MySQL-4479A1?logo=mysql&logoColor=white&style=for-the-badge)
![MySqlConnector](https://img.shields.io/badge/MySqlConnector-0D9D58?logo=mysql&logoColor=white&style=for-the-badge)

- Bootstrap 5.3.3 + Bootstrap Icons via CDN.
- Google Fonts (Playfair Display, Inter).
- Custom CSS/JS under `wwwroot` (gradients, glass cards, animations, validation behavior, success alert timeout).
- Data access is isolated in a repository; controllers talk to a service layer; persistence uses the stored procedure `sp_create_registration`.

## 📂 Project Layout

- `Views/Home/Index.cshtml` — Main Razor view with SEO/OG/JSON-LD, registration form, and hooks into shared partials and validation UI.
- `Views/Shared/_RegistrationHeader.cshtml` — Shared header/brand link.
- `Views/Shared/_RegistrationFooter.cshtml` — Shared footer links and brand block.
- `Views/Shared/_BrandShowcase.cshtml` — Left-side brand/story card.
- `wwwroot/css/registration.css` — Luxury theme (gradient/noise/vignette background, glass cards, accent palette, focus/hover states, motion + reduced-motion support).
- `wwwroot/js/registration.js` — Client-side validation summary toggle, Bootstrap `was-validated`, and 3-second auto-dismiss for success alerts.
- `Models/Registration.cs` — Form model with validation attributes.
- `Services/RegistrationService.cs` — Service layer that handles registration orchestration and delegates to the repository.
- `Data/MySqlRegistrationRepository.cs` — Repository using `MySqlConnector`, calling stored procedure `sp_create_registration`.
- `db/procedures/sp_create_registration.sql` — Stored procedure definition used for inserts.
- `Program.cs` — DI wiring for repository + service, routing, middleware.

## 🧭 UX & Layout Highlights

- Two-card layout: left brand story, right registration form.
- Premium background: deep navy/charcoal gradient with vignette plus subtle noise and glow accents.
- Cards: rounded, glassy, shadowed; hover lift and glow; accent typography.
- Animations: fade/slide on load; honors `prefers-reduced-motion`.
- Header/footer are shared partials for consistency across views.

## ✅ Form & Validation

- Fields: Name, Father’s Name, Date of Birth, Contact Number, Address (textarea).
- HTML5 + Bootstrap validation: `required`, `minlength/maxlength`, India phone pattern `^(\\+?91[- ]?)?[6-9]\\d{9}$`.
- Inline `.invalid-feedback` plus top summary alert `#validationSummary`.
- Success alert auto-hides after 3 seconds; validation summary stays hidden when the form is valid.
- Server-side validation via data annotations on `Registration`; controller delegates to the service layer, which calls the repository/stored procedure.
- Accessibility touches: descriptive labels, icons are decorative/contextual, reduced-motion support, high-contrast palette.

## 🌐 SEO & Social

- Head includes: `<title>`, meta description, canonical placeholder, OG tags.
- JSON-LD `Organization` schema with placeholder URL/logo/address — replace with production values.

## 🚀 Run Locally

From the project root:

- Restore/build: `dotnet restore` then `dotnet build` (first restore may need network access for NuGet).
- Run: `dotnet run` (or `dotnet watch run` for live reload).
- Open the printed URL (e.g., `http://localhost:5230/`).
- macOS HTTPS trust (optional): `dotnet dev-certs https --trust`, then rerun and use HTTPS URL.

### Database setup (MySQL + stored procedure)

1) Ensure MySQL is running and reachable at `127.0.0.1:3306`.
2) Connection string lives in `appsettings*.json` (example):
   ```json
   "ConnectionStrings": {
     "Default": "Server=127.0.0.1;Port=3306;Database=troywings_db;User=root;Password=Arbab@321123;"
   }
   ```
3) Create the `sp_create_registration` stored procedure before submitting the form:
   - In MySQL Workbench or CLI: `SOURCE /absolute/path/to/db/procedures/sp_create_registration.sql;`
4) Table definition expected by the procedure:
   ```sql
   CREATE TABLE IF NOT EXISTS Registrations (
       Id INT AUTO_INCREMENT PRIMARY KEY,
       Name VARCHAR(80) NOT NULL,
       FatherName VARCHAR(80) NOT NULL,
       DateOfBirth DATE NOT NULL,
       ContactNumber VARCHAR(14) NOT NULL,
       Address VARCHAR(180) NOT NULL,
       CreatedAtUtc DATETIME NOT NULL
   );
   ```
5) The repository uses parameterized calls to the stored procedure and logs unexpected row counts.

## 🧪 Commands Cheat Sheet

- Restore : `dotnet restore`
- Build: `dotnet build`
- Run: `dotnet run`
- Live reload: `dotnet watch run`
- Update workloads : `dotnet workload update`
- Lint/format (C#): `dotnet format` (optional)
- Publish (example): `dotnet publish -c Release -o ./publish`

## 🎛️ Customization

- Palette: adjust CSS variables in `wwwroot/css/registration.css` (`--accent`, `--bg-*`, etc.).
- Branding: update copy/icons and links in the shared partials or main view.
- SEO: swap canonical, OG tags, and JSON-LD placeholders with production values.
- Persistence: connection string in `appsettings*.json`; repository uses `MySqlConnector` + stored procedure for inserts.
- Secrets: move DB credentials into user secrets or environment variables for non-dev usage.

## 🧩 Accessibility & Responsiveness

- Labels and inputs with clear focus rings; contrast tuned for the low-light palette.
- Responsive layout: side-by-side on desktop, stacked on mobile; header/footer remain available.
- Touch-friendly spacing on small screens.
- Reduced motion: key animations disabled when `prefers-reduced-motion` is set.
- Form feedback: validation summary and invalid-feedback text announced by screen readers when focused.

## 🛠️ Troubleshooting

- CSS/JS not updating: hard refresh (`Cmd/Ctrl+Shift+R`).
- Dev cert warning: run `dotnet dev-certs https --trust` once.
- NuGet restore blocked: ensure network access or pre-restore packages.
- MySQL connection failures: verify host/port/user/password in `appsettings*.json`; ensure `sp_create_registration` exists in `troywings_db`.
- 502/404 while running: check console output for the actual listening URL and verify SSL dev cert trust if using HTTPS.
