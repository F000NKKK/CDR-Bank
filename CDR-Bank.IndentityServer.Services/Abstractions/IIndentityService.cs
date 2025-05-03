using CDR_Bank.Libs.Identity.Contracts.RequestContracts.Abstractions;

namespace CDR_Bank.IndentityServer.Services.Abstractions;

public interface IIndentityService
{
    string Login(UserLoginData loginData);
    string Registration(UserLoginData loginData);
    UserData GetUserData(string token);
    UserContactInfoContract GetUserContactsData(string token);
    bool ChangePassword(string token, PasswordChange passwordChange);
}