
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

Abrir el administrador de paquetes para la solucion en Herramientas > Administrador de paquetes NuGet > Consola del Administrador de paquetes

 Despues ejecutar el siguiente comandos en la terminal y esperar a que termine:
```
add-migration CreacionTablaAvion    
```
Despues ejecutar el siguiente comando en la terminal y esperar a que termine:
```
update-database
```
