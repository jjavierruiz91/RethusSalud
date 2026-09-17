# Guía de auditoría responsive móvil — bugs reales encontrados y su patrón de solución

Basada en una auditoría real hecha con el navegador emulando 375px (iPhone) sobre una app
ASP.NET Core MVC + Bootstrap 5. Los tres problemas encontrados no son específicos de este
proyecto — son patrones que aparecen en cualquier layout con Bootstrap grid, flexbox o CSS
Grid al pasar de escritorio a móvil. Sirve como checklist para auditar otros proyectos.

## Cómo se hizo la auditoría

1. Levantar el sitio localmente.
2. Emular viewport móvil (375×812) en el navegador.
3. Recorrer cada vista principal tomando screenshot, prestando atención a: elementos que se
   parten en dos filas, contenido que desaparece sin explicación, formularios que quedan
   fuera de la pantalla inicial.
4. Para cada anomalía visual, inspeccionar el DOM/CSS calculado (`getBoundingClientRect()`,
   `getComputedStyle()`) antes de tocar nada — la causa casi nunca es obvia a simple vista.

## Bug 1 — Elementos en fila que se parten en dos líneas desordenadas

**Síntoma:** un indicador de pasos (1-2-3-4-5) que en escritorio se ve en una fila limpia,
en móvil se parte en 3+2, con las líneas conectoras desalineadas.

**Causa real:** el media query que oculta las etiquetas de texto en pantallas chicas
**olvidó reducir el `min-width` del contenedor de cada paso**. El elemento seguía
reservando el ancho completo (pensado para texto + número) aunque el texto ya no estuviera
visible, así que los N elementos no cabían en una fila.

```css
/* Antes: solo ocultaba el texto, pero el contenedor seguía ancho */
@media (max-width: 575.98px) {
  .wizard-step-label { display: none; }
}

/* Después: también se reduce el min-width del contenedor */
@media (max-width: 575.98px) {
  .wizard-step { min-width: 0; }
  .wizard-step-label { display: none; }
  .wizard-step-number { width: 30px; height: 30px; font-size: 0.8rem; }
  .wizard-step-connector { width: 14px; }
}
```

**Cómo detectarlo en otro proyecto:** cualquier fila de N elementos con `flex-wrap: wrap`
donde un media query oculta *contenido* pero no ajusta el `min-width`/`flex-basis` del
elemento que lo contiene.

## Bug 2 — Una sección entera desaparece (altura 0) sin ningún error visible

**Síntoma:** una tabla de datos completa (con filas reales en el HTML) simplemente no se ve
en móvil — no hay error, no hay mensaje, el espacio ni siquiera está en blanco, colapsa a
0px de alto.

**Causa real:** un patrón de "altura fija con scroll interno" (`display:flex; flex-direction:
column;` en cascada, con `flex: 1 1 auto; min-height: 0;` en el elemento que debe llenar el
espacio restante). Este patrón funciona bien en escritorio porque los filtros/encabezados
caben en poco espacio y sobra altura para la tabla. En móvil, como los filtros se apilan
verticalmente (en vez de estar en una fila), consumen **toda** la altura disponible antes de
llegar al elemento con `min-height: 0` — que, fiel a su nombre, se encoge a nada en vez de
desbordar.

```css
/* El patrón que colapsa en móvil */
.panel-compacto {
  display: flex;
  flex-direction: column;
  flex: 1 1 auto;
  min-height: 0;   /* <- permite que se encoja a 0 si no hay espacio */
  overflow: hidden;
}
.panel-compacto .tabla-scroll {
  flex: 1 1 auto;
  min-height: 0;
}

/* La solución: en móvil, abandonar el scroll interno y dejar que la página
   completa haga scroll normal, con cada sección en su altura natural */
@media (max-width: 767.98px) {
  .panel-compacto,
  .panel-compacto .tabla-scroll {
    flex: none;
    min-height: 0;
    max-height: none;
    overflow: visible;
  }
}
```

**Cómo detectarlo en otro proyecto:** si algo "desaparece" en móvil sin error visible,
inspecciona `getBoundingClientRect()` del contenedor — si da `height: 0` con hijos reales
adentro, busca un `min-height: 0` en la cadena de ancestros flex. Es la firma exacta de este
bug.

## Bug 3 — Un formulario queda fuera de la pantalla inicial

**Síntoma:** una página con un panel informativo grande (hero, imagen, texto) seguido del
formulario real. En escritorio se ven ambos lado a lado. En móvil, el panel informativo
empuja el formulario **debajo del fold** — el usuario tiene que hacer scroll antes de ver
siquiera un campo de texto.

**Causa real:** un CSS Grid de 2 columnas que colapsa a 1 columna en móvil, pero mantiene el
**orden del HTML** — si el panel informativo aparece primero en el markup (común, porque en
desktop va a la izquierda), en móvil también aparece primero, empujando el formulario abajo.

```css
@media (max-width: 991.98px) {
  .grid-2-columnas { grid-template-columns: 1fr; }

  /* La solución: usar `order` para que el formulario aparezca primero
     visualmente en móvil, sin tocar el HTML */
  .panel-informativo { order: 2; }
  .panel-formulario  { order: 1; }
}
```

**Cómo detectarlo en otro proyecto:** cualquier layout de 2 columnas que colapse a 1 en
móvil — pregúntate cuál de las dos partes es *accionable* (un formulario, un CTA) y
verifica que quede primera visualmente, sin importar el orden del HTML.

## Checklist para auditar otro proyecto

- [ ] Recorrer cada vista principal en viewport 375px, no asumir que "Bootstrap ya lo
      resuelve solo"
- [ ] Buscar filas de elementos (`flex-wrap`) donde un media query oculta contenido sin
      ajustar `min-width`
- [ ] Buscar layouts con `min-height: 0` en cascada — son los candidatos a colapsar a 0
      cuando el contenido de arriba crece (como pasa al apilar filtros en móvil)
- [ ] Buscar layouts de 2 columnas (grid o flex) donde el orden del HTML no coincide con
      la prioridad real para el usuario en móvil — usar `order` para corregir sin tocar
      el markup
- [ ] Verificar formularios de login/registro específicamente — son los más propensos al
      Bug 3 por tener paneles decorativos grandes al lado

---
*Guía basada en una auditoría real (RethusSalud, sep. 2026).*
