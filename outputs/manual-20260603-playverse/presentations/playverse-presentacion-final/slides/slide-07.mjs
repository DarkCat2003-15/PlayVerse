import { C, bg, footer, image, kicker, panel, text, title } from "./theme.mjs";

export async function slide07(presentation, ctx) {
  const slide = presentation.slides.add();
  bg(slide, ctx);
  kicker(slide, ctx, "Pantallas publicas");
  title(slide, ctx, "La interfaz publica ya presenta identidad, catalogo y contacto.", { y: 82, w: 850, size: 40, h: 84 });
  await image(slide, ctx, "playverse-home.png", 66, 184, 538, 302, { line: "#285E3A" });
  await image(slide, ctx, "playverse-catalogo.png", 666, 184, 538, 302, { line: "#285E3A" });
  panel(slide, ctx, 66, 514, 538, 62, { fill: "#071B10", line: "#255D39" });
  text(slide, ctx, "Inicio: logo, slogan y acceso directo a juegos o inicio de sesion.", 90, 534, 486, 30, { size: 16, color: C.muted });
  panel(slide, ctx, 666, 514, 538, 62, { fill: "#071B10", line: "#255D39" });
  text(slide, ctx, "Catalogo: filtros por categoria y juegos destacados para visitantes.", 690, 534, 486, 30, { size: 16, color: C.muted });
  footer(slide, ctx, 7, "Capturas locales del frontend PlayVerse");
  return slide;
}
