using System.Security.Cryptography;
using System.Text;
using CDR_Bank.DataAccess.Identity;
using CDR_Bank.DataAccess.Identity.Entities;
using CDR_Bank.IndentityServer.Services.Abstractions;
using CDR_Bank.Libs.Identity.Contracts.RequestContracts.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using CDR_Bank.Libs.Identity.Contracts.RequestContracts;


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
            var passwordHash = SHA512.HashData(bayts);
            var user = _context.Users.FirstOrDefault(u => (u.Email == loginData.Email)&&(u.PasswordHash==passwordHash.ToString()));
            if (user is null)
            {
                return "";
            }
            return GenerateJwtToken(user.Id, user.Email);
        }

        public string Registration(UserRegistrationContract registrationData)
        {
            
            if (!(_context.Users.FirstOrDefault(u => u.Email == registrationData.Email) is null))
            {
                return "";
            }

            var bayts = Encoding.UTF8.GetBytes(registrationData.Password);
            var passwordHash = SHA512.HashData(bayts);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = registrationData.Email,
                PasswordHash = passwordHash!.ToString()
            };

            _context.Users.Add(user);

            var userContactInfo = new UserContactInfo();
            userContactInfo.City = registrationData.City;
            userContactInfo.Country = registrationData.Country;
            userContactInfo.Address = registrationData.Address;
            userContactInfo.PhoneNumber = registrationData.PhoneNumber;
            userContactInfo.PostalCode = registrationData.PostalCode;
            userContactInfo.LastName = registrationData.LastName;
            userContactInfo.FirstName = registrationData.FirstName;
            userContactInfo.MiddleName = registrationData.MiddleName;
            userContactInfo.BirthDate = DateTime.Parse(registrationData.BirthDate);

            user.ContactInfo = userContactInfo;

            _context.Users.Add(user);
            _context.SaveChanges();
            return GenerateJwtToken(user.Id, user.Email);
        }


        public bool Edit(string token, UserContactInfoContract contactInfoContract)
        {
            UserData data = CheckJwtToken(token);
            User user = _context.Users.FirstOrDefault(u => u.Email == data.Email);
            if(user is null)
            {
                return false;
            }

            UserContactInfo contactInfo = user.ContactInfo;

            if (!contactInfoContract.City.Equals(""))
            {
                contactInfo.City = contactInfoContract.City;
            }
            if (!contactInfoContract.Address.Equals(""))
            {
                contactInfo.Address = contactInfoContract.Address;
            }
            if (!contactInfoContract.BirthDate.Equals(""))
            {
                contactInfo.BirthDate = contactInfoContract.BirthDate;
            }
            if (!contactInfoContract.Country.Equals(""))
            {
                contactInfo.Country = contactInfoContract.Country;
            }
            if (!contactInfoContract.FirstName.Equals(""))
            {
                contactInfo.FirstName = contactInfoContract.FirstName;
            }
            if (!contactInfoContract.LastName.Equals(""))
            {
                contactInfo.LastName = contactInfoContract.LastName;
            }
            if (!contactInfoContract.MiddleName.Equals(""))
            {
                contactInfo.MiddleName = contactInfoContract.MiddleName;
            }
            if (!contactInfoContract.PostalCode.Equals(""))
            {
                contactInfo.PostalCode = contactInfoContract.PostalCode;
            }


            _context.ContactInfos.Update(contactInfo);
            _context.SaveChanges();
            return true;
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
            User user = _context.Users.FirstOrDefault(u => (u.Email == data.Email) && (u.Id == data.Id));
            UserContactInfo contactInfo = user.ContactInfo;
            if (contactInfo is null)
            {
                user.ContactInfo = new UserContactInfo();
                _context.Users.Update(user);
                _context.ContactInfos.Update(user.ContactInfo);
                _context.SaveChanges();
                contactInfo = user.ContactInfo;
            }
            var result = new UserContactInfoContract
            {
                PhoneNumber = contactInfo.PhoneNumber,
                PhoneConfirmed = contactInfo.PhoneConfirmed,
                Address = contactInfo.Address,
                City = contactInfo.City,
                Country = contactInfo.Country,
                PostalCode = contactInfo.PostalCode,
                BirthDate = contactInfo.BirthDate,
                FirstName = contactInfo.FirstName,
                LastName = contactInfo.LastName,
                MiddleName = contactInfo.MiddleName,
                Email = data.Email,

            };
            return result;
        }

        public bool ChangePassword(string token, PasswordChange passwordChange)
        {
            UserData data = CheckJwtToken(token);
            var bayts = Encoding.UTF8.GetBytes(passwordChange.OldPassword);
            var passwordHash = SHA512.HashData(bayts);
            User user = _context.Users.FirstOrDefault(u => (u.Email == data.Email) && (u.Id == data.Id)&&(u.PasswordHash==passwordHash.ToString()));
            if (user is null)
            {
                return false;
            }
            bayts = Encoding.UTF8.GetBytes(passwordChange.NewPassword);
            passwordHash = SHA512.HashData(bayts);
            user.PasswordHash = passwordHash.ToString();
            _context.Users.Update(user);
            return true;
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
