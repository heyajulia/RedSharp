using RedSharp;

var message = "*-1\r\n"u8.ToArray();
var array = Parser.Parse(message);
Console.WriteLine(array == NullArray.Instance);
