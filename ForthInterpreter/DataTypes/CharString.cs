namespace ForthInterpreter.DataTypes;

public class CharString
{
    public CharString(int charAddress, int length)
    {
        CharAddress = charAddress;
        Length = length;
    }

    public int CharAddress { get; }
    public int Length { get; }
}