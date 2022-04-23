namespace ForthInterpreter.DataTypes;

public static class BoolType
{
    public static int TrueFlag => -1;
    public static int FalseFlag => 0;

    public static bool IsTrue(int cellValue)
    {
        return cellValue != 0;
    }

    public static bool IsFalse(int cellValue)
    {
        return cellValue == 0;
    }

    public static int Flag(bool boolValue)
    {
        return boolValue ? TrueFlag : FalseFlag;
    }
}