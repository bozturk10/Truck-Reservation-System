using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ARS.DataAccess.DataSQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ARS.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Token")]
    [AllowAnonymous]
    public class TokenController : Controller
    {

        public class LoginInfo
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        [Route("GetToken")]
        public IActionResult GetToken([FromBody]LoginInfo user)
        {
            List<Claim> someClaims = ValidateUserAndAssignClaims(user);
            if (someClaims != null) { return Json(new { token = GenerateToken(someClaims) }); }

            return Unauthorized();
        }

        [HttpPost]
        private string GenerateToken(List<Claim> someClaims)
        {

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


        private List<Claim> ValidateUserAndAssignClaims(LoginInfo user)
        {
            List<Claim> someClaims = new List<Claim>();
            String checkUQuery = "SELECT * FROM [tblUser] WHERE Username=@Username AND Password=@Password";
            SqlParameter[] parameters = new SqlParameter[2] { new SqlParameter("@Username", user.UserName),
                                                              new SqlParameter("@Password", user.Password)
                                                            };
            IDBManager dBManager = new MSSQLDBManager();
            var dataTable = dBManager.ExecuteQuery(checkUQuery, parameters);
            LoginInfo db_result = new LoginInfo();
            db_result = null;
            if (dataTable.Rows.Count == 1)
            {
                int.TryParse(dataTable.Rows[0]["UserType"].ToString(), out int usertype);
                string username = dataTable.Rows[0]["Username"].ToString();
                someClaims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, username));
                someClaims.Add(new Claim(JwtRegisteredClaimNames.NameId, Guid.NewGuid().ToString()));
                someClaims.Add(new Claim(ClaimTypes.Role, "Basic"));

                if (usertype == 1)
                {
                    

                    int.TryParse(dataTable.Rows[0]["NakliyeFirmasiId"].ToString(), out int nakliyeFirmasiId);
                    someClaims.Add(new Claim("NakliyeFirmasiId", nakliyeFirmasiId.ToString()));

                    int.TryParse(dataTable.Rows[0]["ProductionLocationId"].ToString(), out int productionLocationId);
                    someClaims.Add(new Claim("ProductionLocationId", productionLocationId.ToString()));
                    return someClaims;
                }

                if (usertype == 2)
                {
                    someClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    return someClaims;
                }
            }

            return null;

        }

    }
}
