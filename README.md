
# 1.- Clonar el repositorio

Clonar el repositorio:
```
git clone https://github.com/manolo-tambien/ApiAviones.git codigoFuente
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
