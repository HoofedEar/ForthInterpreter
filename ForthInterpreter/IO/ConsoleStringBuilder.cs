using System.Text;

namespace ForthInterpreter.IO;

public class ConsoleStringBuilder
{
    private readonly StringBuilder _stringBuilder = new();

    public ConsoleStringBuilder(int maxColumns)
    {
        MaxColumns = maxColumns;
        LastFilledColumn = 0;
    }

    private int MaxColumns { get; }
    private int LastFilledColumn { get; set; }

    private void Append(string value)
    {
        if (LastFilledColumn + value.Length > MaxColumns)
        {
            if (LastFilledColumn < 80)
                _stringBuilder.AppendLine();
            LastFilledColumn = 0;
        }

        _stringBuilder.Append(value);
        LastFilledColumn += value.Length;
    }

    public void AppendFormat(string format, params object[] args)
    {
        Append(string.Format(format, args));
    }

    public override string ToString()
    {
        return _stringBuilder.ToString();
    }
}