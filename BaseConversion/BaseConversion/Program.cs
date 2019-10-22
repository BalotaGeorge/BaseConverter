using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseConversion
{
    class Program
    {
        static string sInitialNumber, sFinalNumber;
        static int nInitialBase, nFinalBase, nSmallestPosBase, nCommaPlace;
        static bool bOverFlow, bNegativeMark;
        static void ReadAndCheckNumber()
        {
            sFinalNumber = "";
            bool bAllGood = true;
            Console.Write("Enter the number you would like to convert: ");
            do
            {
                nCommaPlace = -1;
                bAllGood = true;
                sInitialNumber = Console.ReadLine();
                for (int i = 0; i < sInitialNumber.Length; i++)
                {
                    char t = sInitialNumber[i];
                    if (!((t >= '0' && t <= '9') || (t >= 'A' && t <= 'F') || (t >= 'a' && t <= 'f') || t == '.' || t == ',')) bAllGood = false;
                    else
                    {
                        if ((t == '.' || t == ',') && i < sInitialNumber.Length - 1) nCommaPlace = i;
                        else
                        {
                            if (t >= '0' && t <= '9' && nSmallestPosBase < t - 48 + 1) nSmallestPosBase = t - 48 + 1;
                            if (t >= 'A' && t <= 'F' && nSmallestPosBase < t - 55 + 1) nSmallestPosBase = t - 55 + 1;
                            if (t >= 'a' && t <= 'f' && nSmallestPosBase < t - 87 + 1) nSmallestPosBase = t - 87 + 1;
                        }
                    }
                    if (i == 0 && t == '-')
                    {
                        bAllGood = true;
                        bNegativeMark = true;
                    }
                }
                if (!bAllGood) Console.Write("Please use characters between 0-9 and A-F (or a-f). Try again: ");
            } while (!bAllGood);
        }
        static int ReadAndCheckBase(string sText, int nSmallBase)
        {
            if (nSmallBase == 1) nSmallBase++;
            int nBase = -int.MaxValue;
            Console.Write(sText);
            do
            {
                try { nBase = int.Parse(Console.ReadLine()); }
                catch (FormatException) { Console.Write("Please enter a actual number. Try again: "); }
                if ((nBase < nSmallBase || nBase > 16) && nBase != -int.MaxValue) 
                {
                    if (nSmallBase < 16) Console.Write("Please enter a base value between " + nSmallBase + "-16. Try again: ");
                    else Console.Write("Please enter a base value of 16. Try again: ");
                    nBase = -int.MaxValue;
                }
            } while (nBase == -int.MaxValue);
            return nBase;
        }
        static int ConvertBaseToDecimal(string Number, int Base)
        {
            int n = 0;
            for (int i = 0; i < Number.Length; i++) 
            {
                if (Number[i] >= '0' && Number[i] <= '9')
                {
                    try { checked { n += (Number[i] - 48) * (int)Math.Pow(Base, Number.Length - i - 1); } }
                    catch (OverflowException) { bOverFlow = true; }
                }
                else if (Number[i] >= 'A' && Number[i] <= 'Z')
                {
                    try { checked { n += (Number[i] - 55) * (int)Math.Pow(Base, Number.Length - i - 1); } }
                    catch (OverflowException) { bOverFlow = true; }
                }
                else if (Number[i] >= 'a' && Number[i] <= 'z')
                {
                    try { checked { n += (Number[i] - 87) * (int)Math.Pow(Base, Number.Length - i - 1); } }
                    catch (OverflowException) { bOverFlow = true; }
                }
            }
            return n;
        }
        static string ConvertDecimalToBase(int Number, int Base)
        {
            string ret = "";
            char[] sym = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            int aux = Number, k = 0;
            while (aux != 0) { k++; aux /= Base; }
            int[] digits = new int[k];
            int index = 0;
            while (Number != 0) 
            {
                digits[index++] = Number % Base;
                Number /= Base;
            }
            for (int i = index - 1; i >= 0; i--) ret += sym[digits[i]];
            return ret;
        }
        static float ConvertCommaBaseToDecimal(string Number, int Base)
        {
            float n = 0f;
            for (int i = 0; i < Number.Length; i++)
            {
                if (Number[i] >= '0' && Number[i] <= '9') n += (Number[i] - 48) * (float)Math.Pow(Base, -(i + 1)); 
                else if (Number[i] >= 'A' && Number[i] <= 'Z') n += (Number[i] - 55) * (float)Math.Pow(Base, -(i + 1)); 
                else if (Number[i] >= 'a' && Number[i] <= 'z') n += (Number[i] - 87) * (float)Math.Pow(Base, -(i + 1)); 
            }
            return n;
        }
        static string ConvertDecimalToCommaBase(float Number, int Base)
        {
            char[] sym = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            string ret = "";
            int maxCommaDigits = 8;
            while (Number != 0 && maxCommaDigits > 0) 
            {
                Number *= Base;
                int i = (int)Number;
                ret += sym[i];
                Number -= i;
                maxCommaDigits--;
            }
            return ret;
        }
        static void Main(string[] args)
        {
            char Desire;
            do
            {
                Console.WriteLine();
                ReadAndCheckNumber();
                nInitialBase = ReadAndCheckBase("Enter the initial base of the number: ", nSmallestPosBase);
                nFinalBase = ReadAndCheckBase("Enter the final base of the number: ", 2);
                if (nCommaPlace != -1)
                {
                    string sBeforeCommaNumber = sInitialNumber.Substring(0, nCommaPlace);
                    string sAfterCommaNumber = sInitialNumber.Substring(nCommaPlace + 1);
                    string sFirstHalf = ConvertDecimalToBase(ConvertBaseToDecimal(sBeforeCommaNumber, nInitialBase), nFinalBase);
                    string sSecondHalf = ConvertDecimalToCommaBase(ConvertCommaBaseToDecimal(sAfterCommaNumber, nInitialBase), nFinalBase);
                    if (sFirstHalf.Length == 0) sFirstHalf = "0";
                    if (bNegativeMark) sFinalNumber = "-" + sFirstHalf + "." + sSecondHalf;
                    else sFinalNumber = sFirstHalf + "." + sSecondHalf;
                }
                else
                {
                    sFinalNumber += bNegativeMark ? "-" : "";
                    sFinalNumber += ConvertDecimalToBase(ConvertBaseToDecimal(sInitialNumber, nInitialBase), nFinalBase);
                    if (sFinalNumber.Length == 0) sFinalNumber = "0";
                }
                if (!bOverFlow) Console.WriteLine(sInitialNumber + " B" + nInitialBase + " = " + sFinalNumber + " B" + nFinalBase);
                else Console.WriteLine("The number you introduced was to large to be converted. Sorry");
                Console.Write("Would you like to convert another number? (type y if so) ");
                try { Desire = Console.ReadLine()[0]; }
                catch (IndexOutOfRangeException) { Desire = 'n'; }
            } while (Desire == 'y' || Desire == 'Y');
        }
    }
}
