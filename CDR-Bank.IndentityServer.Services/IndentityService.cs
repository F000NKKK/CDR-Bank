using System.Security.Cryptography;
using System.Text;
using CDR_Bank.DataAccess.Identity;
using CDR_Bank.DataAccess.Identity.Entities;
using CDR_Bank.IndentityServer.Services.Abstractions;
using CDR_Bank.Libs.Identity.Contracts.RequestContracts.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;


namespace CDR_Bank.IndentityServer.Services
{
    public class IndentityService : IIndentityService
    {
        IdentityDataContext _context;
        private readonly string _secretKey;
        public IndentityService(IdentityDataContext context, string secretKey)
        {
            _secretKey = secretKey;
            _context = context;
        }
        public string Login(UserLoginData loginData)
        {
            var bayts = Encoding.UTF8.GetBytes(loginData.Password);
            var passwordHash = SHA3_512.HashData(bayts);
            var user = _context.Users.FirstOrDefault(u => (u.Email == loginData.Email)&&(u.PasswordHash==passwordHash.ToString()));
            if (user is null)
            {
                return "";
            }
            return GenerateJwtToken(user.Id, user.Email);
        }

        public string Registration(UserLoginData loginData)
        {
            
            if (_context.Users.FirstOrDefault(u => u.Email == loginData.Email) is null)
            {
                return "";
            }

            var bayts = Encoding.UTF8.GetBytes(loginData.Password);
            var passwordHash = SHA3_512.HashData(bayts);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = loginData.Email,
                PasswordHash = passwordHash.ToString()
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return GenerateJwtToken(user.Id, user.Email);
        }

        public UserData GetUserData(string token)
        {
            UserData data = CheckJwtToken(token);
            data.CreatedAt = _context.Users.FirstOrDefault(u => (u.Email == data.Email) && (u.Id == data.Id)).CreatedAt;
            return data;
        }

        public UserContactInfoContract GetUserContactsData(string token)
        {
            UserData data = CheckJwtToken(token);
            UserContactInfo contactInfo = _context.Users.FirstOrDefault(u => (u.Email == data.Email) && (u.Id == data.Id)).ContactInfo;
            var result = new UserContactInfoContract
            {
                PhoneNumber = contactInfo.PhoneNumber,
                PhoneConfirmed = contactInfo.PhoneConfirmed,
                Address = contactInfo.Address,
                City = contactInfo.City,
                Country = contactInfo.Country,
                PostalCode = contactInfo.PostalCode,
            };
            return result;
        }

        private UserData CheckJwtToken(string token)
        {
            if (string.IsNullOrEmpty(_secretKey))
                throw new ArgumentNullException(nameof(_secretKey), "JWT secret key is not set in the configuration");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            var userdata = new UserData{Email = email,Id = Guid.Parse(userId)};

            return userdata;
        }

    private string GenerateJwtToken(Guid userId, string email)
    {
            var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
            };

            if (string.IsNullOrEmpty(_secretKey))
                throw new ArgumentNullException(nameof(_secretKey), "JWT secret key is not set in the configuration");

            if (_secretKey.Length < 32)
                throw new ArgumentException("JWT secret key must be at least 32 bytes for A256CBC-HS512 algorithm", nameof(_secretKey));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.Aes256CbcHmacSha512);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(4),
                signingCredentials: creds
            );
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenString = jwtSecurityTokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
