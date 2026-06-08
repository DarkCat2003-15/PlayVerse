import { C, bg, footer, image, kicker, panel, text, title } from "./theme.mjs";

export async function slide06(presentation, ctx) {
  const slide = presentation.slides.add();
  bg(slide, ctx, C.paper);
  kicker(slide, ctx, "Base de datos", { textColor: C.green2, color: C.magenta });
  title(slide, ctx, "El modelo de datos sostiene catalogo, acceso y actividad del jugador.", { color: C.darkText, y: 82, w: 920, size: 39, h: 100 });
  panel(slide, ctx, 64, 174, 790, 492, { fill: C.white, line: "#D4E6D8" });
  await image(slide, ctx, "relaciones-entidades.png", 84, 194, 750, 452, { fit: "contain", line: "#D4E6D8" });
  panel(slide, ctx, 886, 206, 250, 110, { fill: "#FFFFFF", line: "#D4E6D8" });
  text(slide, ctx, "Catalogo", 910, 226, 200, 26, { color: C.magenta, size: 22, bold: true });
  text(slide, ctx, "Games, Genres, Developers, Publishers, Offers, DLC y Achievements.", 910, 264, 200, 46, { color: "#45584C", size: 14 });
  panel(slide, ctx, 886, 342, 250, 110, { fill: "#FFFFFF", line: "#D4E6D8" });
  text(slide, ctx, "Accesos", 910, 362, 200, 26, { color: C.green2, size: 22, bold: true });
  text(slide, ctx, "Users, Roles, Permissions, UserRoles y Status.", 910, 400, 200, 44, { color: "#45584C", size: 14 });
  panel(slide, ctx, 886, 478, 250, 110, { fill: "#FFFFFF", line: "#D4E6D8" });
  text(slide, ctx, "Comunidad", 910, 498, 200, 26, { color: "#9A6B00", size: 22, bold: true });
  text(slide, ctx, "Wishlist, Library, Reviews, Sessions, Friends y UserAchievements.", 910, 536, 200, 46, { color: "#45584C", size: 14 });
  footer(slide, ctx, 6, "Fuente: PLAYVERSE_DDL.sql y diagrama de entidades");
  return slide;
}
