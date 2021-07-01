using System;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.geoip.Model.V20200101;
using Newtonsoft.Json;
using OdinPlugs.OdinThirdParty.Models.AliModels;

namespace OdinPlugs.OdinThirdParty.OdinAli
{
    public class AliHelper
    {
        /// <summary>
        /// 阿里ip地址查询
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static IpSearchAddress_Model IpSearchAddress(string key, string sec, string ip)
        {
            DefaultProfile profile = DefaultProfile.GetProfile("cn-hangzhou", key, sec);

            IAcsClient client = new DefaultAcsClient(profile);

            var request = new DescribeIpv4LocationRequest();
            request.Ip = ip; // "221.206.131.10";
            try
            {
                var response = client.GetAcsResponse(request);
                IpSearchAddress_Model model = new IpSearchAddress_Model
                {
                    RequestId = response.RequestId,
                    Ip = response.Ip,
                    Country = response.Country,
                    Province = response.Province,
                    City = response.City,
                    County = response.County,
                    Isp = response.Isp
                };
                return model;
            }
            catch (Exception e)
            {
                System.Console.WriteLine("ErrCode:" + e.Message);
                return null;
            }
        }
    }
}