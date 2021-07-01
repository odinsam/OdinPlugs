using Newtonsoft.Json.Linq;

namespace OdinPlugs.OdinNetCore.OdinJson.JsonExtensions
{
    public static class OdinJsonExtensions
    {
        public static JObjectList<T> ToJObjectList<T>(this JArray jary)
        {
            return new JObjectList<T>(jary);
        }

        public static JObjectList<T> ToJObjectList<T>(this JToken jtoken)
        {
            return new JObjectList<T>(jtoken as JArray);
        }

        public static void Add<T>(this JObject jObj, string key, JObjectList<T> value)
        {
            jObj.Add(key, value.ToJArray());
        }
    }

}