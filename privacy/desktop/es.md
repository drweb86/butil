[Languages](README.md)

# Política de privacidad

Última actualización: 25 de septiembre de 2026

**BUtil** by Siarhei Kuchuk

Nombre de la aplicación: BUtil
Nombre del desarrollador: Siarhei Kuchuk

BUtil hace copias de seguridad, sincroniza y restaura archivos en este equipo. También puede importar medios, compartir una carpeta o subir archivos a un servidor que usted configure. No crea una cuenta de desarrollador. El desarrollador no opera un servidor que reciba sus archivos, contraseñas o datos de uso.

## Datos que el desarrollador no recopila

La aplicación no incluye anuncios, analítica, informes de errores ni SDK de seguimiento. El desarrollador no recopila, vende ni comparte datos personales.

## Datos almacenados en su equipo

### Tareas y configuración

Las definiciones de tareas se guardan solo en este equipo. Una tarea puede incluir rutas de carpetas, una programación, la configuración del almacenamiento y las contraseñas o tokens que usted escriba. Las contraseñas y los secretos de almacenamiento se cifran en este equipo antes de guardarse, y solo se pueden leer en este equipo. Esos valores no se envían al desarrollador.

- Tareas de Windows: `%AppData%\BUtil Backup Tasks`
- Tareas de Linux: `~/.config/BUtil Backup Tasks`
- Configuración de Windows (incluido el tema y el idioma elegido por última vez para la licencia o la privacidad): `%AppData%\BUtil\Settings\v1`
- Configuración de Linux: `~/.config/BUtil/Settings/v1`
- Estado de las tareas de Windows: `%AppData%\BUtil\States`
- Estado de las tareas de Linux: `~/.config/BUtil/States`
- Estado de importación de medios de Windows: `%AppData%\BUtil Backup Tasks - States`
- Estado de importación de medios de Linux: `~/.config/BUtil Backup Tasks - States`

### Archivos que usted elige

La copia de seguridad, la sincronización, la restauración y la importación leen y escriben las carpetas que seleccione. Esos archivos permanecen en este equipo o en el destino de almacenamiento que configure. La aplicación no los envía al desarrollador.

### Registros

Los registros de diagnóstico se escriben solo en este equipo:

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

Esos archivos no se envían a ningún sitio.

No se usa ningún servidor del desarrollador para almacenar sus datos.

## Uso de la red

### Destinos que usted configura

Cuando se ejecuta una tarea, la aplicación se conecta solo al lugar que usted indique. Puede ser una carpeta local o un servidor que introduzca: FTP, FTPS, SFTP, WebDAV, SMB, NFS, almacenamiento compatible con S3 o Azure Blob Storage. Los nombres de archivo, el contenido y las credenciales que introdujo se envían a ese servidor para que la tarea pueda ejecutarse. Cada uno de esos servicios tiene su propia política de privacidad. El desarrollador no recibe ese tráfico.

BUtil Server puede escuchar en este equipo para que un cliente BUtil que usted configure pueda enviar archivos. Ese tráfico permanece entre los equipos que configure.

### Comprobación de actualizaciones

Las compilaciones que no son de la Store pueden solicitar la última versión de GitHub:

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub (Microsoft) recibe una solicitud HTTPS normal (dirección IP, user-agent, hora). El desarrollador no recibe ese tráfico.

Las instalaciones de Microsoft Store no usan esta comprobación; la Store entrega las actualizaciones.

### Enlaces que abre

La aplicación puede abrir estas páginas en el navegador del sistema. Esos sitios tienen sus propias políticas de privacidad:

- Página del proyecto: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- Última versión: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- Ayuda de patrones de archivos: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- Ayuda de formato de fecha: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- Créditos de iconos: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

La licencia y esta política de privacidad se muestran dentro de la aplicación. No se abren como páginas web.

## Programación

En Windows puede ejecutar una tarea al iniciar sesión o con una programación semanal. La aplicación lo registra en el Programador de tareas de Windows con un nombre que empieza por `BUtil`. Eso solo inicia esta aplicación en su equipo.

## Menores

La aplicación es una herramienta de copia de seguridad y sincronización de archivos. No está dirigida a menores de 13 años.

## Terceros

GitHub procesa la solicitud de comprobación de actualizaciones y las páginas que abre, como se indica arriba. Microsoft Store procesa las instalaciones y actualizaciones de la Store. Los proveedores de almacenamiento que configure procesan los archivos y las credenciales que la tarea les envía. El desarrollador no recibe ese tráfico.

## Cambios

Las actualizaciones de esta política se publicarán en este archivo del repositorio del proyecto.

## Contacto

Nombre de la aplicación: BUtil
Nombre del desarrollador: Siarhei Kuchuk

Preguntas: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
