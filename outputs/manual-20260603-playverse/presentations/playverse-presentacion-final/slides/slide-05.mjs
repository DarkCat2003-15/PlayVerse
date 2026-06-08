import { C, bg, connector, footer, kicker, node, panel, text, title } from "./theme.mjs";

export async function slide05(presentation, ctx) {
  const slide = presentation.slides.add();
  bg(slide, ctx);
  kicker(slide, ctx, "Arquitectura");
  title(slide, ctx, "Angular consume una API por capas antes de llegar a SQL Server.", { y: 84, w: 910, size: 41, h: 112 });

  node(slide, ctx, "Angular UI\nPublic/Admin\nMaterial", 76, 260, 170, 96, { fill: "#0E3A20" });
  connector(slide, ctx, 246, 308, 306, 308, { arrow: true });
  node(slide, ctx, "Core Services\nHttpClient\nAuth Guard", 308, 260, 170, 96, { fill: "#0B2A19" });
  connector(slide, ctx, 478, 308, 538, 308, { arrow: true });
  node(slide, ctx, "ASP.NET API\nControllers\nCORS", 540, 260, 170, 96, { fill: "#102033" });
  connector(slide, ctx, 710, 308, 770, 308, { arrow: true });
  node(slide, ctx, "Application\nServices\nDTOs", 772, 260, 170, 96, { fill: "#0B3019" });
  connector(slide, ctx, 942, 308, 1002, 308, { arrow: true });
  node(slide, ctx, "Domain + EF\nEntities\nSteamContext", 1004, 260, 180, 96, { fill: "#33250B", color: "#FFE6A8" });
  connector(slide, ctx, 1094, 356, 1094, 414, { arrow: true, color: C.amber });
  node(slide, ctx, "SQL Server\nPLAYVERSE", 1004, 418, 180, 72, { fill: "#3A2609", color: "#FFE6A8", size: 18 });

  panel(slide, ctx, 122, 488, 802, 72, { fill: "#071B10", line: "#255D39" });
  text(slide, ctx, "Servicios transversales: JWT, roles/permisos, Serilog, SMTP/Brevo, OpenAPI/Scalar y middleware de errores.", 146, 510, 750, 36, { size: 18, color: C.muted });
  footer(slide, ctx, 5, "Fuente: Program.cs, ServiceCollectionExtension.cs y estructura .NET");
  return slide;
}
