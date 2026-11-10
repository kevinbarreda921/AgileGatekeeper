---
name: signature-minimal-ui
description: Design system rules and constraints to build unique, tactile, high-craft, and non-generic minimal modern web interfaces.
triggers:
  - "signature UI"
  - "modern minimal UI"
  - "unique design"
  - "clean dashboard"
  - "editorial interface"
---

# Signature Minimal UI — High-Craft Interface Specification

Your objective is to build interfaces with a distinct visual identity, high typographic contrast, and deliberate tactile restraint. Under no circumstances produce generic, interchangeable AI/bootstrap layouts.

---

## 1. Explicit Prohibitions (Banned Patterns)

- **NO soft blur drop-shadows:** Forbid `shadow-lg`, `shadow-xl`, or blurred floating cards.
- **NO crypto/SaaS rainbow gradients:** No arbitrary purple/pink/cyan radial glows or multi-stop gradient headings.
- **NO identical 3/4-card grids:** Do not default to equal-width cards side by side. Break symmetry using calculated modular scales (60/40, 70/30, or dense informational panels paired with open negative space).
- **NO default typographic indifference:** Never output body text and headings in generic weights without tracking discipline.
- **NO cartoonish rounding:** Avoid pill-shaped card containers or conflicting corner radii. All primary structural elements must share a strict radius system (recommended: `rounded-md` [6px] or `rounded-lg` [8px]).
- **NO generic placeholders:** Never use "Lorem ipsum", placeholder avatars with smiling stock faces, or random icon sets. Content must read as realistic domain data.

---

## 2. Color System: 60-30-10 Strict Rule

Every surface and token must follow a disciplined 3-tier hierarchy:

1. **60% Dominant Base (Neutral with Tone):**
   - *Dark mode:* Deep graphite, slate, or warm obsidian (e.g., `#09090b`, `#0c0d0e`). Avoid pure dead black `#000000` unless strictly required for high-contrast OLED panels.
   - *Light mode:* Warm parchment, architectural paper, or pale zinc (e.g., `#fbfbfa`, `#f4f4f5`). Avoid eye-straining clinical `#ffffff` across entire screens.
2. **30% Structural Secondary:**
   - Subtle surface elevation, toolbars, sidebars, and hairline borders.
   - Separate elements using **1px hairline borders** (`border-black/10` or `border-white/10`) or subtle matte surface changes, never drop shadows.
3. **10% Signature Accent (Single Accent Only):**
   - Pick **ONE** energetic brand accent per view:
     - *Industrial Amber* (`#f59e0b` / `#d97706`)
     - *Phosphor Green* (`#10b981` / `#059669`)
     - *Electric Cobalt* (`#2563eb` / `#1d4ed8`)
     - *International Orange* (`#ea580c`)
   - Accent applies strictly to active states, primary CTAs, critical status pips, and deliberate typographic highlights.

---

## 3. Editorial Typography & Layout

- **Extreme Optical Contrast:**
  - **Headings & Display:** Tight tracking (`tracking-tight` or `letter-spacing: -0.03em`), heavier optical weight (`font-semibold` or `font-bold`), clamp-based fluid sizes (`clamp(2rem, 4vw, 3.75rem)`).
  - **Micro-labels & Metadata:** Monospace or clean grotesque in uppercase, reduced font size (`text-[10px]` to `text-xs`), and expanded tracking (`tracking-widest` or `0.08em`).
- **Density Modulation:**
  - Alternate between **high-density data strips** (compact metrics, tight tabular data, monospace figures) and **generous negative space** (uncluttered whitespace around primary decision points).
- **Tabular Figures:** Always apply `font-mono tabular-nums` to financial data, timestamps, metrics, and counters to prevent layout shift.

---

## 4. Tactile Details & Micro-Interactions

- **Borders & Dividers:** Use hair-thin dividers (`1px`) with semi-transparent alphas (`border-border/60` or `divide-zinc-800`).
- **Transitions:** Fast and mechanical. Duration must remain between `120ms` and `200ms` using high-precision easing:
  ```css
  transition: all 150ms cubic-bezier(0.16, 1, 0.3, 1);
  ```
