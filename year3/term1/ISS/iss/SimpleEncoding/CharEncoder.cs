using System;
using System.Collections.Generic;
using System.Text;

namespace LetterFrequency
{
    public enum EncodingType { None = -1, Cycle = 0, Shunting = 1, Rijndael = 2 };

    public class CharEncoder
    {
        private Dictionary<char, string> encodingMap;
        private Language currentLanguage;
        private EncodingType currentEncoding = EncodingType.None;

        private Dictionary<EncodingType, EncodingDelegate> charEncoding;
        private Dictionary<EncodingType, DecodingDelegate> charDecoding;

        private List<KeyValuePair<char, string>> encodingListMap;

        public CharEncoder(Language language, EncodingType startEncoding)
        {
            charEncoding = new Dictionary<EncodingType, EncodingDelegate>(3);
            charDecoding = new Dictionary<EncodingType, DecodingDelegate>(3);

            charEncoding.Add(EncodingType.Cycle, CharsEncoder.GetCycledChar);
            charEncoding.Add(EncodingType.Shunting, CharsEncoder.GetShuntedChars);
            charEncoding.Add(EncodingType.Rijndael, CharsEncoder.GetEncodedChar);

            charDecoding.Add(EncodingType.Cycle, CharsDecoder.GetUnCycledChar);
            charDecoding.Add(EncodingType.Shunting, CharsDecoder.GetUnShuntedChar);
            charDecoding.Add(EncodingType.Rijndael, CharsDecoder.GetDecodedChar);

            currentEncoding = startEncoding;
            ChangeLanguage(language);
        }

        private void ChangeEncodingType(EncodingType newEncoding)
        {
            currentEncoding = newEncoding;
            for (int i = 0; i < Languages.Strings[currentLanguage].Length; ++i)
            {
                char c = Languages.Strings[currentLanguage][i];
                encodingMap[c] = charEncoding[currentEncoding].Invoke(c, currentLanguage);
            }

            encodingListMap = new List<KeyValuePair<char, string>>(encodingMap);
        }

        private void ChangeLanguage(Language newLanguage)
        {
            currentLanguage = newLanguage;
            encodingMap = new Dictionary<char, string>(Languages.Strings[currentLanguage].Length);

            for (int i = 0; i < Languages.Strings[currentLanguage].Length; ++i)
            {
                char c = Languages.Strings[currentLanguage][i];
                encodingMap.Add(c, charEncoding[currentEncoding].Invoke(c, currentLanguage));
            }

            encodingListMap = new List<KeyValuePair<char, string>>(encodingMap);
        }

        public Language CurrentLanguage
        {
            get { return currentLanguage; }
            set
            {
                if (value != currentLanguage)
                    ChangeLanguage(value);
            }
        }

        public EncodingType CurrentEncoding
        {
            get { return currentEncoding; }
            set 
            {
                if (value != currentEncoding)
                    ChangeEncodingType(value);
            }
        }

        public List<KeyValuePair<char, string>> EncodingListMap
        {
            get { return encodingListMap; }
        }

        public Dictionary<char, string> EncodingMap
        {
            get { return encodingMap; }
        }
    }
}
