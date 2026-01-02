using Sqids;

namespace CommonTestUtilities.IdEncryption;

public class IdEncrypterBuilder
{
    public static SqidsEncoder<long> Build()
    {
        return new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = "q4HTcafKNJR5OGvhIMAyswkUWn860EpXVzoZSb2BYgu1el7Cim39dDFtrPjLxQ"
        });
    }
}