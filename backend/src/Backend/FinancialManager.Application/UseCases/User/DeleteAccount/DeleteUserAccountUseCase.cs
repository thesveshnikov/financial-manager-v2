using FinancialManager.Domain.Identity;
using FinancialManager.Domain.Repositories.User;

namespace FinancialManager.Application.UseCases.User.DeleteAccount;

public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserWriteOnlyRepository _repository;

    public DeleteUserAccountUseCase(
        ILoggedUser loggedUser,
        IUserWriteOnlyRepository repository)
    {
        _loggedUser = loggedUser;
        _repository = repository;
    }
    
    public async Task Execute()
    {
        var userId = _loggedUser.GetUserId();

        await _repository.DeactivateAccount(userId);
    }
}