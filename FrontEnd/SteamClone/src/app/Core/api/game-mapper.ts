import { GameCard } from './models/catalog.interface';
import { Product } from './models/product.interface';

const knownArtwork: Record<string, string> = {
  'counter-strike 2':
    'https://shared.cloudflare.steamstatic.com/store_item_assets/steam/apps/730/header.jpg',
  'codex arena':
    'https://shared.cloudflare.steamstatic.com/store_item_assets/steam/apps/730/header.jpg',
  'dota 2':
    'https://shared.cloudflare.steamstatic.com/store_item_assets/steam/apps/570/header.jpg',
  'elden ring':
    'https://shared.cloudflare.steamstatic.com/store_item_assets/steam/apps/1245620/header.jpg',
  'hades':
    'https://shared.cloudflare.steamstatic.com/store_item_assets/steam/apps/1145360/header.jpg',
  'stardew valley':
    'https://shared.cloudflare.steamstatic.com/store_item_assets/steam/apps/413150/header.jpg',
};

const fallbackArtwork = [
  'https://shared.cloudflare.steamstatic.com/store_item_assets/steam/apps/1174180/header.jpg',
  'https://shared.cloudflare.steamstatic.com/store_item_assets/steam/apps/292030/header.jpg',
  'https://shared.cloudflare.steamstatic.com/store_item_assets/steam/apps/1086940/header.jpg',
  'https://shared.cloudflare.steamstatic.com/store_item_assets/steam/apps/2050650/header.jpg',
];

export function artworkForTitle(title: string): string {
  const normalizedTitle = title.trim().toLowerCase();
  const known = knownArtwork[normalizedTitle];

  if (known) {
    return known;
  }

  const hash = [...normalizedTitle].reduce((total, char) => total + char.charCodeAt(0), 0);
  return fallbackArtwork[hash % fallbackArtwork.length];
}

export function gameCardToProduct(game: GameCard): Product {
  const tags = game.genres.length
    ? game.genres.slice(0, 3)
    : [game.developerName || game.publisherName || 'PC'];

  return {
    id: game.gameId,
    title: game.title,
    description: game.description || 'Juego disponible en el catalogo PlayVerse.',
    price: Number(game.finalPrice ?? game.basePrice ?? 0),
    category: game.genres[0] ?? 'General',
    thumbnail: game.imageUrl?.trim() || artworkForTitle(game.title),
    rating: game.hasActiveOffer ? `${game.discountPercentage}% descuento` : 'Disponible',
    tags,
  };
}
