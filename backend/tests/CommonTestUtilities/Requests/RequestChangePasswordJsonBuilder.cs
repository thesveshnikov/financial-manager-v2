using Bogus;
using FinancialManager.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestChangePasswordJsonBuilder
{
    public static RequestChangePasswordJson Build(int newPasswordLength = 10)
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(request => request.CurrentPassword, f => f.Internet.Password())
            .RuleFor(request => request.NewPassword, f =>
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

                while (chars.Count < newPasswordLength)
                    chars.Add(f.PickRandom(all.ToCharArray()));

                return new string(chars
                    .OrderBy(_ => f.Random.Int())
                    .ToArray());
            });
    }
}