# HardTripAlpha
![logo](https://avatars3.githubusercontent.com/u/32564126?v=4&s=200)

## Introducción
 
Este repositiorio va destinado a la programación de la versión Alpha de HardTrip. El paradigma de programación es el orientado a objetos y buscando siempre la modularidad.

## Instalación

A continuación se explican los pasos básico para la instalación del proyecto de Unity:

1. Normalmente, los proyectos de Unity se encuentran en el directorio C:\Usuarios\<Mi Usuario>\Documentos, así que lo primero es movernos a ese directorio. Para asegurarnos lo mejor es comprobar si hay otras carpetas de proyectos de Unity ahí.

2. Instalar Git en caso de que no lo tengamos instalado: https://git-scm.com/download/win. Abrimos un terminal o PowerShell en ese mismo directorio. (Shift + click derecho)

3. Escribimos el siguiente comando *git clone* https://github.com/CAViTECgames/HardTripAlpha. Con esto se nos añadira una nueva carpeta con el proyecto junto a las otras que ya tuviesemos.

4. Abrir Unity y elegir este proyecto. En caso de que no aparezca lo mejor es utilizar la opción Open y buscar la nueva carpeta del proyecto manualmente.

## Contribución

Cuando hagamos algo que queramos subir al repositorio, los pasos a seguir son los siguientes:

1. Abrir un terminal en la carpeta del proyecto.

2. El primer comando a usar será un *git status* donde deben salir en rojo los archivos que hayamos modificado o añadido. Se recomienda guardar y cerrar el proyecto en Unity antes de hacer esto.

3. Si lo anterior es coherente podemos hace *git add .* para añadir todos los cambios en el proyecto o *git add <Nombre archivo>* para solo un archivo concreto.
  
4. Después hay que hacer *git commit -m "Mensaje con información sobre el cambio hecho"* para registrar el cambio en nuestro repositorio local.

5. Tras todo eso sólo queda hace *git push origin master* para subir todos los cambios al repositorio y a la rama "master", que será la que usaremos normalmente.

## Actualización del proyecto local

Si ya teniamos descargado el repositorio en nuestro ordenador y se han hecho cambios en el que está subido en GitHub, lo mejor es seguir estos pasos para evitar tener que volver a descargarlo todo:

1. Abrimos un terminal en la carpeta de nuestro proyecto.

2. Escribimos el siguiente comando *git pull origin master*. Con este comando  se descargarán todos los cambios que se hayan producido en el proyecto.

3. Si ya habiamos hecho cambios a nuestro proyecto que aun no habiamos subido, será neceseraio hacer antes un *git commit -m "Mensaje con información sobre el cambio hecho"* para poder completar sin error el paso anterior, ya que la consola nos avisará de que nuestro cambios podrían perderse. Si queréis, podéis desechar esos cambios mediante el comando *git stash*, así podéis completar el paso anterior sin tener que seguir usando vuestros cambios.

4. Al reabrir el proyecto, deberían estar esos cambios ya añadidos.

## Cómo cambiar de rama

Algunas veces puede resultar útil dividir el repositorio en varias "ramas", sobre todo si somos conscientes de que lo hagamos se pise con lo que está haciendo otro o por si son cambios que no están lo suficientemente listos como para implementarlos en la versión principal. Con estos pasos podemos cambiar de rama:

1. Abrimos un terminal en la carpeta de nuestro proyecto.

2. Ejecutamos *git pull origin <Nombre de la rama>* y con esto se nos añade una rama nueva a nuestro repositirio local.

3. Si habiamos hecho cambios mientras estabamos en nuestra rama master, que aun no habiamos subido, hacemos *git commit -m "Mensaje con información sobre el cambio hecho"* para guardarlos o *git stash* para desecharlos.

4. Hacemos *git checkout <Nombre de la rama>* para cambiar a esa rama. Para volver a la principal, se hace lo mismo *git checkout master*. 
