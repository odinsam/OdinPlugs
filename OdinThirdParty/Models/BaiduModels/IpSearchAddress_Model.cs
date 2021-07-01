namespace OdinPlugs.OdinThirdParty.Models.BaiduModels
{
    public class IpSearchAddress_Model
    {
        public string address { get; set; }
        public AddressContent content { get; set; }

    }
    public class AddressContent
    {
        public string address { get; set; }
        public addressDetail address_detail { get; set; }
        public PointDetail point { get; set; }

    }
    public class addressDetail
    {
        public int city_code { get; set; }
        public string district { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string street { get; set; }
        public string street_number { get; set; }
    }
    public class PointDetail
    {
        public float x { get; set; }
        public float y { get; set; }
    }

}