
namespace MTCG.Logic.Infrastructure
{
    using MTCG.Models;
    using Npgsql;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class CardFactory
    {
        public IEnumerable<Type> CardTypes { get; init; }

        public CardFactory()
        {
            CardTypes = GetAllCardTypes();
        }

        public Card GetCardFromDataReader(NpgsqlDataReader reader)
        {
            int cardId = reader.GetInt32(reader.GetOrdinal("card_id"));
            string name = reader.GetString(reader.GetOrdinal("name"));
            double damage = reader.GetDouble(reader.GetOrdinal("damage"));
            ElementTyp elementTyp = ConvertCharToElementTyp(reader.GetChar(reader.GetOrdinal("element_type")));

            //char card_type = reader.GetChar(reader.GetOrdinal("card_type"));
            string category = reader.GetString(reader.GetOrdinal("category_id"));

            Card instance = GetCardInstanceFromCategory(category);
            instance.Id = cardId;
            instance.Name = name;
            instance.Damage = damage;
            instance.ElementTyp = elementTyp;

            return instance;
        }

        public ElementTyp ConvertCharToElementTyp(char type)
        {
            return type switch
            {
                'N' => ElementTyp.Normal,
                'F' => ElementTyp.Fire,
                'W' => ElementTyp.Water,
                _ => throw new Exception($"Unrecognized Elementype {type}")
            };
        }

        private IEnumerable<Type> GetAllCardTypes()
        {
            Assembly current = Assembly.GetExecutingAssembly();
            return current.GetTypes().Where(type => type.IsAssignableTo(typeof(Card)) && !type.IsAbstract);
        }

        private Card GetCardInstanceFromCategory(string category)
        {
            Type type = CardTypes.FirstOrDefault(type => type.Name == category);
            return (Card)Activator.CreateInstance(type);
        }
    }
}
