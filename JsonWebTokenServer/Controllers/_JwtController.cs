//using System;
//using Microsoft.AspNet.Mvc;
//using MB5.Model.Security.Token;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Security.Principal;
//using Microsoft.AspNet.Authorization;
//using System.Text;
//using System.Collections.Generic;
//using System.Linq;

//// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

//namespace MB5.Controllers
//{
//    public class _JwtController : Controller
//    {
//        string path = @"C:\MB5\MB5\src\MB5\MyTokenKey.json";
//        private TokenAuthOptions _tokenOptions;

//        public _JwtController(TokenAuthOptions tokenOptions)
//        {
//            this._tokenOptions = tokenOptions;
//        }

//        // GET: /<controller>/
//        public dynamic signin()
//        {   
//            StringBuilder sb = new StringBuilder();
//            sb.Append("0#27#jamiu-ayinde#Bt-fleet");
//            sb.Append("#car#lcv");        

//            List<Claim> claims = new List<Claim>();
//            claims.Add(new Claim(ClaimTypes.Name, "jamiurt"));
//            claims.Add(new Claim(ClaimTypes.Role, "admin"));
//            claims.Add(new Claim(ClaimTypes.UserData, sb.ToString()));
//            ClaimsIdentity identity = new ClaimsIdentity(claims, "mb5Auth");
            
//            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();                                                                       
//            JwtSecurityToken securityToken = handler.CreateToken
//            (
//                issuer: _tokenOptions.Issuer,
//                audience: _tokenOptions.Audience,
//                signingCredentials: _tokenOptions.SigningCredentials,
//                subject: identity,
//                expires: DateTime.UtcNow.AddDays(2)
//            );
//            string ltoken = handler.WriteToken(securityToken);
            
//            return new { authenticated = true, entityId = 1, token = ltoken, tokenExpires = DateTime.UtcNow.AddDays(2) };

//        }
        
//        //[Authorize("Bearer")]
//        public string issignin()
//        {
//            StringBuilder sb = new StringBuilder();
//            var userdata = (from i in HttpContext.User.Identities
//                             from c in i.Claims
//                             select new { c.Value, c.Type });

//            foreach (var usrdata in userdata)
//            {
//                sb.Append(usrdata.Type + " = " + usrdata.Value);
//            }

//            return sb.ToString();
//        }

//        public void Genkey()
//        {
//            RSAKeyUtility.GenerateAndSaveKey(path);
//        }
//    }

//    public class AuthRequest
//    {
//        public string username { get; set; }
//        public string password { get; set; }
//    }

//    public class TokenObj
//    {
//        public bool authenticated { get; set; }
//        public string user { get; set; }
//        public int entityId { get; set; }
//        public string token { get; set; }
//        public DateTime tokenExpires { get; set; }
//    }
//}

