# PlayVerse: Postman, Brevo y Render

## 1. Postman

Importa:

- `API.postman_collection.json`
- `API.postman_environment.template.json`

Luego duplica el environment template y cambia:

- `baseUrl`
- `registerEmail`
- `registerUsername`
- `registerPassword`

Orden recomendado para demo:

1. `AUTH > Register > 1. Comenzar proceso de registro`
2. `AUTH > Register > 2. Validar si el token de registro existe`
3. `AUTH > Register > 3. Completar proceso de registro`
4. `AUTH > Iniciar Sesión`
5. `AUTH > Renovar Sesión`
6. `AUTH > Recover Password > 1. Enviar código para recuperar contraseña`
7. `AUTH > Recover Password > 2. Cambiar contraseña (OTPCODE)`
8. `AUTH > Iniciar Sesión`
9. `AUTH > Recover Password > 3. Realizar cambió de contraseña`
10. `App > Información de la APP`

Qué se autoguarda:

- `registerToken`
- `userId`
- `token`
- `refreshToken`
- `otpCode`
- `appVersion`

## 2. Brevo

En Brevo necesitas:

1. Tener un remitente verificado.
2. Tener acceso a SMTP.
3. Copiar:
   - servidor SMTP
   - puerto
   - usuario SMTP
   - clave SMTP

Configuración del backend:

```env
Smtp__Host=smtp-relay.brevo.com
Smtp__Port=587
Smtp__User=tu_usuario_smtp
Smtp__Password=tu_clave_smtp
Smtp__From=tu_correo_verificado
```

Para desarrollo local puedes usar `dotnet user-secrets` o variables de entorno del sistema.

Ejemplo:

```powershell
dotnet user-secrets set "Smtp:Host" "smtp-relay.brevo.com" --project .\\Steam.Web.Api
dotnet user-secrets set "Smtp:Port" "587" --project .\\Steam.Web.Api
dotnet user-secrets set "Smtp:User" "tu_usuario_smtp" --project .\\Steam.Web.Api
dotnet user-secrets set "Smtp:Password" "tu_clave_smtp" --project .\\Steam.Web.Api
dotnet user-secrets set "Smtp:From" "tu_correo_verificado" --project .\\Steam.Web.Api
```

## 3. Render

### Opción recomendada

1. Sube el repo a GitHub.
2. En Render elige `New +`.
3. Selecciona `Blueprint` o `Web Service`.
4. Conecta el repo.
5. Usa el `render.yaml` del proyecto.

### Variables de entorno

Configura en Render:

```env
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:10000
ConnectionStrings__database-1=tu_connection_string_sql_server
Jwt__Issuer=https://tu-servicio.onrender.com
Jwt__Audience=https://tu-frontend.com
Jwt__PrivateKey=tu_clave_jwt
Smtp__Host=smtp-relay.brevo.com
Smtp__Port=587
Smtp__From=tu_correo_verificado
Smtp__User=tu_usuario_smtp
Smtp__Password=tu_clave_smtp
FirstAppTime__User__UserName=User Root
FirstAppTime__User__Email=tu_admin_inicial
FirstAppTime__User__Password=tu_password_inicial
```

### Base de datos

Este proyecto usa SQL Server. En Render necesitas apuntar a una base SQL Server externa.

### Después del despliegue

1. Copia la URL pública de Render.
2. Pon esa URL como `baseUrl` en tu environment de Postman.
3. Si cambió el dominio final, actualiza `Jwt__Issuer`.
