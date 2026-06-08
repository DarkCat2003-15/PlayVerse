import { C, bg, connector, footer, kicker, node, panel, text, title } from "./theme.mjs";

const steps = [
  ["01", "Inicio", "Presentar identidad y objetivo."],
  ["02", "Catalogo", "Mostrar filtros y juegos."],
  ["03", "Login", "Explicar acceso seguro."],
  ["04", "Admin", "Crear/listar/editar/eliminar."],
  ["05", "Cierre", "Arquitectura y conclusiones."],
];

export async function slide09(presentation, ctx) {
  const slide = presentation.slides.add();
  bg(slide, ctx);
  kicker(slide, ctx, "Demo");
  title(slide, ctx, "La exposicion puede mostrarse como un recorrido completo.", { y: 84, w: 880, size: 41, h: 106 });

  steps.forEach((step, i) => {
    const x = 78 + i * 236;
    node(slide, ctx, `${step[0]}\n${step[1]}\n${step[2]}`, x, 246, 178, 146, {
      fill: i === 2 ? "#102033" : i % 2 ? "#0B3019" : "#092316",
      color: i === 2 ? "#FFD6E7" : C.white,
      size: 15,
    });
    if (i < steps.length - 1) connector(slide, ctx, x + 178, 318, x + 222, 318, { arrow: true, color: i === 1 ? C.magenta : C.green });
  });

  panel(slide, ctx, 170, 484, 940, 76, { fill: "#071B10", line: "#255D39" });
  text(slide, ctx, "Guion recomendado: empezar con el problema, mostrar pantallas, conectar con arquitectura y terminar con aprendizajes del equipo.", 206, 508, 868, 34, { size: 19, color: C.muted, bold: true });
  footer(slide, ctx, 9, "Fuente: flujo de demostracion del sistema");
  return slide;
}
