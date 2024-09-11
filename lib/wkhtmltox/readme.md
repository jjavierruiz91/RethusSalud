# Instalacion de wkhtmltox
Esta libreria funciona para generar html a pdf 

## 1. Libreria 
Link: https://wkhtmltopdf.org/

### 1.1. Paso de instalacion 
Para estos pasos se utiliza el binario compilado 

Vamos a este link: https://wkhtmltopdf.org/downloads.html

Descargamos la ultima version de 7z Archive (XP/2003 or later) para windows

Este archivo nos decarga lo sgte 

- bin 
-- libwkhtmltox.dll
- include 

NOTA: Importante cuando descargas el archivo el archivo dll que se descarga tiene este nombre wkhtmltox se tiene que renombrar a libwkhtmltox.dll esto para que funcione

## 2. Instalacion de DinkToPdf
Esta libreria es la pirncipal y utiliza por debajo wkhtmltox 

Vamos al nuget link: https://www.nuget.org/packages/DinkToPdf

Comando a ejecutar en el proyeco: dotnet add package DinkToPdf --version 1.0.8