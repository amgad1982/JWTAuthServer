using System.Security.Claims;
using JWTAuthServer.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

[ApiController]
[Route("auth")]
public class AuthController:ControllerBase{
    public AuthController(){
        
    }

    [HttpPost("login")]
    public IActionResult Login(string userName,string password){
        if(userName=="user" && password=="password"){
            var token=GenerateJwtToken("http://localhost:5000");
            return Ok(new {token});
        };
        return Unauthorized();
    }

    //generate signed jwt token
    private string GenerateJwtToken(string audience){
        var claims=new List<Claim>{
            new Claim(JwtRegisteredClaimNames.Sub,"user"),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName,"user")
        };
        var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super secret key"));
        var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
        var token=new JwtSecurityToken(
            issuer:"http://localhost:5000",
            audience:audience,
            claims,
            expires:DateTime.UtcNow.AddDays(30),
            signingCredentials:creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}