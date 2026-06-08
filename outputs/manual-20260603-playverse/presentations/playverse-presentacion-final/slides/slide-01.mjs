import { C, bg, chip, footer, image, kicker, text, title } from "./theme.mjs";

export async function slide01(presentation, ctx) {
  const slide = presentation.slides.add();
  bg(slide, ctx);
  await image(slide, ctx, "playverse-home.png", 660, 94, 500, 282, { line: "#1F5C36" });
  await image(slide, ctx, "playverse-catalogo.png", 760, 390, 400, 226, { line: "#1F5C36" });
  await image(slide, ctx, "logo.png", 78, 86, 156, 116, { fit: "contain", frame: false });
  kicker(slide, ctx, "Proyecto final", { y: 52 });
  title(slide, ctx, "PlayVerse", { y: 224, size: 70, w: 560, h: 86 });
  text(slide, ctx, "Tu universo de videojuegos en un solo lugar.", 80, 322, 520, 38, { size: 24, color: "#DFFFE8" });
  text(slide, ctx, "Tienda academica de videojuegos con catalogo publico, acceso privado y administracion del contenido.", 80, 376, 520, 70, { size: 20, color: C.muted });
  chip(slide, ctx, "Juan Buenaño", 80, 482, { w: 150 });
  chip(slide, ctx, "Jaime Carriel", 244, 482, { w: 150 });
  chip(slide, ctx, "Angular + .NET + SQL Server", 80, 526, { w: 248, color: C.green });
  footer(slide, ctx, 1, "Presentacion para exposicion - PlayVerse");
  return slide;
}
