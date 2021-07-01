using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace OdinPlugs.OdinCore.Models
{

    public class RequestParamsModel
    {
        public string RequestQueryString { get; set; }
        public JObject RequestJson { get; set; }
        public String RequestFormDataString { get; set; }
        public JObject RequestFormData { get; set; }
        public List<Dictionary<string, UpLoadSmFile>> RequestUploadFile { get; set; }
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class UpLoadSmFile
    {
        public long FileSize { get; set; }
        public string FileName { get; set; }
        [JsonIgnore]
        public byte[] FileContent { get; set; }
    }
}