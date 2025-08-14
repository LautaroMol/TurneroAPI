# Requerimientos del turnero Médico con Auth0

Este documento define las funcionalidades y requisitos clave del sistema a desarrollar.

---

## 1. Autenticación y Autorización
- **Auth0** como proveedor de autenticación.
- Registro e inicio de sesión para:
  - Médicos
  - Usuarios (pacientes)
- Roles:
  - **Administrador**
  - **Médico**
  - **Paciente**
- Restricción de acceso a controladores y endpoints según rol.

---

## 2. Gestión de Usuarios
- Registro de médicos con datos:
  - Nombre y Apellido
  - Especialidad médica
  - Número de matrícula
  - Horarios de atención
  - Email y teléfono
- Registro de pacientes con datos:
  - Nombre y Apellido
  - DNI
  - Email y teléfono
- Edición y actualización de datos por parte del usuario.
- Posibilidad de dar de baja la cuenta.

---

## 3. Gestión de Turnos
- Solicitud de cita con médico.
- El médico puede:
  - Aceptar
  - Rechazar
  - Reprogramar
- El paciente recibe notificación por email en cada acción.
- El paciente puede cancelar un turno.
- Calendario de disponibilidad de cada médico.

---

## 4. Recordatorios y Notificaciones
- Envío automático de email recordatorio:
  - 24 horas antes del turno.
  - 1 hora antes del turno.
- Confirmación de turno por parte del paciente.

---

## 5. Panel de Administración
- Vista para el administrador:
  - Listado de médicos y pacientes.
  - Estadísticas de citas.
  - Posibilidad de bloquear usuarios.
  - Gestión de roles.

---

## 6. Seguridad
- Autenticación centralizada con Auth0.
- Tokens JWT para proteger endpoints.
- Validación de permisos en controladores.
- Configuración de CORS para limitar accesos.

---

## 7. Otros Módulos y Funcionalidades Extra (para práctica)
- **Historial Médico**:
  - El médico puede cargar información sobre consultas previas.
  - El paciente puede ver su historial.
- **Subida de Archivos**:
  - Posibilidad de adjuntar estudios o recetas.
- **Chat Médico-Paciente**:
  - Mensajería en tiempo real (SignalR o WebSockets).
- **Reseñas**:
  - Los pacientes pueden calificar y comentar su experiencia con un médico.

---

## 8. Aspectos Técnicos
- Backend: ASP.NET Core 8 Web API
- Base de datos: SQL Server
- ORM: Entity Framework Core
- Autenticación: Auth0 (OAuth2 / OpenID Connect)
- Email: Servicio SMTP o proveedor externo (SendGrid, MailKit)
- Integración de colas (opcional) para tareas programadas.

---
