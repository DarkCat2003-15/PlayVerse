import { C, bg, footer, kicker, metric, panel, text, title } from "./theme.mjs";

export async function slide02(presentation, ctx) {
  const slide = presentation.slides.add();
  bg(slide, ctx, C.paper);
  kicker(slide, ctx, "Problema", { textColor: C.green2, color: C.magenta });
  title(slide, ctx, "Administrar una tienda de videojuegos requiere mas que publicar una lista.", {
    color: C.darkText,
    size: 38,
    w: 600,
    y: 88,
    h: 130,
  });
  text(slide, ctx, "El sistema debe conectar catalogo, usuarios, roles, compras, comunidad y seguridad sin duplicar reglas entre pantallas.", 78, 250, 540, 82, { color: "#34463A", size: 21 });
  panel(slide, ctx, 78, 412, 540, 118, { fill: C.white, line: "#CFE2D4" });
  text(slide, ctx, "Sin una arquitectura ordenada, cada cambio en juegos, ofertas o permisos se vuelve dificil de mantener y demostrar.", 110, 438, 470, 66, { color: C.darkText, size: 20, bold: true });
  metric(slide, ctx, "Visitante", "Descubre juegos, categorias y contacto sin iniciar sesion.", 710, 106, 420, { fill: C.white, line: "#D8E7DC", valueColor: C.magenta, labelColor: "#4C5B52" });
  metric(slide, ctx, "Admin / Dev", "Mantiene catalogo, maestros, contenido y roles.", 710, 246, 420, { fill: C.white, line: "#D8E7DC", valueColor: C.green2, labelColor: "#4C5B52" });
  metric(slide, ctx, "Usuario", "Gestiona wishlist, libreria, reviews, sesiones y amigos.", 710, 386, 420, { fill: C.white, line: "#D8E7DC", valueColor: "#9A6B00", labelColor: "#4C5B52" });
  footer(slide, ctx, 2, "Fuente: estructura funcional del frontend y backend");
  return slide;
}
