
# 1.- Clonar el repositorio

Clonar el repositorio:
```
git clone https://github.com/manolo-tambien/ApiAviones.git codigoFuente
```
## 1.1 Instalar instancia de sql server en docker sobre Host(MacOS Sillicon Apple)
Toda la información se encunetra en el siguiente enlace https://bornsql.ca/blog/you-can-run-a-sql-server-docker-container-on-apple-m1-and-m2-silicon/ y de forma resumida se explica con lo siguientes comandos:

Comenzar con el siguiente comando:
```
docker pull mcr.microsoft.com/mssql/server:2022-latest
```
Siguiente comando: 
```
docker run -e "ACCEPT_EULA=1" -e "MSSQL_SA_PASSWORD=MyPass@word" -e "MSSQL_PID=Developer" -e "MSSQL_USER=SA" -p 1433:1433 -d --name=sql mcr.microsoft.com/azure-sql-edge
```

# 2.- Cambiar el nombre del servidor de la cadena de conexión

En el archivo appsettings.json buscar la variable "ConexionSql" y escribir el nombre de tu servidor SQL

# 3.- Migracion de la base de datos
Se presentan dos métodos para correr la migración, la primera con equipos windows y la seguda para MacOS Apple Sillicon:

## 3.1 Instrucciones para equipo con Windows
Abrir el administrador de paquetes para la solucion en Herramientas > Administrador de paquetes NuGet > Consola del Administrador de paquetes

 Despues ejecutar el siguiente comandos en la terminal y esperar a que termine:
```
cd ApiAviones
add-migration CreacionTablaAvion    
```
Despues ejecutar el siguiente comando en la terminal y esperar a que termine. Hará un push a la base de datos con lo que creó:
```
update-database
```
## 3.2 Insttrucciones para equipos con (MacOS Apple Sillicon)
Instalar primero dotnet-ef en la terminal y en el directorio del proyecto:
```
dotnet tool install --global dotnet-ef
```
Agregar el directorio de las herramientas "dotnet-ef" al path de variables
```
export PATH="$PATH:/Users/'your user folder'/.dotnet/tools"
```
Ejecutar los siguientes comandos:
```
dotnet restore
dotnet ef
```
Si todo marcha bien ejecutar la migración:
```
dotnet ef migrations add NOMBRE_DE_LA_MIGRACION
dotnet ef database update
```
En caso de que falle la autenticación usar el siguiente formato para la cadena de conexión: 
```
"Server=localhost,1433;Database=ApiAviones;User ID=sa;Password=MyPass@word;TrustServerCertificate=true;MultipleActiveResultSets=true;"
```

Listo, la base de datos y las tablas fueron creadas.
