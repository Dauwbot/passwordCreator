using System;
using System.IO;
using System.Globalization;
using System.Text;

namespace passwordCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            int linesRemoved = 0;
            String line;
            try
            {
                StreamReader sr = new StreamReader("./sourceFiles/francais.txt", Encoding.GetEncoding("iso-8859-1"));
                string tempFile = Path.GetTempFileName();
                StreamWriter sw = new StreamWriter(tempFile);
                line = sr.ReadLine();
                while (line != null)
                {
                    string normalizedLine = line.Normalize(NormalizationForm.FormD);
                    int lineLength = normalizedLine.Length;
                    int currentPos = 0;
                    foreach (char c in normalizedLine)
                    {
                        switch (CharUnicodeInfo.GetUnicodeCategory(c))
                        {
                            case UnicodeCategory.UppercaseLetter:
                                Console.WriteLine(line);
                                linesRemoved++;
                                break;
                            case UnicodeCategory.DashPunctuation:
                                Console.WriteLine(line);
                                linesRemoved++;
                                break;
                            case UnicodeCategory.NonSpacingMark:
                                Console.WriteLine(line);
                                linesRemoved++;
                                break;
                            default:
                                ++currentPos;
                                break;
                        }

                        if (currentPos == lineLength) {
                            switch (CharUnicodeInfo.GetUnicodeCategory(c))
                            {
                                case UnicodeCategory.NonSpacingMark:
                                    Console.WriteLine(line);
                                    linesRemoved++;
                                    break;
                                default:
                                    sw.WriteLine(line);
                                    break;
                            }
                        }
                    }
                    line = sr.ReadLine();
                }
                sr.Close();
                Console.WriteLine($"Removed {linesRemoved} from file");
                File.Move(tempFile, "./sourceFiles/francais_modified.txt");
                Console.ReadLine();
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
    }
}
