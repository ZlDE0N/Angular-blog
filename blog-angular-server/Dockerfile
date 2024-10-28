# Usa la imagen base de SQL Server
FROM mcr.microsoft.com/mssql/server:2022-latest

# Variables de entorno para aceptar la EULA y configurar la contraseña del SA
ENV ACCEPT_EULA=Y
ENV MSSQL_SA_PASSWORD=Security2024*
ENV MSSQL_PID=Developer
ENV MSSQL_COLLATION=SQL_Latin1_General_CP1_CI_AS

# Copiar un archivo de inicialización opcional (si se necesita)
# COPY ./init.sql /docker-entrypoint-initdb.d/

# Expone el puerto SQL Server por defecto
EXPOSE 1435
