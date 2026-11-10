---
name: senior-blazor-architecture
description: >-
  Use this skill whenever creating, modifying, organizing, or refactoring Blazor (.NET 10)
  components, layouts, pages, menus, headers, or modules in AgileGatekeeper. Enforces
  senior-level software architecture, zero-monolith modular separation (headers, sidebars,
  nav items, and widgets in dedicated component files), strict feature-folder placement,
  and best practices for component programming, dependency injection, and memory management.
triggers:
  - "componentes"
  - "arquitectura"
  - "menu"
  - "header"
  - "login"
  - "modulo"
  - "desarrollador senior"
  - "layout"
  - "blazor architecture"
  - "senior architecture"
---

# Senior Blazor (.NET 10) Component Architecture & Engineering Rules

Este skill define el estándar obligatorio de arquitectura, modularidad y buenas prácticas que Antigravity debe aplicar en cada intervención dentro del proyecto **AgileGatekeeper**.

---

## 1. Principio Fundamental: Cero Monolitos (Strict Modular Separation)

Nunca incrustes menús, cabeceras, barras de herramientas o formularios complejos directamente dentro de un archivo de layout o una página masiva.

### Reglas de Desacoplamiento:
1. **Layouts Limpios (`MainLayout.razor`, `LoginLayout.razor`)**:
   - Los archivos de Layout actúan **exclusivamente como orquestadores**.
   - Solo deben contener la grilla estructural base, la invocación a los subcomponentes modulares (`<Sidebar />`, `<Header />`) y el `@Body`.
2. **Componentes Atómicos en su Propio Archivo**:
   - **Header**: En su propio archivo `Components/Layout/Menu/Header.razor` (saludos, acciones circulares, buscador, notificaciones, conmutador de tema).
   - **Sidebar**: En su propio archivo `Components/Layout/Menu/Sidebar.razor` (logo, navegación, botón de colapso, drawer móvil).
   - **Ítems de Navegación**: `Components/Layout/Menu/SidebarNavItem.razor` (detección de ruta activa, badge contador, estilos de píldora).
   - **Perfil de Usuario**: `Components/Layout/Menu/UserProfileWidget.razor` (avatar, estado en línea, nombre, ubicación).
   - **Reconexión**: `Components/Layout/Reconnect/ReconnectModal.razor`.
   - **Modales y Diálogos**: En componentes modales aislados reutilizables.

---

## 2. Organización Estricta por Dominios (Feature Folders)

Cada archivo nuevo DEBE pertenecer a una carpeta temática concreta:

```text
AgileGatekeeper/
├── Components/
│   ├── Layout/
│   │   ├── MainLayout.razor              # Orquestador del shell principal
│   │   ├── LoginLayout.razor             # Layout aislado para autenticación
│   │   ├── Menu/                         # Componentes de menú y cabecera
│   │   │   ├── Sidebar.razor
│   │   │   ├── Header.razor
│   │   │   ├── SidebarNavItem.razor
│   │   │   └── UserProfileWidget.razor
│   │   └── Reconnect/
│   │       └── ReconnectModal.razor
│   ├── Shared/                           # UI reutilizable transversal
│   │   └── WorkspacePlaceholder.razor    # Contenedores, modales, tablas genéricas
│   └── Pages/
│       ├── Login/                        # Pantallas de acceso y credenciales
│       ├── Menu/                         # Dashboard y página de inicio
│       ├── Modulos/                      # Módulos de negocio (Orders, Shipments, etc.)
│       ├── Notificaciones/               # Centro de avisos y mensajería
│       └── Sistema/                      # Error, NotFound (404)
├── Models/
│   ├── Auth/                             # LoginModel, credenciales
│   └── Users/                            # UserProfile, roles
└── Services/
    ├── Auth/                             # IAuthService, AuthService
    └── Theme/                            # IThemeService, ThemeService
```

---

## 3. Estándares Senior de Programación de Componentes en Blazor

Al escribir o modificar cualquier componente `.razor`:

1. **Parámetros Explícitos y Reutilización**:
   - Declara parámetros con `[Parameter]`.
   - Usa `[EditorRequired]` en parámetros obligatorios para prevenir errores en tiempo de compilación.
   - Usa `RenderFragment` o `RenderFragment<T>` para permitir contenido hijo dinámico (slots).
2. **Comunicación Reactiva y Eventos**:
   - Usa `EventCallback` y `EventCallback<T>` para comunicar eventos del componente hijo al padre (`@bind-IsCollapsed`, `OnOpenMobileSidebar`, `OnClose`).
   - Evita acoplamientos rígidos entre componentes hermanos.
3. **Prevención de Fugas de Memoria (`IDisposable`)**:
   - Todo componente que se suscriba a eventos externos (`NavigationManager.LocationChanged`, eventos de servicios singleton o scoped como `OnAuthStateChanged`, `OnThemeChanged`) **DEBE** implementar `@implements IDisposable` y desuscribirse en el método `Dispose()`.
4. **Formularios Robustos**:
   - Emplea siempre `EditForm`, `DataAnnotationsValidator` y `ValidationMessage` vinculados a un modelo en `Models/`.
   - Mantén los estados interactivos (como mostrar/ocultar contraseña o spinners de carga) en variables booleanas fuertemente tipadas en C#.
5. **Inyección de Dependencias**:
   - Programa contra interfaces (`@inject IAuthService`, `@inject IThemeService`), nunca contra clases concretas.
   - Registra todo servicio en `Program.cs` con el ciclo de vida apropiado (`AddScoped` para Blazor Server/Interactive).

---

## 4. Filosofía de Estilo y Acabado Visual

- **Integración con el Sistema de Diseño**:
  - Respeta las guías de `agile-gatekeeper-design` y `signature-minimal-ui`.
  - Asegura paridad total entre Modo Claro y Modo Oscuro con clases `dark:*`.
  - Utiliza `material-symbols-outlined` para la iconografía y `font-display` / `font-sans` para tipografía.
- **Sin CSS Aislado Redundante**:
  - Prefiere siempre utilidades Tailwind y tokens centralizados en `wwwroot/app.css`. Evita crear archivos `.razor.css` dispersos innecesarios.
