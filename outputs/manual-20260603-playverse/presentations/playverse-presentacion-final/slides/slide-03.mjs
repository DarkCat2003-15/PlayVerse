import { C, bg, chip, footer, kicker, panel, text, title } from "./theme.mjs";

export async function slide03(presentation, ctx) {
  const slide = presentation.slides.add();
  bg(slide, ctx);
  kicker(slide, ctx, "Solucion");
  title(slide, ctx, "PlayVerse separa la experiencia publica de la operacion privada.", { y: 84, size: 43, w: 880, h: 110 });

  const cards = [
    ["Vitrina publica", C.panel, C.green, ["Inicio", "Catalogo de juegos", "Quienes somos", "Contactanos"]],
    ["Panel administrativo", "#101B2B", C.magenta, ["Catalogo", "Contenido", "Roles y usuarios", "Cuenta"]],
    ["Experiencia usuario", "#172A13", C.amber, ["Wishlist", "Libreria", "Reviews", "Sesiones y amigos"]],
  ];
  cards.forEach((card, i) => {
    const x = 82 + i * 392;
    panel(slide, ctx, x, 210, 330, 296, { fill: card[1], line: "#245F39" });
    text(slide, ctx, card[0], x + 26, 236, 260, 32, { size: 23, bold: true, color: card[2] });
    card[3].forEach((item, idx) => text(slide, ctx, item, x + 30, 304 + idx * 38, 250, 24, { size: 19, color: C.white }));
    chip(slide, ctx, i === 0 ? "Visitantes" : i === 1 ? "Admin / Developer" : "User", x + 28, 450, { w: i === 1 ? 160 : 120, fill: "#0D331D" });
  });
  footer(slide, ctx, 3, "Fuente: rutas publicas, admin.routes y AdminDashboard");
  return slide;
}
