namespace WebApi.Tests.Resources;

public class UserIdentityManager
{
    private readonly FinancialManager.Domain.Entities.User _user;
    private readonly FinancialManager.Domain.Entities.Category _category;
    private readonly string _password;
    private readonly string _accessToken;

    public UserIdentityManager(
        FinancialManager.Domain.Entities.User user,
        FinancialManager.Domain.Entities.Category category,
        string password,
        string accessToken)
    {
        _user = user;
        _category = category;
        _password = password;
        _accessToken = accessToken;
    }

    public Guid GetId() => _user.Id;
    public string GetName() => _user.Name;
    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;
    public string GetAccessToken() => _accessToken;
    public FinancialManager.Domain.Entities.Category GetCategory() => _category;
}