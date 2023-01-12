
namespace MTCG.Logic.Infrastructure
{
    using MTCG.Models;
    using Newtonsoft.Json.Linq;
    using Npgsql;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.PortableExecutable;
    using System.Text.Json.Nodes;

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
            instance.Category = category;
            instance.Name = name;
            instance.Damage = damage;
            instance.ElementTyp = elementTyp;

            return instance;
        }

        public Card GetCardFromJObject(JObject jObject)
        {
            int cardId = jObject.Value<int>("Id");
            string name = jObject.Value<string>("Name");
            double damage = jObject.Value<double>("Damage");
            ElementTyp elementTyp = ConvertCharToElementTyp(jObject.Value<string>("ElementTyp").First());

            //char card_type = reader.GetChar(reader.GetOrdinal("card_type"));
            string category = jObject["Category"].Value<string>();

            Card instance = GetCardInstanceFromCategory(category);
            instance.Id = cardId;
            instance.Category = category;
            instance.Name = name;
            instance.Damage = damage;
            instance.ElementTyp = elementTyp;

            return instance;
        }

        public ElementTyp ConvertCharToElementTyp(char type)
        {
            return type.ToString().ToLower() switch
            {
                "n" => ElementTyp.Normal,
                "f" => ElementTyp.Fire,
                "w" => ElementTyp.Water,
                "0" => ElementTyp.Normal,
                "1" => ElementTyp.Fire,
                "2" => ElementTyp.Water,
                _ => throw new Exception($"Unrecognized Elementype {type}")
            };
        }

        public char? ConvertElementTypeToChar(ElementTyp? type)
        {
            if (type == null)
                return null;

            return type switch
            {
                ElementTyp.Normal => 'n',
                ElementTyp.Fire => 'f',
                ElementTyp.Water => 'w',
                _ => throw new Exception($"Unrecognized Elementype {type}")
            };
        }

        private IEnumerable<Type> GetAllCardTypes()
        {
            Assembly current = Assembly.GetExecutingAssembly();
            return current.GetTypes().Where(type => type.IsAssignableTo(typeof(Card)) && !type.IsAbstract);
        }

        public Card GetCardInstanceFromCategory(string category)
        {
            Type type = CardTypes.FirstOrDefault(type => type.Name == category);
            return (Card)Activator.CreateInstance(type);
        }
    }
}
