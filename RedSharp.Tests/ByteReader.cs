namespace RedSharp.Tests;

using RedSharp;

public class ByteReaderTests
{
    [Test]
    public void Constructor_GivenNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ByteReader(null!));
    }

    [Test]
    public void Constructor_GivenEmpty_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => new ByteReader(Array.Empty<byte>()));
    }

    [Test]
    public void Constructor_GivenNonEmpty_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => new ByteReader([1, 2, 3]));
    }
}