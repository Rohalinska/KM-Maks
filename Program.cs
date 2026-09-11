using System;
using System.Text;

class Program
{
    static void Main()
    {
        int N = 58;
        double F = 0.3;
        sbyte A = 60;
        sbyte B = 20;
        float C = 33.25f;

        Console.WriteLine("=== ЗАВДАННЯ 1 та 2 (запис остачей згори вниз) ===");
        Console.WriteLine($"N = {N} -> Base 2: {IntToBase(N, 2)}, Base 8: {IntToBase(N, 8)}, Base 16: {IntToBase(N, 16)}");
        Console.WriteLine($"F = {F} -> Bin: {FracToBin(F, 6)}");

        Console.WriteLine("\n=== ЗАВДАННЯ 3 ===");
        AddTwosComplement8Bit(A, B);

        Console.WriteLine("\n=== ЗАВДАННЯ 4 ===");
        ParseIEEE754Single(C);

        Console.WriteLine("\n=== ЗАВДАННЯ 5 ===");
        double sumDouble = 0.1 + 0.2;
        Console.WriteLine($"0.1 + 0.2 = {sumDouble}");
        Console.WriteLine($"0.1 + 0.2 == 0.3: {sumDouble == 0.3}");
    }

    // Переведення цілого числа (запис остачей згори вниз — перша остача стає першим символом)
    static string IntToBase(int n, int targetBase)
    {
        if (n == 0) return "0";
        string digits = "0123456789ABCDEF";
        StringBuilder result = new StringBuilder();
        int val = Math.Abs(n);

        while (val > 0)
        {
            // Використовуємо Append замість Insert(0, ...),
            // щоб остачі зберігали порядок обчислення (згори вниз)
            result.Append(digits[val % targetBase]);
            val /= targetBase;
        }

        return n < 0 ? "-" + result.ToString() : result.ToString();
    }

    static string FracToBin(double f, int precision = 6)
    {
        StringBuilder result = new StringBuilder("0.");
        double val = Math.Abs(f) - Math.Truncate(Math.Abs(f));

        for (int i = 0; i < precision; i++)
        {
            val *= 2;
            int bit = (int)val;
            result.Append(bit);
            val -= bit;
        }

        return result.ToString();
    }

    static void AddTwosComplement8Bit(sbyte a, sbyte b)
    {
        byte a8 = (byte)a;
        byte b8 = (byte)b;
        byte sum8 = (byte)(a8 + b8);
        sbyte sumSigned = (sbyte)sum8;

        bool overflow = (a > 0 && b > 0 && sumSigned < 0) || (a < 0 && b < 0 && sumSigned >= 0);

        string binA = Convert.ToString(a8, 2).PadLeft(8, '0');
        string binB = Convert.ToString(b8, 2).PadLeft(8, '0');
        string binSum = Convert.ToString(sum8, 2).PadLeft(8, '0');

        Console.WriteLine($"A ({a}): {binA}");
        Console.WriteLine($"B ({b}): {binB}");
        Console.WriteLine($"Sum:   {binSum} (Десяткове: {sumSigned})");
        Console.WriteLine($"Прапорець переповнення (Overflow): {overflow}");
    }

    static void ParseIEEE754Single(float val)
    {
        byte[] bytes = BitConverter.GetBytes(val);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }

        uint bits = BitConverter.ToUInt32(bytes, 0);
        string bitStr = Convert.ToString(bits, 2).PadLeft(32, '0');

        string signStr = bitStr.Substring(0, 1);
        string expStr = bitStr.Substring(1, 8);
        string mantissaStr = bitStr.Substring(9, 23);

        int sign = signStr == "1" ? -1 : 1;
        int expVal = Convert.ToInt32(expStr, 2) - 127;

        double mantissaVal = 1.0;
        for (int i = 0; i < mantissaStr.Length; i++)
        {
            if (mantissaStr[i] == '1')
            {
                mantissaVal += Math.Pow(2, -(i + 1));
            }
        }

        double restored = sign * mantissaVal * Math.Pow(2, expVal);

        Console.WriteLine($"Число C = {val}");
        Console.WriteLine($"32 біти: {bitStr} (0x{bits:X8})");
        Console.WriteLine($"Знак S={signStr}, Порядок E={expStr} (p={expVal}), Мантиса M=1.{mantissaStr.Substring(0, 6)}...");
        Console.WriteLine($"Відновлене значення: {restored}");
    }
}