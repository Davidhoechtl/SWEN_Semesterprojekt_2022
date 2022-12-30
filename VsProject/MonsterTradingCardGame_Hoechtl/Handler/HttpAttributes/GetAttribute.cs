namespace MonsterTradingCardGame_Hoechtl.Handler.HttpAttributes
{
    internal class GetAttribute : HttpAttribute
    {
        public override Models.HttpMethod Method => Models.HttpMethod.GET;
    }
}
