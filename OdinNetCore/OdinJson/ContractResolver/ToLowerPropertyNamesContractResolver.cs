using Newtonsoft.Json.Serialization;
using OdinPlugs.OdinNetCore.OdinJson.OdinStrategy;

namespace OdinPlugs.OdinNetCore.OdinJson.ContractResolver
{
    public class ToLowerPropertyNamesContractResolver : DefaultContractResolver
    {
        public ToLowerPropertyNamesContractResolver()
        {
            base.NamingStrategy = new NamingStrategyToLower();
        }
    }
}