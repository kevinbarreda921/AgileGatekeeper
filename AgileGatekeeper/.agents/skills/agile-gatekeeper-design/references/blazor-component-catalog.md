# Blazor (.NET 10) Component Catalog & UI Patterns

Use these standardized patterns when building pages and components in **AgileGatekeeper**.

---

## 1. Module Page Header with Primary Action

Every operational page in `Components/Pages/Modulos/` or `Notificaciones/` should start with a clean header bar:

```razor
<div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4 mb-6">
    <div>
        <h2 class="font-display text-xl font-bold text-slate-900 dark:text-white tracking-tight">
            Título del Módulo
        </h2>
        <p class="text-xs text-slate-400 dark:text-slate-500 mt-0.5">
            Descripción breve del propósito operativo de la vista.
        </p>
    </div>

    <div class="flex items-center gap-2.5">
        <!-- Botón Secundario -->
        <button type="button"
                class="h-9 px-4 rounded-xl border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-800 hover:bg-slate-50 dark:hover:bg-slate-700 text-slate-700 dark:text-slate-200 text-xs font-semibold transition-colors flex items-center gap-1.5 cursor-pointer shadow-subtle">
            <span class="material-symbols-outlined text-[16px]">filter_list</span>
            <span>Filtrar</span>
        </button>

        <!-- Botón Primario (Píldora con Glow) -->
        <button type="button"
                class="h-9 px-4 rounded-full bg-blue-600 hover:bg-blue-700 active:scale-[0.98] text-white text-xs font-semibold transition-all flex items-center gap-1.5 cursor-pointer"
                style="box-shadow: rgba(37, 99, 235, 0.35) 0px 4px 14px -1px;">
            <span class="material-symbols-outlined text-[16px]">add</span>
            <span>Nuevo Registro</span>
        </button>
    </div>
</div>
```

---

## 2. KPI / Telemetry Card Pattern

```razor
<div class="bg-white dark:bg-[#0f172a] rounded-2xl border border-slate-200/80 dark:border-slate-800 p-5 shadow-subtle hover:shadow-md transition-all">
    <div class="flex items-center justify-between">
        <span class="text-[11px] font-semibold text-slate-400 dark:text-slate-500 uppercase tracking-wider">
            @Label
        </span>
        <div class="w-9 h-9 rounded-xl bg-blue-50 dark:bg-blue-950/40 text-blue-600 dark:text-blue-400 flex items-center justify-center">
            <span class="material-symbols-outlined text-[19px]">@Icon</span>
        </div>
    </div>

    <div class="mt-3 flex items-baseline gap-2.5">
        <span class="font-display text-2xl font-bold text-slate-900 dark:text-white tabular-nums tracking-tight">
            @Value
        </span>
        <span class="inline-flex items-center gap-0.5 px-2 py-0.5 rounded-full text-[11px] font-semibold bg-emerald-50 text-emerald-600 dark:bg-emerald-950/40 dark:text-emerald-400">
            <span class="material-symbols-outlined text-[13px]">trending_up</span>
            @Trend
        </span>
    </div>

    <span class="text-[11.5px] text-slate-400 dark:text-slate-500 mt-1.5 block">
        @Subtitle
    </span>
</div>
```

---

## 3. Enterprise Data Table / List Card

```razor
<div class="bg-white dark:bg-[#0f172a] rounded-2xl border border-slate-200/80 dark:border-slate-800 shadow-subtle overflow-hidden">
    <!-- Card Toolbar -->
    <div class="px-5 py-4 border-b border-slate-100 dark:border-slate-800 flex flex-col sm:flex-row sm:items-center justify-between gap-3">
        <span class="text-xs font-semibold text-slate-500 dark:text-slate-400 uppercase tracking-wider">
            Registros Activos
        </span>

        <!-- Search Input -->
        <div class="relative w-full sm:w-64">
            <span class="material-symbols-outlined text-[17px] text-slate-400 absolute left-3 top-1/2 -translate-y-1/2">search</span>
            <input type="text"
                   placeholder="Buscar..."
                   class="w-full h-8 pl-9 pr-3 rounded-lg border border-slate-200 dark:border-slate-700 bg-slate-50/70 dark:bg-slate-800/60 text-xs text-slate-800 dark:text-slate-100 placeholder:text-slate-400 focus:outline-none focus:bg-white dark:focus:bg-slate-900 focus:border-blue-600 focus:ring-2 focus:ring-blue-600/15 transition-all" />
        </div>
    </div>

    <!-- Table -->
    <div class="overflow-x-auto">
        <table class="w-full text-left border-collapse">
            <thead>
                <tr class="border-b border-slate-100 dark:border-slate-800/80 bg-slate-50/50 dark:bg-slate-800/20 text-[11px] font-semibold text-slate-400 dark:text-slate-500 uppercase tracking-wider">
                    <th class="py-3 px-5">ID</th>
                    <th class="py-3 px-5">Entidad</th>
                    <th class="py-3 px-5">Estado</th>
                    <th class="py-3 px-5 text-right">Acciones</th>
                </tr>
            </thead>
            <tbody class="divide-y divide-slate-100 dark:divide-slate-800/70 text-[13px]">
                <tr class="hover:bg-slate-50/60 dark:hover:bg-slate-800/30 transition-colors">
                    <td class="py-3.5 px-5 font-semibold text-blue-600 dark:text-blue-400 tabular-nums">#1024</td>
                    <td class="py-3.5 px-5 font-medium text-slate-900 dark:text-white">Nombre del Registro</td>
                    <td class="py-3.5 px-5">
                        <span class="inline-flex items-center gap-1.5 px-2.5 py-1 rounded-full text-xs font-medium bg-emerald-50 text-emerald-600 dark:bg-emerald-950/40 dark:text-emerald-400">
                            <span class="w-1.5 h-1.5 rounded-full bg-emerald-500"></span>
                            Activo
                        </span>
                    </td>
                    <td class="py-3.5 px-5 text-right">
                        <button class="p-1.5 rounded-lg text-slate-400 hover:text-slate-700 dark:hover:text-slate-200 hover:bg-slate-100 dark:hover:bg-slate-800 transition-colors">
                            <span class="material-symbols-outlined text-[18px]">more_horiz</span>
                        </button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</div>
```

---

## 4. Form Controls (`EditForm` Pattern)

```razor
<div class="space-y-1.5">
    <label class="block text-xs font-semibold text-slate-700 dark:text-slate-300">
        Nombre del Campo
    </label>
    <InputText @bind-Value="model.Property"
               placeholder="Ingresa un valor..."
               class="w-full h-10 px-3.5 rounded-xl border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-900 text-[13px] text-slate-900 dark:text-white placeholder:text-slate-400 focus:outline-none focus:border-blue-600 focus:ring-2 focus:ring-blue-600/20 transition-all" />
    <ValidationMessage For="@(() => model.Property)" class="text-xs text-red-500 mt-1" />
</div>
```

---

## 5. Modal Dialog Pattern

```razor
@if (IsOpen)
{
    <div class="fixed inset-0 z-50 bg-slate-900/40 dark:bg-black/70 backdrop-blur-xs flex items-center justify-center p-4 transition-opacity"
         @onclick="OnClose">
        <div class="w-full max-w-md bg-white dark:bg-[#0f172a] rounded-2xl border border-slate-200/80 dark:border-slate-800 shadow-modal p-6 relative"
             @onclick:stopPropagation="true">
            <div class="flex items-center justify-between pb-4 border-b border-slate-100 dark:border-slate-800">
                <h3 class="font-display text-base font-bold text-slate-900 dark:text-white">@Title</h3>
                <button type="button"
                        @onclick="OnClose"
                        class="w-7 h-7 rounded-lg text-slate-400 hover:text-slate-600 dark:hover:text-slate-200 hover:bg-slate-100 dark:hover:bg-slate-800 flex items-center justify-center transition-colors">
                    <span class="material-symbols-outlined text-[18px]">close</span>
                </button>
            </div>

            <div class="py-4">
                @ChildContent
            </div>
        </div>
    </div>
}
```
