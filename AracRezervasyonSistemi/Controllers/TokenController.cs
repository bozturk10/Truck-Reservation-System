using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Data.SqlClient;



namespace AracRezervasyonSistemi.Controllers
{
    public class TokenController : Controller
    {
        [HttpPost]
        public IActionResult GetToken([FromBody]LoginInfo user)
        {
            //String cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ars_db;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //SqlConnection con = new SqlConnection(cs);

            //String getUTypeQuery = "SELECT UserType FROM [tblUser] WHERE Username=@Username";

            //SqlParameter param = new SqlParameter();
            //param.ParameterName = "@Username";
            //param.Value = user.UserName;

            ////var dataTable = dBManager.ExecuteQuery(get, null);


            if (IsValidUserAndPassword(user.UserName, user.Password))
                return Json(new { token = GenerateToken(user) });

            return Unauthorized();
        }

        public class LoginInfo
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public int UserType { get; set; }
        }


        [HttpPost]
        private string GenerateToken(LoginInfo l)
        {
            List<Claim> someClaims = new List<Claim>();

            //if (l.UserName == "a" && l.Password == "a")
            someClaims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, l.UserName));
            someClaims.Add(new Claim(JwtRegisteredClaimNames.NameId, Guid.NewGuid().ToString()));
            //someClaims.Add(new Claim(ClaimTypes.Role, "Basic"));
            //someClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
            //someClaims.Add(new Claim("NakliyeciId", "123"));


            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("uzun ince bir yoldayım şarkısını buradan tüm sevdiklerime hediye etmek istiyorum mümkün müdür acaba?"));
            var token = new JwtSecurityToken(
                                                issuer: "http://localhost:53806/",
                                                audience: "http://localhost:53806/",
                                                claims: someClaims,
                                                expires: DateTime.Now.AddYears(1),
                                                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                                                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool IsValidUserAndPassword(string userName, string password)
        {
            return true;
        }
    }
}