# PlayVerse Backend

Backend en ASP.NET Core para PlayVerse: tu universo de videojuegos en un solo lugar.

## Postman

Importa estos dos archivos:

- `API.postman_collection.json`
- `API.postman_environment.template.json`

La coleccion esta preparada para demo:

- Guarda `registerToken` al iniciar registro.
- Guarda `userId` al completar registro.
- Guarda `token` y `refreshToken` al iniciar sesion y renovar sesion.
- Guarda `otpCode` para recuperacion de contrasena.
- Guarda `appVersion` al consultar la info de la app.
- Usa variables para `baseUrl`, credenciales y passwords de prueba.

Variables principales:

- `baseUrl`
- `registerEmail`
- `registerUsername`
- `registerPassword`
- `loginEmail`
- `loginPassword`
- `recoverNewPassword`
- `changePasswordNew`

## Brevo SMTP

El proyecto funciona con SMTP, asi que Brevo se integra sin cambiar de proveedor ni libreria.

Valores a configurar:

- `Smtp__Host=smtp-relay.brevo.com`
- `Smtp__Port=587`
- `Smtp__User=tu_usuario_smtp_de_brevo`
- `Smtp__Password=tu_clave_smtp_de_brevo`
- `Smtp__From=tu_remitente_validado`

No subas estas credenciales a GitHub. Configuralas como variables de entorno o secretos.

## Render

Se agregaron estos archivos para despliegue:

- `Dockerfile`
- `.dockerignore`
- `render.yaml`

Variables de entorno minimas en Render:

- `ConnectionStrings__database-1`
- `Jwt__Issuer`
- `Jwt__Audience`
- `Jwt__PrivateKey`
- `Cors__AllowedOrigins`
- `Smtp__From`
- `Smtp__User`
- `Smtp__Password`
- `FirstAppTime__User__Email`
- `FirstAppTime__User__Password`

Render no ofrece base de datos SQL Server administrada dentro de su oferta habitual, asi que para este proyecto necesitas una instancia externa de SQL Server.

`Cors__AllowedOrigins` acepta uno o varios dominios separados por comas. Ejemplo:

```text
https://playverse.onrender.com,https://www.playverse.example
```

## Notas

- En Render se habilito soporte de `X-Forwarded-*` para que HTTPS funcione bien detras del proxy.
- `appsettings.json` quedo con placeholders, no con credenciales reales.
