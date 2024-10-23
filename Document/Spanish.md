# ZCore Library Documentation

## Table of Contents
1. [Dependencies](#dependencies)
2. [GlobalCode](#globalcode)
3. [Modules](#modules)
4. [Properties](#properties)
5. [Serializables](#serializables)

---

## Dependencies

Esta sección se encarga de agregar dependencias externas, como archivos `.dll`, necesarias para el funcionamiento de la biblioteca.

## GlobalCode

Aquí se incluye el código global accesible por todos los módulos y clases de la biblioteca. 

También podrá ser usado por los proyectos que implementen ZCore como dependencia, aquí un vistazo de su contenido:

### Constants

La clase `Constants` proporciona constantes útiles, mayormente usadas para cálculos informáticos y cálculos de tiempo.

### CryptoParams

Proporciona herramientas para validar y ajustar parámetros de cifrado como el tamaño de claves y vectores de inicialización, asegurando que cumplen con los requisitos de seguridad.

#### Métodos de la Clase:

1. **CheckBlockSize**
   - Valida y ajusta el tamaño de un bloque de datos según los límites establecidos.

2. **CheckKeySize**
   - Verifica y ajusta el tamaño de la clave de cifrado según lo esperado.

3. **CheckIVLength**
   - Comprueba y ajusta el tamaño del vector de inicialización (IV) conforme a los requisitos.

4. **CipherKeySchedule**
   - Deriva una nueva clave a partir de una clave existente, utilizando un valor de sal y un algoritmo hash.

5. **InitVector**
   - Inicializa un vector de bytes a partir de una clave de cifrado, ajustándose al tamaño esperado.


## Modules

Contiene los módulos que agrupan las funcionalidades principales de la biblioteca. Cada módulo tiene un propósito específico en el sistema.

## Properties
Generado automáticamente por Visual Studio, esta sección define las propiedades del proyecto (AssemblyInfo).

## Serializables

Esta sección contiene las clases que pueden ser serializadas para su almacenamiento o transmisión.

Suelo usar el serializador JSON de ServiceStack para una mayor velocidad, aprovechando que JSON es una sintáxis fácil de leer.

---

## License
Incluir detalles de la licencia aquí.

