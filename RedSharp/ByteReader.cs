namespace RedSharp;

// TODO: Should we "roll-back" the position if we exhaust the byte array while reading?

public class ByteReader
{
    private readonly byte[] _bytes;
    private readonly int _length;
    private int _pos;

    public int BytesRemaining => _length - _pos;
    public bool IsExhausted => BytesRemaining <= 0;
    public byte? Current => IsExhausted ? null : _bytes[_pos];

    public ByteReader(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);
        
        _bytes = bytes;
        _length = bytes.Length;
    }

    public byte? Read() => IsExhausted ? null : _bytes[_pos++];

    public byte[]? Read(int n)
    {
        var start = _pos;

        for (var i = 0; i < n; i++)
        {
            if (Read() is null)
            {
                return null;
            }
        }

        return _bytes[start.._pos];
    }

    public byte[]? ReadWhile(Predicate<byte> predicate)
    {
        if (IsExhausted)
        {
            return null;
        }

        var start = _pos;

        while (!IsExhausted && predicate(Current!.Value))
        {
            _pos++;
        }
        
        return IsExhausted ? null : _bytes[start.._pos];
    }
}