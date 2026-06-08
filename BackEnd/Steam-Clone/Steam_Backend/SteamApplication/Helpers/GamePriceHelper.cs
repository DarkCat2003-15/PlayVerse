using SteamDomain.Database.SqlServer.Entities;

namespace SteamApplication.Helpers
{
    public static class GamePriceHelper
    {
        public static Offer? GetActiveOffer(Game game, DateTime now)
        {
            return game.Offers
                .Where(offer =>
                    offer.StartDate.HasValue &&
                    offer.EndDate.HasValue &&
                    offer.StartDate.Value <= now &&
                    offer.EndDate.Value >= now)
                .OrderByDescending(offer => offer.Discount ?? 0)
                .FirstOrDefault();
        }

        public static decimal ResolveCurrentPrice(Game game, DateTime now)
        {
            var basePrice = game.Price ?? 0m;
            var activeOffer = GetActiveOffer(game, now);

            if (activeOffer?.Discount is not int discount || discount <= 0)
            {
                return basePrice;
            }

            if (discount > 100)
            {
                discount = 100;
            }

            var finalPrice = basePrice - (basePrice * discount / 100m);
            return decimal.Round(finalPrice, 2, MidpointRounding.AwayFromZero);
        }
    }
}
