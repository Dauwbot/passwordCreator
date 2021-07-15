using System;
using System.IO;
using System.Globalization;
using System.Text;
using System.Linq;

namespace passwordCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            //RemoveAccentuationFromFile("./sourceFiles/francais.txt");
            //AddUpperCaseLetter("./sourceFiles/anglais.txt");
        }
    
        static void RemoveAccentuationFromFile (string filePath) {
            int linesRemoved = 0;
            String line;
            try
            {
                StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("iso-8859-1")); // Latin 1; Western European (ISO) Encoding, otherwise it get opened in UTF8
                string tempFile = Path.GetTempFileName();
                StreamWriter sw = new StreamWriter(tempFile);
                line = sr.ReadLine();
                while (line != null)
                {
                    string normalizedLine = line.Normalize(NormalizationForm.FormD); // https://stackoverflow.com/questions/9349608/how-to-check-if-unicode-character-has-diacritics-in-net It separate the diacritics from the preceding letter
                    int lineLength = normalizedLine.Length; // So that we can know where we're in our line & get the last letter
                    int currentPos = 0;
                    foreach (char c in normalizedLine)
                    {
                        switch (CharUnicodeInfo.GetUnicodeCategory(c))
                        {
                            case UnicodeCategory.UppercaseLetter: // I don't want countries names or people in my password
                                Console.WriteLine(line);
                                linesRemoved++;
                                break;
                            case UnicodeCategory.DashPunctuation:   // so that we don't get words with a dash in it ex: demi-dieu, semi-automatique, sous-bois
                                Console.WriteLine(line);
                                linesRemoved++;
                                break;
                            case UnicodeCategory.NonSpacingMark: // With the Normalize method we separate our accent from our letter
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
                Console.WriteLine($"Removed {linesRemoved} lines from file");
                File.Delete(filePath);
                File.Move(tempFile, filePath);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        static void AddUpperCaseLetter (string filePath) {
            try
            {
                StreamReader sr = new StreamReader(filePath);
                string tempFile = Path.GetTempFileName();

                using (StreamWriter sw = new StreamWriter(tempFile))
                {
                    String line = sr.ReadLine();
                    while (line != null)
                    {
                        sw.WriteLine(line.First().ToString().ToUpper() + line.Substring(1));
                        line = sr.ReadLine();
                    }
                }
                sr.Close();
                File.Delete(filePath);
                File.Move(tempFile, filePath);
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }
        }


    }
}
