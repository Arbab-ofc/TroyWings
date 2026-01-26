# TroyWingsApp — Registration + Users Directory

Premium registration UI and a luxury Users directory for .NET 8 MVC (Razor) with Bootstrap 5.3, Bootstrap Icons, and Google Fonts. The app includes SEO/OG/JSON-LD placeholders, glassmorphism styling, responsive layout, AJAX-powered pagination/editing, and MongoDB persistence via `MongoDB.Driver`.

## 🔧 Tech Stack & Tools

![.NET 8](https://img.shields.io/badge/.NET%208-512BD4?logo=dotnet&logoColor=white&style=for-the-badge)
![ASP.NET MVC](https://img.shields.io/badge/ASP.NET%20MVC-512BD4?logo=dotnet&logoColor=white&style=for-the-badge)
![Bootstrap 5](https://img.shields.io/badge/Bootstrap%205-7952B3?logo=bootstrap&logoColor=white&style=for-the-badge)
![Bootstrap Icons](https://img.shields.io/badge/Bootstrap%20Icons-7952B3?logo=bootstrap&logoColor=white&style=for-the-badge)
![Google Fonts](https://img.shields.io/badge/Google%20Fonts-4285F4?logo=googlefonts&logoColor=white&style=for-the-badge)
![jQuery](https://img.shields.io/badge/jQuery-0769AD?logo=jquery&logoColor=white&style=for-the-badge)
![CSS3](https://img.shields.io/badge/CSS3-1572B6?logo=css3&logoColor=white&style=for-the-badge)
![MongoDB](https://img.shields.io/badge/MongoDB-47A248?logo=mongodb&logoColor=white&style=for-the-badge)
![MongoDB.Driver](https://img.shields.io/badge/MongoDB.Driver-47A248?logo=mongodb&logoColor=white&style=for-the-badge)

- Bootstrap 5.3.3 + Bootstrap Icons via CDN.
- Google Fonts (Playfair Display, Inter).
- Custom CSS/JS under `wwwroot` (gradients, glass cards, animations, validation behavior, success alert timeout, jQuery-powered users UI).
- MongoDB persistence implemented with `MongoDB.Driver` (no EF Core runtime dependency).

## 📂 Project Layout

- `TroyWingsApp/Views/Home/Index.cshtml` — Registration view with SEO/OG/JSON-LD and validation UI.
- `TroyWingsApp/Views/Users/Index.cshtml` — Users directory view with AJAX paging and edit modal.
- `TroyWingsApp/Views/Shared/_RegistrationLayout.cshtml` — Shared layout with header/nav and footer.
- `TroyWingsApp/Views/Shared/_Layout.cshtml` — Base layout that loads jQuery before per-page scripts.
- `TroyWingsApp/Views/Shared/_BrandShowcase.cshtml` — Left-side brand/story card.
- `TroyWingsApp/wwwroot/css/registration.css` — Luxury theme (gradient/noise/vignette background, glass cards, accent palette, focus/hover states, motion + reduced-motion support).
- `TroyWingsApp/wwwroot/css/users.css` — Users page styling and responsive grid.
- `TroyWingsApp/wwwroot/js/registration.js` — jQuery-based validation summary toggle, Bootstrap `was-validated`, and 3-second auto-dismiss for success alerts.
- `TroyWingsApp/wwwroot/js/users.js` — jQuery AJAX paging + edit modal handling.
- `TroyWingsApp/wwwroot/js/site.js` — jQuery-powered mobile header/hamburger behavior.
- `TroyWingsApp/Models/Registration.cs` — Form model with validation attributes.
- `TroyWingsApp/Models/UpdateRegistrationRequest.cs` — DTO for edit modal updates.
- `TroyWingsApp/Models/PagedResult.cs` — Pagination envelope.
- `TroyWingsApp/Data/MongoRegistrationRepository.cs` — Repository using `MongoDB.Driver` to insert, list, and update registrations.
- `TroyWingsApp/Controllers/HomeController.cs` — Receives form posts, validates, and stores via repository.
- `TroyWingsApp/Controllers/UsersController.cs` — Users list JSON endpoint and update endpoint.
- `TroyWingsApp/Program.cs` — Service registration for repository, DB/table bootstrap on startup, routing, middleware.

## 🧭 UX & Layout Highlights

- Two-card layout: left brand story, right registration form.
- Premium background: deep navy/charcoal gradient with vignette plus subtle noise and glow accents.
- Cards: rounded, glassy, shadowed; hover lift and glow; accent typography.
- Animations: fade/slide on load; honors `prefers-reduced-motion`.
- Header/footer are shared in a single layout for consistency across views.

## ✅ Form & Validation

- Fields: Name, Father’s Name, Date of Birth, Contact Number, Address (textarea).
- HTML5 + Bootstrap validation: `required`, `minlength/maxlength`, India phone pattern `^(\+?91[- ]?)?[6-9]\d{9}$`.
- Inline `.invalid-feedback` plus top summary alert `#validationSummary`.
- Success alert auto-hides after 3 seconds; validation summary stays hidden when the form is valid.
- Server-side validation via data annotations on `Registration`; repository persists on success.
- Error handling: TempData surfaces success/error banners; invalid submissions redisplay entered data with validation messages.
- Accessibility touches: descriptive labels, icons are decorative/contextual, reduced-motion support, high-contrast palette.

## 🌐 SEO & Social

- Head includes: `<title>`, meta description, canonical placeholder, OG tags.
- JSON-LD `Organization` schema with placeholder URL/logo/address — replace with production values.

## 🚀 Run Locally

From `TroyWingsApp/`:

- Restore/build: `dotnet restore` then `dotnet build` (first restore may need network access for NuGet).
- Run: `dotnet run` (or `dotnet watch run` for live reload).
- Open the printed URL (e.g., `http://localhost:5230/`).
- macOS HTTPS trust (optional): `dotnet dev-certs https --trust`, then rerun and use HTTPS URL.

### Database setup (MongoDB)

1) Ensure MongoDB is running and reachable at `mongodb://localhost:27017/`.
2) Connection settings live in `appsettings*.json` (example):
   ```json
   "Mongo": {
     "ConnectionString": "mongodb://localhost:27017/",
     "Database": "Troywings"
   }
   ```
3) Collections are created automatically on first use (`Registrations` and `Counters`).
4) The repository uses an auto-increment counter document so the app can keep numeric `Id` values.

## 🧪 Commands Cheat Sheet

- Restore : `dotnet restore`
- Build: `dotnet build`
- Run: `dotnet run`
- Live reload: `dotnet watch run`
- Update workloads : `dotnet workload update`
- Lint/format (C#): `dotnet format` (optional, not required for this project)
- Publish (example): `dotnet publish -c Release -o ./publish`

## 🎛️ Customization

- Palette: adjust CSS variables in `wwwroot/css/registration.css` (`--accent`, `--bg-*`, etc.).
- Branding: update copy/icons and links in the shared partials or main view.
- SEO: swap canonical, OG tags, and JSON-LD placeholders with production values.
- Persistence: repository uses `MongoDB.Driver`; update the Mongo connection settings for your environment and extend the document mapping if you add fields.
- Secrets: move DB credentials into user secrets or environment variables for non-dev usage.

## 👥 Users Directory

- Route: `/Users` (Razor view), `/Users/List` (JSON), `/Users/Update` (JSON).
- Pagination: desktop/tablet shows 4 cards per page (2x2); mobile shows 2 per page.
- Editing: "Edit" button on each card opens a modal and saves via jQuery AJAX.

## 🧩 Accessibility & Responsiveness

- Labels and inputs with clear focus rings; contrast tuned for the low-light palette.
- Responsive layout: side-by-side on desktop, stacked on mobile; header/footer remain available.
- Touch-friendly spacing on small screens.
- Reduced motion: all key animations disabled when `prefers-reduced-motion` is set.
- Form feedback: validation summary and invalid-feedback text announced by screen readers when focused.

## 🛠️ Troubleshooting

- CSS/JS not updating: hard refresh (`Cmd/Ctrl+Shift+R`).
- Dev cert warning: run `dotnet dev-certs https --trust` once.
- NuGet restore blocked: ensure network access or pre-restore packages.
- MongoDB connection failures: verify the connection string and database name in `appsettings*.json`; confirm the MongoDB service is running.
- 502/404 while running: check console output for the actual listening URL and verify SSL dev cert trust if using HTTPS.
