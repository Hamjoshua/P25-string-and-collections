using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace app
{
    public class FileChanger
    {
        private const string _phoneRegex = @"\(\S*\) +\S*";
        private string _fileText;
        private string _filePath;
        private Dictionary<string, List<string>> _wrongWordsDict;

        private static FileChanger s_instance;

        FileChanger()
        {

        }

        public static FileChanger Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = new FileChanger();
                }
                return s_instance;
            }
        }

        public void SetDictionary(string dictionaryPath)
        {
            using (StreamReader streamReader = new StreamReader(dictionaryPath))
            {
                string allText = streamReader.ReadToEnd();
                _wrongWordsDict = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(allText);
            }
        }

        public void SetFile(string filePath)
        {
            _filePath = filePath;
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                _fileText = streamReader.ReadToEnd();
            }
        }

        public int ChangeTelephoneNumbers()
        {
            int countOfChanges = 0;

            foreach (Match match in Regex.Matches(_fileText, _phoneRegex))
            {
                string foundedString = match.Value;
                foundedString = foundedString.Replace('-', ' ');
                foundedString = foundedString.Replace("(", String.Empty);
                foundedString = foundedString.Replace(")", String.Empty);
                foundedString = "+380 " + foundedString;
                _fileText = _fileText.Replace(match.Value, foundedString);
                ++countOfChanges;
            }

            return countOfChanges;
        }

        public int CorrectWrongWords()
        {
            int countOfChanges = 0;

            foreach (string key in _wrongWordsDict.Keys)
            {
                List<string> values = _wrongWordsDict[key];
                foreach (string value in values)
                {
                    foreach (Match match in Regex.Matches(_fileText, value))
                    {
                        _fileText = _fileText.Replace(value, key);
                        ++countOfChanges;
                    }

                }
            }

            return countOfChanges;
        }

        public void SaveChanges(bool overwrite = false)
        {
            using (StreamWriter streamWriter = new StreamWriter(_filePath, !overwrite))
            {
                streamWriter.Write(_fileText);
            }
        }
    }
}
