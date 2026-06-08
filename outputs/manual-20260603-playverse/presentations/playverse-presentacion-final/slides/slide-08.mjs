import { C, bg, footer, image, kicker, panel, text, title } from "./theme.mjs";

export async function slide08(presentation, ctx) {
  const slide = presentation.slides.add();
  bg(slide, ctx, C.paper);
  kicker(slide, ctx, "Panel privado", { textColor: C.green2, color: C.magenta });
  title(slide, ctx, "El panel administrativo concentra catalogo, contenido y accesos.", { color: C.darkText, y: 82, w: 890, size: 39, h: 82 });
  await image(slide, ctx, "playverse-admin.png", 66, 178, 538, 302, { line: "#D4E6D8" });
  await image(slide, ctx, "playverse-admin-catalogo-form.png", 666, 178, 538, 302, { line: "#D4E6D8" });
  panel(slide, ctx, 66, 508, 538, 68, { fill: C.white, line: "#D4E6D8" });
  text(slide, ctx, "Resumen: metricas, navegacion lateral y acciones para actualizar datos del backend.", 90, 524, 486, 42, { size: 15, color: C.darkText });
  panel(slide, ctx, 666, 508, 538, 68, { fill: C.white, line: "#D4E6D8" });
  text(slide, ctx, "Catalogo: crear juegos y maestros; tambien permite listar, editar y eliminar registros.", 690, 524, 486, 42, { size: 15, color: C.darkText });
  footer(slide, ctx, 8, "Capturas locales con sesion de demostracion");
  return slide;
}
