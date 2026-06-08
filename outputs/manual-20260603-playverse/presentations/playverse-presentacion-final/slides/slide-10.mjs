import { C, bg, footer, kicker, panel, text, title } from "./theme.mjs";

const done = [
  "Frontend Angular con pantallas publicas y panel privado.",
  "Backend ASP.NET Core con controladores, servicios y JWT.",
  "Base SQL Server con catalogo, usuarios, roles y comunidad.",
  "Manual tecnico, manual de usuario y presentacion de exposicion.",
];

const next = [
  "Ejecutar backend durante la demo para poblar metricas reales.",
  "Completar datos de prueba y capturas finales del panel.",
  "Agregar pruebas automatizadas y validaciones de borde.",
  "Pulir despliegue con variables seguras en Render.",
];

export async function slide10(presentation, ctx) {
  const slide = presentation.slides.add();
  bg(slide, ctx, C.paper);
  kicker(slide, ctx, "Conclusiones", { textColor: C.green2, color: C.magenta });
  title(slide, ctx, "PlayVerse deja una base funcional y una ruta clara de mejora.", { color: C.darkText, y: 84, w: 880, size: 40, h: 106 });

  panel(slide, ctx, 80, 210, 510, 330, { fill: C.white, line: "#D4E6D8" });
  text(slide, ctx, "Logrado", 110, 234, 240, 32, { color: C.green2, size: 26, bold: true });
  done.forEach((item, i) => text(slide, ctx, `${i + 1}. ${item}`, 118, 300 + i * 48, 420, 42, { color: C.darkText, size: 17 }));

  panel(slide, ctx, 670, 210, 510, 330, { fill: "#071B10", line: "#255D39" });
  text(slide, ctx, "Siguiente iteracion", 700, 234, 300, 32, { color: C.magenta, size: 26, bold: true });
  next.forEach((item, i) => text(slide, ctx, `${i + 1}. ${item}`, 708, 300 + i * 48, 410, 42, { color: C.white, size: 17 }));
  footer(slide, ctx, 10, "Conclusiones del equipo PlayVerse");
  return slide;
}
