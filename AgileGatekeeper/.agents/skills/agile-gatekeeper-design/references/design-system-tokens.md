# Cobalt Precision & Precision Enterprise SaaS — Design System Tokens

This reference defines the exact visual tokens, typographic scale, elevation levels, and layout metrics for **AgileGatekeeper**.

---

## 1. Color Palette Tokens

| Role | Token / Usage | Light Mode Hex / Class | Dark Mode Hex / Class |
| :--- | :--- | :--- | :--- |
| **Canvas Base** | Main app background | `#f8fafc` (`bg-slate-50/50`) | `#090d16` (`dark:bg-[#090d16]`) |
| **Sidebar Surface** | Vertical navigation dock | `#ffffff` (`bg-white`) | `#0c1322` (`dark:bg-[#0c1322]`) |
| **Card Surface (Level 1)** | Primary modules & tables | `#ffffff` (`bg-white`) | `#0f172a` (`dark:bg-[#0f172a]`) |
| **Muted Surface** | Nested pods & inputs | `#f1f5f9` (`bg-slate-100`) | `#131c31` (`dark:bg-slate-800/60`) |
| **Subtle Border** | Card & divider outlines | `#e2e8f0` (`border-slate-200/80`) | `#1e293b` (`dark:border-slate-800`) |
| **Primary Brand** | CTAs, active nav pills | `#2563eb` (`bg-blue-600`) | `#2563eb` (`bg-blue-600`) |
| **Primary Hover** | Button hover states | `#1d4ed8` (`hover:bg-blue-700`) | `#1d4ed8` (`hover:bg-blue-700`) |
| **Primary Soft** | Icon pods & active badges | `#eff6ff` (`bg-blue-50`) | `rgba(23,37,84,0.4)` (`dark:bg-blue-950/40`) |
| **Secondary Accent** | Analytics & multi-series | `#6366f1` (`text-indigo-500`) | `#818cf8` (`dark:text-indigo-400`) |
| **Tertiary Accent** | Live telemetry & sparklines | `#06b6d4` (`text-cyan-500`) | `#22d3ee` (`dark:text-cyan-400`) |
| **Text Primary** | Headings & primary data | `#0f172a` (`text-slate-900`) | `#f8fafc` (`dark:text-white`) |
| **Text Secondary** | Body copy & descriptions | `#475569` (`text-slate-600`) | `#94a3b8` (`dark:text-slate-400`) |
| **Text Muted** | Captions, metadata, hints | `#94a3b8` (`text-slate-400`) | `#64748b` (`dark:text-slate-500`) |
| **Semantic Success** | Completed, online, positive | `#10b981` (`emerald-500/600`) | `#34d399` (`dark:text-emerald-400`) |
| **Semantic Warning** | In transit, pending, alert | `#f59e0b` (`amber-500/600`) | `#fbbf24` (`dark:text-amber-400`) |
| **Semantic Error** | Failed, rejected, danger | `#ef4444` (`red-500/600`) | `#f87171` (`dark:text-red-400`) |

---

## 2. Typography Scale

- **Display / Headlines (`font-display` -> `Plus Jakarta Sans`)**:
  - `headline-xl`: `text-3xl sm:text-4xl font-bold tracking-tight leading-tight`
  - `headline-lg`: `text-2xl font-semibold tracking-tight`
  - `headline-md`: `text-xl font-bold tracking-tight`
  - `headline-sm`: `text-base font-semibold tracking-tight`
  - `numeric-stat`: `font-display text-2xl sm:text-3xl font-bold tracking-tight tabular-nums`
- **Body & UI Controls (`font-sans` -> `Inter`)**:
  - `body-md`: `text-[13.5px] leading-relaxed font-normal`
  - `body-sm`: `text-xs leading-normal font-normal`
  - `label-lg`: `text-[13px] font-semibold tracking-tight`
  - `label-md`: `text-xs font-semibold tracking-wide`
  - `label-sm`: `text-[11px] font-semibold uppercase tracking-wider`

---

## 3. Elevation & Shadows

Configured in `App.razor` Tailwind theme:
- **`shadow-subtle`**: `0 1px 2px 0 rgba(0, 0, 0, 0.03), 0 1px 6px -1px rgba(0, 0, 0, 0.02)` — Default for all cards, panels, and table wrappers.
- **`shadow-modal`**: `0 20px 25px -5px rgba(0, 0, 0, 0.04), 0 8px 10px -6px rgba(0, 0, 0, 0.03)` — Used for modals, dialogs, and floating popovers.
- **`shadow-spotlight`**: `0 0 0 1px rgba(37, 99, 235, 0.15), 0 8px 24px -4px rgba(0, 0, 0, 0.06)` — Used for highlighted/selected interactive cards.
- **Primary CTA Glow**: `style="box-shadow: rgba(37, 99, 235, 0.35) 0px 4px 14px -1px;"` — Used on primary blue pill buttons and active sidebar items.

---

## 4. Layout & Spatial Rhythm

- **Desktop (≥ 1280px)**: Persistent sidebar (`w-64` expanded / `w-[72px]` collapsed) + fluid main content area with `p-8` padding and `gap-6` (`1.5rem`) between vertical sections.
- **Tablet & Mobile (< 768px)**: Off-canvas slide-out drawer with `bg-slate-900/40 backdrop-blur-xs` overlay, `p-4 sm:p-6` main canvas padding, and stacked 1-to-2 column grids (`grid-cols-1 sm:grid-cols-2 lg:grid-cols-4`).
