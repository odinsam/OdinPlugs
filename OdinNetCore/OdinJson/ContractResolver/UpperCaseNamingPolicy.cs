using System.Text.Json;

namespace OdinPlugs.OdinNetCore.OdinJson.ContractResolver
{
    public class UpperCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) =>
            name.ToUpper();
    }
}