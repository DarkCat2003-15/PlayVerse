import { C, bg, chip, footer, kicker, panel, text, title } from "./theme.mjs";

const groups = [
  ["Frontend", C.magenta, ["Angular 21", "Angular Material", "TypeScript 5.9", "RxJS", "Vitest"]],
  ["Backend", C.green2, ["ASP.NET Core .NET 9", "API REST", "JWT Bearer", "Serilog", "Scalar / OpenAPI"]],
  ["Datos", "#9A6B00", ["SQL Server 2022", "Entity Framework Core", "PLAYVERSE_DDL", "SteamContext", "Repositorios"]],
  ["Demo / despliegue", "#365DCC", ["Postman", "Dockerfile", "Render config", "SMTP Brevo", "CORS localhost"]],
];

export async function slide04(presentation, ctx) {
  const slide = presentation.slides.add();
  bg(slide, ctx, C.paper);
  kicker(slide, ctx, "Tecnologias", { textColor: C.green2, color: C.magenta });
  title(slide, ctx, "El stack cubre interfaz, API, seguridad, datos y despliegue.", { color: C.darkText, y: 88, w: 900, size: 40, h: 105 });

  groups.forEach((group, i) => {
    const x = 74 + i * 298;
    panel(slide, ctx, x, 216, 260, 330, { fill: C.white, line: "#D2E4D8" });
    text(slide, ctx, group[0], x + 22, 240, 220, 30, { size: 22, bold: true, color: group[1] });
    group[2].forEach((item, idx) => chip(slide, ctx, item, x + 22, 300 + idx * 42, {
      w: 216,
      fill: "#EEF6F0",
      line: "#D8E7DC",
      color: C.darkText,
      size: 12,
    }));
  });
  footer(slide, ctx, 4, "Fuente: package.json, csproj, README y render.yaml");
  return slide;
}
