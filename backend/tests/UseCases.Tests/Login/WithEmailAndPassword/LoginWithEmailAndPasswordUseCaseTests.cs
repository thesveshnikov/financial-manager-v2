using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FinancialManager.Application.UseCases.Login.WithEmailAndPassword;
using FinancialManager.Exception;
using FinancialManager.Exception.ExceptionsBase;
using Shouldly;
using System.Net;
using CommonTestUtilities.Security;

namespace UseCases.Tests.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestLoginJsonBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(request.Password, user);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Tokens.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Tokens.AccessToken.ShouldNotBeNullOrEmpty();
        result.Tokens.RefreshToken.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task ShouldThrowException_WhenUserDontExist()
    {
        var request = RequestLoginJsonBuilder.Build();

        var useCase = CreateUseCase();

        var exception = await useCase.Execute(request).ShouldThrowAsync<InvalidLoginException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.Unauthorized);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_LOGIN_INVALID);
        });
    }

    [Fact]
    public async Task ShouldThrowException_WhenPasswordIsIncorrect()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(user: user);

        var exception = await useCase.Execute(request).ShouldThrowAsync<InvalidLoginException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.Unauthorized);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_LOGIN_INVALID);
        });
    }

    private LoginWithEmailAndPasswordUseCase CreateUseCase(string? password = null, FinancialManager.Domain.Entities.User? user = null)
    {
        var accessTokenGenerator = IAccessTokenGeneratorBuilder.Build();
        var refreshTokenGenerator = IRefreshTokenGeneratorBuilder.Build();
        var refreshTokenWriteOnlyRepository = IRefreshTokenWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = IUnitOfWorkBuilder.Build();
        var passwordHasherBuilder = new IPasswordHasherBuilder();
        var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();
        if(user is not null)
            userReadOnlyRepositoryBuilder.GetByEmail(user);

        if(string.IsNullOrWhiteSpace(password) == false)
            passwordHasherBuilder.VerifyPassword(password);

        return new LoginWithEmailAndPasswordUseCase(passwordHasherBuilder.Build(), userReadOnlyRepositoryBuilder.Build(), refreshTokenWriteOnlyRepository, accessTokenGenerator, refreshTokenGenerator, unitOfWork);
    }
}