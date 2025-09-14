using Domain.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.DTO;

namespace Infrastructure.JWT
{
    public class TokenGeneration
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbContext;
     
        public TokenGeneration(IConfiguration configuration, AppDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<LoginResponseDto?> Authenticate(LoginDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Phoneno) || string.IsNullOrWhiteSpace(request.Password))
            {
                return null;

            }
            var userAccount = await _dbContext.Users.FirstOrDefaultAsync(x => x.Phoneno == request.Phoneno);

            if (userAccount == null)
                return null;

            bool passcheck;

           
                passcheck = request.Password == userAccount.Password;
            
            

            if (!passcheck)
                return null;

            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
             new Claim(JwtRegisteredClaimNames.Name, request.Phoneno),
             new Claim(ClaimTypes.Role, userAccount.Role.ToString()),
             new Claim(ClaimTypes.NameIdentifier, userAccount.Id.ToString())

         }
                ),
                Expires = tokenExpiryTimeStamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                SecurityAlgorithms.HmacSha512Signature),

            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);
            return new LoginResponseDto
            {
                AccessToken = accessToken,
                PhnoPasswordCredential = request.Phoneno,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds,
                Role = userAccount.Role.ToString(),
                Id = userAccount.Id,
                Phoneno = userAccount.Phoneno




            };
        }


    }
}
