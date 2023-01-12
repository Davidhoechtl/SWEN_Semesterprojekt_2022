using MTCG.DAL;
using MTCG.Logic.Infrastructure.Repositories.MockUps;
using MTCG.Logic.Models.Trading;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Tests
{
    internal class TradeTests
    {
        [SetUp]
        public void Setup()
        {
            tradeOfferRepository = new MockTradeOfferRepository();

            tradeLauncher = new TradeLauncher(tradeOfferRepository, null);
        }

        [Test]
        public void Trade_BuyOfferNull()
        {
            TradingOffer offer = null;
            User buyer = new User()
            {
                Cards = new List<Card>()
                {
                    new Goblin()
                    {
                        Id = 1,
                        Category = "Goblin",
                        Name = "SmallGoblin",
                        Damage = 99,
                        ElementTyp = ElementTyp.Normal
                    }
                }
            };
            User seller = new User()
            {
                Cards = new List<Card>()
                {
                    new Troll()
                    {
                        Category = "Troll",
                        Name = "SmallTroll",
                        Damage = 30,
                        ElementTyp = ElementTyp.Normal
                    }
                }
            };

            bool result = tradeLauncher.TryBuyTradeOffer(offer, seller, buyer, buyer.Cards.First());
            
            Assert.IsFalse(result);
        }

        [Test]
        public void Trade_BuyOfferInactive()
        {
            TradingOffer offer = tradeOfferRepository.GetAllTradeOffers().First(o => o.Id == 2);
            User buyer = new User()
            {
                Cards = new List<Card>()
                {
                    new Goblin()
                    {
                        Id = 1,
                        Category = "Goblin",
                        Name = "SmallGoblin",
                        Damage = 99,
                        ElementTyp = ElementTyp.Normal
                    }
                }
            };
            User seller = new User()
            {
                Cards = new List<Card>()
                {
                    new Troll()
                    {
                        Category = "Troll",
                        Name = "SmallTroll",
                        Damage = 30,
                        ElementTyp = ElementTyp.Normal
                    }
                }
            };

            bool result = tradeLauncher.TryBuyTradeOffer(offer, seller, buyer, buyer.Cards.First());

            Assert.IsFalse(result);
        }

        [Test]
        public void Trade_BuyOffer_Ok()
        {
            TradingOffer offer = tradeOfferRepository.GetAllTradeOffers().First(o => o.Id == 1);
            User buyer = new User()
            {
                Cards = new List<Card>()
                {
                    new Goblin()
                    {
                        Id = 1,
                        Category = "WaterGoblin",
                        Name = "Goblin",
                        Damage = 99,
                        ElementTyp = ElementTyp.Water
                    }
                }
            };
            User seller = new User()
            {
                Cards = new List<Card>()
                {
                    new Troll()
                    {
                        Category = "Troll",
                        Name = "SmallTroll",
                        Damage = 30,
                        ElementTyp = ElementTyp.Normal
                    }
                }
            };

            bool result = tradeLauncher.TryBuyTradeOffer(offer, seller, buyer, buyer.Cards.First());

            Assert.IsTrue(result);
        }

        [Test]
        public void Trade_BuyOffer_NotEnoughDamage()
        {
            TradingOffer offer = tradeOfferRepository.GetAllTradeOffers().First(o => o.Id == 1);
            User buyer = new User()
            {
                Cards = new List<Card>()
                {
                    new Goblin()
                    {
                        Id = 1,
                        Category = "WaterGoblin",
                        Name = "Goblin",
                        Damage = 10,
                        ElementTyp = ElementTyp.Water
                    }
                }
            };
            User seller = new User()
            {
                Cards = new List<Card>()
                {
                    new Troll()
                    {
                        Category = "Troll",
                        Name = "SmallTroll",
                        Damage = 30,
                        ElementTyp = ElementTyp.Normal
                    }
                }
            };

            bool result = tradeLauncher.TryBuyTradeOffer(offer, seller, buyer, buyer.Cards.First());

            Assert.IsFalse(result);
        }

        MockTradeOfferRepository tradeOfferRepository;
        TradeLauncher tradeLauncher;
    }
}
