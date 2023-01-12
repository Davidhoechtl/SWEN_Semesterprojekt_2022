
namespace MonsterTradingCardGame_Hoechtl.Infrastructure
{
    using MTCG.Logic.Infrastructure;
    using MTCG.Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal class CardJsonConverter : JsonConverter
    {
        private readonly CardFactory cardFactory;

        public CardJsonConverter(CardFactory cardFactory)
        {
            this.cardFactory = cardFactory;
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Card));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            return cardFactory.GetCardFromJObject(jo);
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }
}
