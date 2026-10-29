using Bogus;
using FinancialManager.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestRegisterUserAccountJsonBuilder
{
    public static RequestRegisterUserAccountJson Build(int passwordLength = 10)
    {
        return new Faker<RequestRegisterUserAccountJson>()
            .RuleFor(request => request.Name, f => f.Person.FullName)
            .RuleFor(request => request.Email, (f, user) => f.Internet.Email(user.Name))
            .RuleFor(request => request.Password, f =>
            {
                const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                const string lower = "abcdefghijklmnopqrstuvwxyz";
                const string digits = "0123456789";
                const string special = "!@#$%^&*";
                const string all = upper + lower + digits + special;

                var chars = new List<char>
                {
                    f.PickRandom(upper.ToCharArray()),
                    f.PickRandom(lower.ToCharArray()),
                    f.PickRandom(digits.ToCharArray()),
                    f.PickRandom(special.ToCharArray())
                };

                while (chars.Count < passwordLength)
                    chars.Add(f.PickRandom(all.ToCharArray()));

                return new string(chars
                    .OrderBy(_ => f.Random.Int())
                    .ToArray());
            })
            .RuleFor(request => request.DateOfBirth, f => f.Date.Past().AddYears(-13))
            .Generate();
    }
}