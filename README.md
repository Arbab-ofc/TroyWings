# TroyWingsApp — Premium Registration Page 

Modern, luxury registration UI for .NET 8 MVC (Razor) using Bootstrap 5.3 + Bootstrap Icons + Google Fonts. Includes SEO/OG/JSON-LD placeholders, glassmorphism, responsive layout, and client-side validation only (no backend persistence).

## 🔧 Tech Stack & Tools

![.NET 8](https://img.shields.io/badge/.NET%208-512BD4?logo=dotnet&logoColor=white&style=for-the-badge)
![ASP.NET MVC](https://img.shields.io/badge/ASP.NET%20MVC-512BD4?logo=dotnet&logoColor=white&style=for-the-badge)
![Bootstrap 5](https://img.shields.io/badge/Bootstrap%205-7952B3?logo=bootstrap&logoColor=white&style=for-the-badge)
![Bootstrap Icons](https://img.shields.io/badge/Bootstrap%20Icons-7952B3?logo=bootstrap&logoColor=white&style=for-the-badge)
![Google Fonts](https://img.shields.io/badge/Google%20Fonts-4285F4?logo=googlefonts&logoColor=white&style=for-the-badge)
![JavaScript](https://img.shields.io/badge/JavaScript-F7DF1E?logo=javascript&logoColor=111&style=for-the-badge)
![CSS3](https://img.shields.io/badge/CSS3-1572B6?logo=css3&logoColor=white&style=for-the-badge)

- Razor views only (no controllers/models changes)
- Bootstrap 5.3.3 (CDN) + Bootstrap Icons (CDN)
- Google Fonts (Playfair Display, Inter)
- Custom CSS/JS under `wwwroot` (gradients, glass cards, animations, focus/hover states)

## 📂 Project Layout

- `TroyWingsApp/Views/Home/Index.cshtml` — Razor view, head meta/SEO, JSON-LD, header brand, footer, left brand card, right registration form, Bootstrap validation hooks.
- `TroyWingsApp/wwwroot/css/registration.css` — Luxury theme (nav brand, hero cards, gradient background with noise/vignette, responsive grid, date picker/icon tinting, hover/focus states, reduced-motion support).
- `TroyWingsApp/wwwroot/js/registration.js` — Minimal script to show validation summary and apply Bootstrap `was-validated`.

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

## 🌐 SEO & Social

- Head includes: `<title>`, meta description, canonical placeholder, OG tags.
- JSON-LD `Organization` schema with placeholder URL/logo/address — replace with real values.

## 🚀 Run Locally

From `TroyWingsApp/`:

- `dotnet run` (or `dotnet watch run` for live reload)
- Open the printed URL (e.g., `http://localhost:5230/`).
- macOS HTTPS trust (optional): `dotnet dev-certs https --trust`, then rerun and use HTTPS URL.

## 🧪 Commands Cheat Sheet

- Scaffold (done already): `dotnet new mvc -n TroyWingsApp`
- Restore (if needed): `dotnet restore`
- Build: `dotnet build`
- Run: `dotnet run`
- Live reload: `dotnet watch run`
- Update workloads (if warned): `dotnet workload update`

## 🎛️ Customization

- Palette: tweak CSS variables in `wwwroot/css/registration.css` (`--accent`, `--bg-*`, etc.).
- Branding: update copy/icons, brand name, and links in `Index.cshtml`.
- SEO: replace canonical, OG tags, and JSON-LD placeholders with production values.
- Backend: wire the form to a controller/action for persistence; currently client-only.

## 🧩 Accessibility & Responsiveness

- Labels and inputs with clear focus rings; color contrast tuned for low-light palette.
- Responsive: side-by-side on desktop, stacked on mobile.
- Large tap targets on mobile; form first on small screens.

## 🛠️ Troubleshooting

- CSS not updating: hard refresh (`Cmd/Ctrl+Shift+R`).
- Dev cert warning: run `dotnet dev-certs https --trust` once.
- Workload warning: `dotnet workload update`.
