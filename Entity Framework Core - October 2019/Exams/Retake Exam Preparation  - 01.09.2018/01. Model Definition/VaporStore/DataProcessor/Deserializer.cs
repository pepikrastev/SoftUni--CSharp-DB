namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enum;
    using VaporStore.DataProcessor.ImportDto;

    public static class Deserializer
	{
		public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
            var gamesDtos = JsonConvert.DeserializeObject<ImprtGameJsonDto[]>(jsonString);
            var games = new HashSet<Game>();
            var sb = new StringBuilder();

            foreach (var gameDto in gamesDtos)
            {
                if (!IsValid(gameDto) || gameDto.Tags.Count < 1)
                {
                    sb.AppendLine("Invalid Data");
                }
                else
                {
                    var game = new Game
                    {
                        Name = gameDto.Name,
                        Price = gameDto.Price,
                        ReleaseDate = DateTime.ParseExact(gameDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                    };

                    //•If a developer / genre / tag with that name doesn’t exist, create it.
                    var developer = context.Developers.FirstOrDefault(d => d.Name == gameDto.Developer);
                    if (developer == null)
                    {
                        developer = new Developer { Name = gameDto.Developer };
                        context.Developers.Add(developer);
                        context.SaveChanges();
                    }
                    //or
                    //var developer = developers.Any(d => d.Name == gameDto.Developer)
                    //    ? developers.FirstOrDefault(d => d.Name == gameDto.Developer)
                    //    : new Developer { Name = gameDto.Developer, };
                    game.Developer = developer;

                    var genre = context.Genres.FirstOrDefault(g => g.Name == gameDto.Genre);
                    if (genre == null)
                    {
                        genre = new Genre { Name = gameDto.Genre };
                        context.Genres.Add(genre);
                        context.SaveChanges();
                    }
                    game.Genre = genre;

                    foreach (var tagDto in gameDto.Tags)
                    {
                        var tag = context.Tags.FirstOrDefault(t => t.Name == tagDto);
                        if (tag == null)
                        {
                            tag = new Tag { Name = tagDto };
                            context.Tags.Add(tag);
                            context.SaveChanges();
                        }

                        game.GameTags.Add(new GameTag {Game = game, Tag = tag });
                    };
                    //
                    games.Add(game);
                    sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");
                }
            }

            context.Games.AddRange(games);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

		public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
            var userDtos = JsonConvert.DeserializeObject<ImportUsersJsonDto[]>(jsonString);
            var users = new HashSet<User>();
            var sb = new StringBuilder();

            foreach (var userDto in userDtos)
            {
                if (!IsValid(userDto) || userDto.Cards.Length < 1)
                {
                    sb.AppendLine("Invalid Data");
                }
                else
                {
                    var user = new User
                    {
                        FullName = userDto.FullName,
                        Username = userDto.Username,
                        Email = userDto.Email
                    };

                    foreach (var cardDto in userDto.Cards)
                    {
                        if (!IsValid(cardDto))
                        {
                            sb.AppendLine("Invalid Data");
                            continue;
                        }

                        var card = new Card
                        {
                            Number = cardDto.Number,
                            Cvc = cardDto.Cvc,
                            Type = (CardType)Enum.Parse(typeof(CardType), cardDto.Type)
                        };
                        user.Cards.Add(card);
                    }
                    users.Add(user);
                    sb.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards"); 
                }
            }

            context.Users.AddRange(users);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
            var xmlSerializer = new XmlSerializer(typeof(ImportPurchaseXmlDto[]), new XmlRootAttribute("Purchases"));
            var purchaseDtos = (ImportPurchaseXmlDto[])xmlSerializer.Deserialize(new StringReader(xmlString));

            List<Purchase> purchases = new List<Purchase>();

            StringBuilder sb = new StringBuilder();

            foreach (var purchaseDto in purchaseDtos)
            {
                //var isValidGame = context.Games.Any(g => g.Name == purchaseDto.Title);
                //var isValidType = Enum.IsDefined(typeof(PurchaseType), purchaseDto.Type);
                //var isValidCard = context.Cards.Any(c => c.Number == purchaseDto.CardNumber);

                if (!IsValid(purchaseDto) /*|| !isValidGame || !isValidType || !isValidCard*/)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var purchase = new Purchase
                {
                    Game = context.Games.First(g => g.Name == purchaseDto.Title),
                    Type = (PurchaseType)Enum.Parse(typeof(PurchaseType), purchaseDto.Type),
                    ProductKey = purchaseDto.Key,
                    Card = context.Cards.First(c => c.Number == purchaseDto.CardNumber),
                    Date = DateTime.ParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
                };
                purchases.Add(purchase);
                sb.AppendLine($"Imported {purchase.Game.Name} for {purchase.Card.User.Username}");
            }

            context.Purchases.AddRange(purchases);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}