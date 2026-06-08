import path from "node:path";

export const C = {
  ink: "#020C07",
  deep: "#04160C",
  panel: "#082417",
  panel2: "#102D1D",
  green: "#45F26E",
  green2: "#21B84E",
  magenta: "#FF3D8D",
  amber: "#F6C85F",
  paper: "#F2F8F4",
  white: "#FFFFFF",
  muted: "#A6BBAE",
  darkText: "#0A1B10",
  line: "#1F5C36",
};

export function bg(slide, ctx, fill = C.ink) {
  ctx.addShape(slide, { x: 0, y: 0, w: ctx.W, h: ctx.H, fill, line: ctx.line(fill, 0) });
}

export function kicker(slide, ctx, text, opts = {}) {
  const x = opts.x ?? 72;
  const y = opts.y ?? 46;
  ctx.addShape(slide, { x, y: y + 10, w: 28, h: 4, fill: opts.color ?? C.green, line: ctx.line("#00000000", 0) });
  ctx.addText(slide, {
    text: text.toUpperCase(),
    x: x + 40,
    y,
    w: opts.w ?? 360,
    h: 28,
    fontSize: 13,
    color: opts.textColor ?? C.green,
    bold: true,
    valign: "middle",
  });
}

export function title(slide, ctx, text, opts = {}) {
  return ctx.addText(slide, {
    text,
    x: opts.x ?? 72,
    y: opts.y ?? 86,
    w: opts.w ?? 820,
    h: opts.h ?? 120,
    fontSize: opts.size ?? 44,
    color: opts.color ?? C.white,
    bold: true,
    typeface: ctx.fonts.title,
    insets: { left: 0, right: 0, top: 0, bottom: 0 },
  });
}

export function text(slide, ctx, value, x, y, w, h, opts = {}) {
  return ctx.addText(slide, {
    text: value,
    x,
    y,
    w,
    h,
    fontSize: opts.size ?? 19,
    color: opts.color ?? C.white,
    bold: opts.bold ?? false,
    typeface: opts.typeface ?? ctx.fonts.body,
    valign: opts.valign ?? "top",
    align: opts.align ?? "left",
    insets: opts.insets ?? { left: 0, right: 0, top: 0, bottom: 0 },
  });
}

export function panel(slide, ctx, x, y, w, h, opts = {}) {
  return ctx.addShape(slide, {
    x,
    y,
    w,
    h,
    fill: opts.fill ?? C.panel,
    line: ctx.line(opts.line ?? C.line, opts.lineWidth ?? 1),
  });
}

export function chip(slide, ctx, value, x, y, opts = {}) {
  const w = opts.w ?? Math.max(90, value.length * 8 + 26);
  panel(slide, ctx, x, y, w, opts.h ?? 32, { fill: opts.fill ?? "#0D331D", line: opts.line ?? "#2A6B40" });
  text(slide, ctx, value, x + 12, y + 6, w - 24, 20, {
    size: opts.size ?? 12,
    color: opts.color ?? C.white,
    bold: opts.bold ?? true,
    valign: "middle",
  });
}

export function metric(slide, ctx, value, label, x, y, w, opts = {}) {
  panel(slide, ctx, x, y, w, 94, { fill: opts.fill ?? C.panel, line: opts.line ?? C.line });
  text(slide, ctx, value, x + 18, y + 12, w - 36, 38, { size: 29, color: opts.valueColor ?? C.green, bold: true });
  text(slide, ctx, label, x + 18, y + 54, w - 36, 28, { size: 13, color: opts.labelColor ?? C.muted });
}

export function node(slide, ctx, value, x, y, w, h, opts = {}) {
  panel(slide, ctx, x, y, w, h, { fill: opts.fill ?? C.panel, line: opts.line ?? C.line });
  text(slide, ctx, value, x + 16, y + 12, w - 32, h - 24, {
    size: opts.size ?? 15,
    color: opts.color ?? C.white,
    bold: opts.bold ?? true,
    valign: "middle",
  });
}

export function connector(slide, ctx, x1, y1, x2, y2, opts = {}) {
  const horizontal = Math.abs(x2 - x1) >= Math.abs(y2 - y1);
  const x = Math.min(x1, x2);
  const y = Math.min(y1, y2);
  const w = horizontal ? Math.abs(x2 - x1) : 2;
  const h = horizontal ? 2 : Math.abs(y2 - y1);
  ctx.addShape(slide, { x, y, w, h, fill: opts.color ?? C.green, line: ctx.line("#00000000", 0) });
  if (opts.arrow) {
    text(slide, ctx, ">", x2 - 10, y2 - 13, 20, 20, { size: 16, color: opts.color ?? C.green, bold: true, align: "center" });
  }
}

export async function image(slide, ctx, filename, x, y, w, h, opts = {}) {
  const imagePath = path.join(ctx.assetDir, filename);
  await ctx.addImage(slide, {
    path: imagePath,
    x,
    y,
    w,
    h,
    fit: opts.fit ?? "cover",
    alt: opts.alt ?? filename,
  });
  if (opts.frame !== false) {
    ctx.addShape(slide, { x, y, w, h, fill: "#00000000", line: ctx.line(opts.line ?? "#2C6A41", opts.lineWidth ?? 1) });
  }
}

export function footer(slide, ctx, n, source = "Fuente: repositorio local PlayVerse") {
  text(slide, ctx, source, 72, 688, 820, 18, { size: 9, color: source.includes("local") ? "#76917E" : "#687A70" });
  text(slide, ctx, String(n).padStart(2, "0"), 1160, 682, 42, 22, { size: 13, color: C.green, bold: true, align: "right" });
}
