using Newtonsoft.Json.Serialization;

namespace OdinPlugs.OdinNetCore.OdinJson.OdinStrategy
{
    public class NamingStrategyToLower : NamingStrategy
    {
        protected override string ResolvePropertyName(string name)
        {
            return name.ToLower();
        }
    }
}