[Languages](README.md)

# Política de privacidad

Última actualización: 21 de octubre de 2026

**BUtil** by Siarhei Kuchuk

Nombre de la aplicación: BUtil
Nombre del desarrollador: Siarhei Kuchuk

BUtil hace copias de seguridad, sincroniza y restaura archivos en este dispositivo. También puede importar medios, compartir una carpeta por FTPS o subir archivos a un servidor que usted configure. No crea una cuenta del desarrollador. El desarrollador no opera un servidor que reciba sus archivos, contraseñas o datos de uso.

## Datos que el desarrollador no recopila

La aplicación no incluye anuncios, analítica, informes de fallos ni SDK de seguimiento. El desarrollador no recopila, vende ni comparte datos personales.

Esta política de privacidad y la licencia se muestran dentro de la aplicación. No se abren como páginas web.

## Datos almacenados en su dispositivo

### Tareas y ajustes

Las definiciones de las tareas se almacenan solo en el almacenamiento privado de la aplicación en este dispositivo. Una tarea puede incluir rutas de carpetas, una programación, ajustes de almacenamiento y las contraseñas o tokens que usted escriba. Las contraseñas y los secretos de almacenamiento se sellan con el almacén de claves de Android antes de guardarse y solo se pueden leer en este dispositivo. Esos valores no se envían al desarrollador. No se incluyen en la copia de seguridad de Android. Desinstalar la aplicación los elimina.

El tema que usted elige se almacena en los ajustes privados de la aplicación en este dispositivo.

### Archivos que usted elige

La copia de seguridad, la sincronización, la restauración, la importación y la subida leen y escriben las carpetas que usted selecciona con el selector de documentos del sistema, o el servidor que usted configure. Esos archivos permanecen en las carpetas que eligió o en el destino de almacenamiento que configure. La aplicación no los sube al desarrollador. Desinstalar la aplicación no elimina los archivos de las carpetas que usted seleccionó.

### Certificado FTPS

Cuando comparte una carpeta por FTPS, la aplicación crea un certificado autofirmado en el almacenamiento privado de la aplicación. Desinstalar la aplicación elimina ese certificado. Los archivos compartidos permanecen en la carpeta que usted autorizó.

No se usa ningún servidor del desarrollador para almacenar sus datos.

## Uso de la red

### Destinos que usted configura

Cuando se ejecuta una tarea, la aplicación se conecta solo al lugar que usted indicó. Puede ser una carpeta de este dispositivo o un servidor que usted escriba: FTP, FTPS, SFTP, WebDAV, SMB, NFS, almacenamiento compatible con S3 o Azure Blob Storage. Los nombres de los archivos, el contenido de los archivos y las credenciales que usted introdujo se envían a ese servidor para que la tarea pueda ejecutarse. Cada uno de esos servicios tiene su propia política de privacidad. El desarrollador no recibe ese tráfico.

Compartir una carpeta por FTPS escucha en este dispositivo para que un cliente FTPS que usted configure pueda leer o escribir la carpeta que usted autorizó, con el nombre de usuario y la contraseña que usted estableció. Ese tráfico permanece entre sus dispositivos (y cualquier persona de la red que tenga esas credenciales). El desarrollador no es parte de él. Usted es responsable de quién puede alcanzar el puerto, de qué carpeta comparte y de la solidez de esa contraseña.

### Enlace que usted abre

La fila de la versión en el menú puede abrir la página de inicio del proyecto en el navegador del sistema:

[github.com/drweb86/butil](https://github.com/drweb86/butil)

Ese sitio tiene su propia política de privacidad. El desarrollador no recibe ese tráfico.

La aplicación no comprueba actualizaciones y no contacta con un servicio de actualización.

## Programación

Puede ejecutar una tarea cuando el dispositivo se inicia o en los días que usted elija. La aplicación lo registra con el reloj de alarma del sistema. Eso solo inicia esta aplicación en su dispositivo.

## Permisos

La aplicación usa solo los siguientes permisos:

- **Internet** y **estado de la red**, para alcanzar un servidor que usted configure y para compartir una carpeta por FTPS.
- **Notificaciones** y un **servicio en primer plano**, para mantener en marcha una tarea en ejecución con la pantalla apagada.
- **Alarmas exactas** y **ejecución al inicio**, para iniciar una tarea programada.
- **Bloqueo de activación**, para reducir la suspensión durante una transferencia.

## Menores

La aplicación es una herramienta de copia de seguridad y sincronización de archivos. No está dirigida a menores de 13 años.

## Terceros

Los proveedores de almacenamiento que usted configura procesan los archivos y las credenciales que la tarea les envía. La apertura de la página de inicio del proyecto la realizan el navegador del sistema y ese sitio. El desarrollador no recibe ese tráfico.

## Cambios

Las actualizaciones de esta política se incluyen en la aplicación.

## Contacto

Nombre de la aplicación: BUtil
Nombre del desarrollador: Siarhei Kuchuk

Las preguntas sobre esta aplicación para Android pueden enviarse a través de la ficha de la tienda que usó para instalarla, o en [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues).
