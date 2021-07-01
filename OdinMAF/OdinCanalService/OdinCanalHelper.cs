using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OdinPlugs.OdinMAF.OdinCanalService.OdinCanalModels;

namespace OdinPlugs.OdinMAF.OdinCanalService
{
    public class OdinCanal : IOdinCanal
    {
        public OdinCanalModel GetCanalInfo(string jsonData)
        {
            return JsonConvert.DeserializeObject<OdinCanalModel>(jsonData);
        }
    }


}