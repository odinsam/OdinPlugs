using System.Text.Json;

namespace OdinPlugs.OdinNetCore.OdinJson.ContractResolver
{
    public class LowerCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) =>
            name.ToLower();
    }
}