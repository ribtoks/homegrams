using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LetterFrequency
{
    /// <summary>
    /// Language of input text
    /// </summary>
    public enum Language { English, Ukrainian, Russian };

    public delegate string EncodingDelegate(char c, Language language);

    public delegate char DecodingDelegate(string s, Language language);

    public delegate string TextDecodingDelegate(string s, Language language);

    //interface for user controls
    public interface ILanguageChangable
    {
        void ChangeLanguage(Language newLanguage);
    }

    /// <summary>
    /// Class, that can count letters frequency
    /// </summary>
    public class LetterCounter
    {
        //frequency tables
        private Dictionary<Language, List<KeyValuePair<char, double>>> frequencyTable =
            new Dictionary<Language, List<KeyValuePair<char, double>>>();

        //count of certain letters in text
        private Dictionary<Language, int[]> letterCounts = new Dictionary<Language, int[]>();

        //count of all found letters in text
        private Dictionary<Language, int> allLettersCounts = new Dictionary<Language, int>();

        public LetterCounter()
        {
            allLettersCounts.Add(Language.English, 0);
            allLettersCounts.Add(Language.Russian, 0);
            allLettersCounts.Add(Language.Ukrainian, 0);

            Array languages = Enum.GetValues(typeof(Language));
            foreach (Language language in languages)
            {
                frequencyTable.Add(language, new List<KeyValuePair<char, double>>());
                letterCounts.Add(language, new int[0]);
            }
        }

        /// <summary>
        /// Saves Frequency Table of specified
        /// language to binary file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="language"></param>
        public void SaveLangFreqTableToFile(string filePath, Language language)
        {
            Languages.SaveFrequencyToFile(filePath, frequencyTable[language]);
        }

        /// <summary>
        /// Reads Frequency Table of specified
        /// language from binary file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="language"></param>
        public void ReadLangFreqTableFromFile(string filePath, Language language)
        {
            frequencyTable[language] = Languages.ReadFrequencyFromFile(filePath);
        }

        /// <summary>
        /// Calculate frequency of letters in
        /// text, that is in string
        /// </summary>
        /// <param name="s">String with text</param>
        public void CalculateFrequency(string s, Language language)
        {
            letterCounts[language] = new int[Languages.Strings[language].Length];
            frequencyTable[language] = new List<KeyValuePair<char, double>>();

            for (int i = 0; i < s.Length; ++i)
            {
                char c = char.ToLower(s[i]);

                int index = Languages.LanguageLetterIndex(c, language);
                if (index != -1)
                {
                    letterCounts[language][index]++;
                    allLettersCounts[language]++;
                }
            }
        }

        /// <summary>
        /// Calculate frequency of letters
        /// in text, that is in stream
        /// </summary>
        /// <param name="streamReader"></param>
        public void CalculateFrequency(StreamReader streamReader, Language language)
        {
            letterCounts[language] = new int[Languages.Strings[language].Length];

            int nextIntChar = 0;
            while ((nextIntChar = streamReader.Read()) != -1)
            {
                char c = char.ToLower((char)nextIntChar);

                int index = Languages.LanguageLetterIndex(c, language);
                if (index != -1)
                {
                    letterCounts[language][index]++;
                    allLettersCounts[language]++;
                }
            }
        }

        private List<KeyValuePair<char, double>> GetSortedFrequency(string letters, int[] frequencies, int allFrequencies)
        {
            char[] languageArray = letters.ToCharArray();
            double[] persentage = Array.ConvertAll<int, double>(
                frequencies,
                new Converter<int, double>(
                    delegate(int integer)
                    {
                        return ((double)integer / (double)allFrequencies) * 100;
                    }
                    ));

            Array.Sort<double, char>(persentage, languageArray);

            List<KeyValuePair<char, double>> lettersFrequencies = new List<KeyValuePair<char, double>>(persentage.Length);

            for (int i = persentage.Length - 1; i >= 0; --i)
                lettersFrequencies.Add(new KeyValuePair<char, double>(languageArray[i], persentage[i]));

            return lettersFrequencies;
        }

        public List<KeyValuePair<char, double>> GetLettersPersentage(Language language)
        {
            if (frequencyTable[language].Count > 0)
                return frequencyTable[language];

            frequencyTable[language] = GetSortedFrequency(Languages.Strings[language],
                        letterCounts[language],
                        allLettersCounts[language]);

            return frequencyTable[language];
        }

        public void WriteLettersFrequencyTo(TextWriter writer, Language language)
        {
            int i;
            //calculate sum first
            double sum = 0;

            for (i = 0; i < letterCounts[language].Length; ++i)
                sum += letterCounts[language][i];

            for (i = 0; i < letterCounts[language].Length; ++i)
            {
                double d = letterCounts[language][i];
                writer.WriteLine("'{0}' - {1:#.###}", Languages.Strings[language][i], d / sum);
            }
        }
    }

    public static class Languages
    {
        public readonly static Dictionary<Language, string> Strings;

        public readonly static Dictionary<Language, Dictionary<char, int>> Chars;

        private static readonly string UkrainianLetters = "абвгґдеєжзиіїйклмнопрстуфхцчшщьюя";

        private static readonly string EnglishLetters = "abcdefghijklmnopqrstuvwxyz";

        private static readonly string RussianLetters = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

        static Languages()
        {
            Strings = new Dictionary<Language, string>();
            Strings.Add(Language.English, EnglishLetters);
            Strings.Add(Language.Ukrainian, UkrainianLetters);
            Strings.Add(Language.Russian, RussianLetters);


            Chars = new Dictionary<Language, Dictionary<char, int>>(3);

            Array languages = Enum.GetValues(typeof(Language));

            foreach (Language language in languages)
            {
                Chars.Add(language, new Dictionary<char, int>(Strings[language].Length));

                for (int i = 0; i < Strings[language].Length; ++i)
                    Chars[language].Add(Strings[language][i], i);
            }
        }

        public static int LanguageLetterIndex(char c, Language language)
        {
            if (Chars[language].ContainsKey(c))
                return Chars[language][c];

            return -1;
        }

        public static List<KeyValuePair<char, double>> ReadFrequencyFromFile(string filePath)
        {
            try
            {
                using (Stream str = File.Open(filePath, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    return (List<KeyValuePair<char, double>>)bf.Deserialize(str);
                }
            }
            catch (FileNotFoundException)
            {
                throw new ApplicationException("No such file!");
            }
            catch (IOException)
            {
                throw new ApplicationException("User has no rights to open file!");
            }
        }

        public static void SaveFrequencyToFile(string filePath, List<KeyValuePair<char, double>> frequency)
        {
            try
            {
                using (Stream str = File.Open(filePath, FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(str, frequency);
                }
            }
            catch (IOException)
            {
                throw new ApplicationException("User has no rights to create file!");
            }
        }
    }

    public static class CharsEncoder
    {
        public static readonly string RijndaelSeparator = @"~.";

        private static int EncodingCycleLength = 13;

        public static string GetCycledChar(char c, Language language)
        {
            c = char.ToLower(c);
            int newIndex = (Languages.Chars[language][c] + EncodingCycleLength) % (Languages.Strings[language].Length);
            return Languages.Strings[language][newIndex].ToString();
        }

        public static string GetShuntedChars(char c, Language language)
        {
            int cycleLength = EncodingCycleLength;
            EncodingCycleLength = (int)c;
            string s = c.ToString() + GetCycledChar(c, language).ToString();
            EncodingCycleLength = cycleLength;
            return s;
        }

        public static string GetEncodedChar(char c, Language language)
        {
            return RijndaelSeparator + StringEncoder.Encode(c.ToString(), language.ToString());
        }
    }

    public static class CharsDecoder
    {
        private static int EncodingCycleLength = 13;

        public static char GetUnCycledChar(string s, Language language)
        {
            char c = char.ToLower(char.Parse(s));
            int oldIndex = Languages.Chars[language][c] - EncodingCycleLength;

            if (oldIndex < 0)
                oldIndex += Languages.Strings[language].Length;

            return Languages.Strings[language][oldIndex];
        }

        public static char GetUnShuntedChar(string s, Language language)
        {
            return s[0];
        }

        public static char GetDecodedChar(string encodedChar, Language language)
        {
            return char.Parse(StringEncoder.Decode(encodedChar, language.ToString()));
        }
    }

    public static class TextDecoder
    {
        public static string DecodeCycledText(string text, Language language)
        {
            StringBuilder decoded = new StringBuilder(text.Length);

            for (int i = 0; i < text.Length; ++i)
            {
                if (char.IsLetter(text[i]))
                {
                    decoded.Append(CharsDecoder.GetUnCycledChar(text[i].ToString(), language));
                }
                else
                    decoded.Append(text[i]);
            }

            return decoded.ToString();
        }

        public static string DecodeShuntedText(string text, Language language)
        {
            StringBuilder decoded = new StringBuilder(text.Length);

            for (int i = 0; i < text.Length; ++i)
            {
                if (char.IsLetter(text[i]))
                {
                    decoded.Append(text[i]);
                    ++i;
                }
                else
                    decoded.Append(text[i]);
            }

            return decoded.ToString();
        }

        public static string DecodeRijndaeledText(string text, Language language)
        {
            StringBuilder decoded = new StringBuilder(text.Length);

            for (int i = 0; i < text.Length; ++i)
            {
                int startIndex = text.IndexOf(CharsEncoder.RijndaelSeparator, i);
                if (i == startIndex)
                {
                    int index = i + CharsEncoder.RijndaelSeparator.Length;
                    int eqIndex = text.IndexOf("==", index + 1);
                    decoded.Append(CharsDecoder.GetDecodedChar(text.Substring(index, eqIndex - index + 2), language));
                    i = eqIndex + 1;
                }
                else
                {
                    if (startIndex != -1)
                    {
                        decoded.Append(text.Substring(i, startIndex - i));
                        i = startIndex - 1;
                    }
                    else
                    {
                        decoded.Append(text.Substring(i));
                        i = text.Length - 1;
                    }
                }
            }

            return decoded.ToString();
        }
    }
}
