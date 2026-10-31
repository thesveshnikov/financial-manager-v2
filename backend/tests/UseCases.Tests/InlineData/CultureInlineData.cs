using System.Collections;

namespace UseCases.Tests.InlineData;

public class CultureInlineData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "en-US" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}