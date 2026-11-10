---
name: agile-gatekeeper-design
description: >-
  Use this skill whenever creating, designing, styling, or modifying Blazor (.NET 10) UI
  pages, modules, forms, tables, cards, modals, or components in the AgileGatekeeper
  project. Enforces 100% responsive mobile-first design, the Cobalt Precision & Precision
  Enterprise SaaS design system, Tailwind CSS tokens, Dark/Light mode support, and
  feature-folder component architecture.
---

# AgileGatekeeper UI/UX Design & Component Architecture Skill

This skill guides the creation of modern, high-craft enterprise interfaces in **AgileGatekeeper** using **Blazor (.NET 10 InteractiveServer)** and **Tailwind CSS**, following the **Cobalt Precision / Precision Enterprise SaaS** aesthetic and enforcing **100% responsiveness across all viewports**.

---

## 1. Regla Mandatoria: Diseño 100% Responsivo (Mobile-First & Fluid Breakpoints)

**TODO componente, página, tabla, modal, formulario y elemento de navegación DEBE ser 100% responsivo.** Está estrictamente prohibido generar interfaces que causen desbordamiento horizontal (*overflow*), textos truncados de forma defectuosa o botones inaccesibles en dispositivos móviles o pantallas táctiles.

### Reglas Específicas de Responsividad:
1. **Shell y Navegación Adaptativa**:
   - **Escritorio (`md:` / `≥ 768px`)**: Sidebar permanente con ancho dinámico (`w-64` expandido / `w-[72px]` colapsado).
   - **Móvil y Tablet (`< 768px`)**: Sidebar en modo cajón deslizable (*off-canvas drawer*) con clase `-translate-x-full md:translate-x-0`, acompañado de un fondo desenfocado oscuro (`fixed inset-0 z-30 bg-slate-900/40 backdrop-blur-xs md:hidden`) que se cierra al hacer clic fuera o pulsar el botón de cerrar.
   - **Cabecera (`Header.razor`)**: Botón hamburguesa visible exclusivamente en móviles (`md:hidden`), textos de saludo compactados (`text-[18px] sm:text-[22px]`) y botones de acción que colapsan texto largo mostrando íconos en pantallas pequeñas (`hidden sm:inline`).

2. **Grillas y Contenedores Fluidos**:
   - **Tarjetas de Métricas / KPIs**: Nunca uses columnas fijas. Emplea grillas escalonadas:
     `grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4`.
   - **Paneles Principales y Dashboards**: Distribución fluida de una a doce columnas:
     `grid grid-cols-1 lg:grid-cols-12 gap-6`.
   - **Espaciado del Canvas**: Acolchado exterior adaptativo: `p-4 sm:p-6 md:p-8`.

3. **Tablas de Datos Empresariales Responsivas**:
   - Toda tabla **DEBE** estar envuelta en un contenedor con desplazamiento horizontal fluido:
     `<div class="overflow-x-auto w-full">`.
   - Oculta columnas auxiliares o metadatos secundarios en pantallas reducidas usando utilidades como `hidden sm:table-cell` o `hidden lg:table-cell`.
   - Las barras de herramientas de tablas (búsqueda, filtros, acciones) deben apilarse en móvil y alinearse en fila en escritorio:
     `flex flex-col sm:flex-row sm:items-center justify-between gap-3`.

4. **Formularios y Diálogos Modales**:
   - Los campos de entrada (`InputText`, `InputSelect`, botones de acción) deben ocupar el ancho completo en móvil (`w-full sm:w-auto`).
   - Los diálogos modales deben tener un margen de seguridad en móviles:
     `w-full max-w-lg mx-4 max-h-[90vh] overflow-y-auto rounded-2xl p-4 sm:p-6`.
   - **Objetivos Táctiles (Touch Targets)**: Altura mínima de interacción de `40px` a `44px` (`h-10` a `h-11`) para facilitar clics precisos en dispositivos táctiles.

---

## 2. Architectural Placement Rules (Feature Folders)

Every `.razor` component, model, and service MUST be placed in its corresponding domain folder:

- **Layout & Navigation (`Components/Layout/Menu/`)**:
  - `Sidebar.razor`, `SidebarNavItem.razor`, `Header.razor`, `UserProfileWidget.razor`.
- **Shared Reusable UI (`Components/Shared/`)**:
  - Parameterized, reusable UI building blocks (`WorkspacePlaceholder.razor`, cards, modals, badges, data tables).
  - Always use `[Parameter]` and `RenderFragment? ChildContent` to avoid duplicating HTML across pages.
- **Pages by Feature (`Components/Pages/`)**:
  - `Login/` -> Authentication views (`Login.razor` using `@layout LoginLayout`).
  - `Menu/` -> Main dashboard and landing views (`Home.razor`).
  - `Modulos/` -> Core operational modules (`Orders.razor`, `Shipments.razor`, `MapOverview.razor`, `Settings.razor`, etc.).
  - `Notificaciones/` -> Messaging and notification centers (`Messages.razor`, `Notifications.razor`).
  - `Sistema/` -> System-level pages (`Error.razor`, `NotFound.razor`).
- **Models & Services**:
  - Group into domain subfolders (`Models/Auth/`, `Models/Users/`, `Services/Auth/`, `Services/Theme/`).
  - Always implement `IDisposable` in components that subscribe to service or navigation events.

---

## 3. Core Visual Identity (Cobalt Precision)

When generating any UI element, adhere strictly to these visual pillars:

1. **Color Hierarchy & Dark Mode Parity**:
   - Every surface, border, and text class **MUST** include both Light Mode and Dark Mode (`dark:*`) Tailwind variants.
   - **Canvas Background**: `bg-[var(--canvas-bg)]` (`#f8fafc` light / `#090d16` dark).
   - **Elevated Cards & Panels**: `bg-white dark:bg-[#0f172a]` with `border border-slate-200/80 dark:border-slate-800` and `shadow-subtle`.
   - **Nested / Secondary Surfaces**: `bg-slate-50 dark:bg-slate-800/50` or `bg-slate-100/80 dark:bg-slate-800`.
   - **Primary Brand Accent**: Cobalt Royal Blue (`bg-blue-600 hover:bg-blue-700 text-white`), soft highlight fills (`bg-blue-50 dark:bg-blue-950/40 text-blue-600 dark:text-blue-400`).
   - **Semantic Accents**:
     - Success: `bg-emerald-50 text-emerald-600 dark:bg-emerald-950/40 dark:text-emerald-400`
     - Warning: `bg-amber-50 text-amber-600 dark:bg-amber-950/40 dark:text-amber-400`
     - Error: `bg-red-50 text-red-600 dark:bg-red-950/40 dark:text-red-400`

2. **Typography**:
   - **Headings & KPI Readouts**: Use `font-display` (`Plus Jakarta Sans`), `font-bold`, `tracking-tight`, `text-slate-900 dark:text-white`. For numeric KPIs, add `tabular-nums`.
   - **Body & Controls**: Use `font-sans` (`Inter`), `text-[13px]` or `text-sm`, `text-slate-600 dark:text-slate-400`.
   - **Micro-labels & Table Headers**: `text-[11.5px]` or `text-xs font-semibold uppercase tracking-wider text-slate-400 dark:text-slate-500`.

3. **Shapes, Spacing & Elevation**:
   - **Primary Cards & Containers**: `rounded-2xl` (`1rem`–`1.5rem`) with `p-4 sm:p-5 md:p-6` internal padding and `shadow-subtle`.
   - **Buttons & Inputs**: Pill-shaped (`rounded-full`) for primary CTAs and login controls, or `rounded-xl` (`0.75rem`) for dense module forms and toolbars.
   - **Status Badges**: Always full pills (`rounded-full px-2.5 py-1 text-xs font-medium`) with an optional `w-1.5 h-1.5 rounded-full` status dot.

4. **Iconography**:
   - Use **Google Material Symbols Outlined**:
     `<span class="material-symbols-outlined text-[19px]">icon_name</span>`
   - Add the `.filled` class when an item or state is active:
     `<span class="material-symbols-outlined text-[19px] filled">icon_name</span>`

---

## 4. Reference Documentation (Progressive Disclosure)

Read these reference files when designing detailed views or components:

- **[Design System Tokens & Specifications](./references/design-system-tokens.md)**: Complete color tokens, typographic scale, responsive grid breakpoints, and elevation levels for *Cobalt Precision* and *Precision Enterprise SaaS*.
- **[Blazor Component Catalog & Templates](./references/blazor-component-catalog.md)**: Copy-ready Blazor + Tailwind templates for KPI metric cards, enterprise data tables, EditForm inputs, modals, segmented tabs, and header toolbars.

---

## 5. Verification Workflow

After creating or modifying any component or page:
1. **Responsive Check**: Verify that layouts adapt across Mobile (`320px–639px`), Tablet (`640px–1023px`), and Desktop (`≥ 1024px`) without causing horizontal viewport overflow.
2. Ensure any new folder namespace is registered in `Components/_Imports.razor` if used globally.
3. Run `dotnet build` to verify there are **0 errors and 0 warnings**.
4. Confirm no `.razor.css` isolation files are created unless strictly necessary (prefer Tailwind utilities and `wwwroot/app.css` tokens).
