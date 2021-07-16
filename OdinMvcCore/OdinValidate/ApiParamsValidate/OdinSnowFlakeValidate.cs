using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using OdinPlugs.OdinCore.ConfigModel;
using OdinPlugs.OdinInject;
using OdinPlugs.OdinInject.InjectCore;
using OdinPlugs.OdinUtils.OdinExtensions.BasicExtensions.OdinString;
using OdinPlugs.SnowFlake.SnowFlakePlugs.ISnowFlake;

namespace OdinPlugs.OdinMvcCore.OdinValidate.ApiParamsValidate
{
    public class OdinSnowFlakeValidationAttribute : ValidationAttribute
    {

        public string GetErrorMessage() => $"参数并非SnowFlake Id.";
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var options = OdinInjectCore.GetService<ConfigOptions>();
            var snowFlake = OdinInjectCore.GetService<IOdinSnowFlake>();
            string[] result = null;
            string resultStr = null;
            try
            {
                if (value.ToString().Length != 18)
                    return new ValidationResult(GetErrorMessage());
                var longvar = Convert.ToInt64(value);
                resultStr = snowFlake.AnalyzeId(longvar);
                result = resultStr.Split('_');
                if (result == null || result.Length != 4)
                    return new ValidationResult(GetErrorMessage());
                else
                {
                    var dt = DateTime.ParseExact(result[0], "yyyy-MM-dd hh:mm:ss:fff", System.Globalization.CultureInfo.CurrentCulture);
                    var datacenterId = Convert.ToInt64(result[1]);
                    var workerId = Convert.ToInt64(result[2]);
                    var sequence = Convert.ToInt64(result[3]);
                    if (datacenterId != options.FrameworkConfig.SnowFlake.DataCenterId ||
                        workerId != options.FrameworkConfig.SnowFlake.WorkerId ||
                        sequence < 0 || sequence > 4095)
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                    else
                    {
                        return ValidationResult.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"======{result[0]}====={result[1]}===={result[2]}======{result[3]}=========");
                System.Console.WriteLine(JsonConvert.SerializeObject(ex).ToJsonFormatString());
                throw ex;
            }
        }
    }
}