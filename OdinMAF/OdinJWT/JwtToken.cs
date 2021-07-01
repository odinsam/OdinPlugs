using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace OdinPlugs.OdinMAF.OdinJWT
{
    public class JwtToken : ISecurityTokenValidator
    {
        private readonly IConfiguration configuration;
        public JwtToken()
        {
        }
        public JwtToken(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        bool ISecurityTokenValidator.CanValidateToken => true;

        int ISecurityTokenValidator.MaximumTokenSizeInBytes { get; set; }

        bool ISecurityTokenValidator.CanReadToken(string securityToken)
        {
            return true;
        }

        public string GetToken(JObject user)
        {
            // push the user’s name into a claim, so we can identify the user later on.
            string userGuid = string.Empty;
            if (user.ContainsKey("guid"))
                userGuid = user.GetValue("guid").ToString();

            var claims = new[]
            {
                new Claim("guid", userGuid)
            };
            //sign the token using a secret key.This secret will be shared between your API and anything that needs to check that the token is legit.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("CnfOptions:JWT:SecurityKey"))); // 获取密钥
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //凭证 ，根据密钥生成
            //.NET Core’s JwtSecurityToken class takes on the heavy lifting and actually creates the token.
            /**
             * Claims (Payload)
                Claims 部分包含了一些跟这个 token 有关的重要信息。 JWT 标准规定了一些字段，下面节选一些字段:
                iss: The issuer of the token，token 是给谁的  发送者
                aud: 接收的
                sub: The subject of the token，token 主题
                exp: Expiration Time。 token 过期时间，Unix 时间戳格式
                iat: Issued At。 token 创建时间， Unix 时间戳格式
                jti: JWT ID。针对当前 token 的唯一标识
                除了规定的字段外，可以包含其他任何 JSON 兼容的字段。
             * */
            DateTime dt = DateTime.Now;
            DateTime expiresTime = dt.AddSeconds(Convert.ToInt64(configuration.GetValue<string>("CnfOptions:JWT:Expires")));
            var token = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("CnfOptions:JWT:Issuer"),
                audience: configuration.GetValue<string>("CnfOptions:JWT:Audience"),
                claims: claims,
                expires: expiresTime,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        ClaimsPrincipal ISecurityTokenValidator.ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            validatedToken = null;
            //判断token是否正确
            if (securityToken != "abcdefg")
                return null;

            //给Identity赋值
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim("name", "wyt"));
            identity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, "admin"));
            var principle = new ClaimsPrincipal(identity);
            return principle;
        }
    }
}
