using FinancialManager.Domain.Repositories;
using FinancialManager.Domain.Repositories.User;

namespace FinancialManager.Application.UseCases.User.DeleteAccount;

public class DeleteUserAccountPermanentlyUseCase : IDeleteUserAccountPermanentlyUseCase
{
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserAccountPermanentlyUseCase(
        IUserWriteOnlyRepository userWriteOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid userId)
    {
        await _userWriteOnlyRepository.DeleteAccount(userId);

        await _unitOfWork.Commit();
    }

}