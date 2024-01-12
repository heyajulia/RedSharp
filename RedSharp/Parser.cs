using System.Text;

namespace RedSharp;

public static class Parser
{
    public static IProtocolArray Parse(byte[] bytes)
    {
        var br = new ByteReader(bytes);
        var b = br.Read() ?? throw new EmptyMessageException();

        return b switch
        {
            (byte)'*' => ParseArray(br),
            _ => throw new InvalidStartException(b)
        };
    }

    private static IProtocolArray ParseArray(ByteReader br)
    {
        var length = ParseArrayLength(br) ?? throw new InvalidLengthException();

        return length switch
        {
            -1 => ReadCrLf(br) ? NullArray.Instance : throw new InvalidTrailerException(),
            _ => throw new NotImplementedException()
        };
    }

    private static bool ReadCrLf(ByteReader br)
    {
        return br.Read() == '\r' && br.Read() == '\n' && br.IsExhausted;
    }

    private static int? ParseArrayLength(ByteReader br)
    {
        var lengthString = br.ReadWhile(b => b != '\r');
        if (lengthString is null)
        {
            return null;
        }

        // I think "Encoding.ASCII.GetString" can never fail, see https://learn.microsoft.com/en-us/dotnet/api/system.text.encoding.ascii?view=net-8.0#system-text-encoding-ascii.
        if (!int.TryParse(Encoding.ASCII.GetString(lengthString), out var length))
        {
            return null;
        }

        return length;
    }
}

internal class InvalidTrailerException : Exception;

public interface IProtocolArray;

public class NullArray : IProtocolArray
{
    private NullArray()
    {
    }

    public static readonly NullArray Instance = new();
}

public class InvalidLengthException : Exception;

public class InvalidStartException(byte b) : Exception
{
    private readonly byte _b = b;
}

public class EmptyMessageException : Exception;