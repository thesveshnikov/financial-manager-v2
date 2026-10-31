using System.Diagnostics.CodeAnalysis;
using CommonTestUtilities.Requests;
using FinancialManager.Application.UseCases.Shared.Validators;
using FinancialManager.Exception;
using Shouldly;

namespace Validators.Tests.Category;

public class CategoryValidatorTests
{
    [Fact]
    public void Success()
    {
        var validator = new CategoryValidator();

        var request = RequestCategoryJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Intentional because is a unit test")]
    public void Validate_ShouldHaveError_WhenNameIsEmpty(string name)
    {
        var request = RequestExpenseJsonBuilder.Build();
        request.Name = name;

        var validator = new ExpenseValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_NAME_REQUIRED));
        });
    }
}