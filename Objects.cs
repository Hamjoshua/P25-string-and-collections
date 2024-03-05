using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace app
{
    public class FileChanger
    {
        private const string PhoneRegex = @"\((\d{3})\)\s?(\d{3})-(\d{2})-(\d{2})";
        private string _fileContent;
        private string _filePath;
        private Dictionary<string, List<string>> _wrongWordsDictionary;

        public FileChanger(string dictionaryPath)
        {
            SetDictionary(dictionaryPath);
        }

        public void SetDictionary(string dictionaryPath)
        {
            using (StreamReader streamReader = new StreamReader(dictionaryPath))
            {
                string allText = streamReader.ReadToEnd();
                _wrongWordsDictionary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(allText);
            }
        }

        public void SetFile(string filePath)
        {
            _filePath = filePath;
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                _fileContent = streamReader.ReadToEnd();
            }
        }

        public int ChangeTelephoneNumbers()
        {
            int countOfChanges = Regex.Matches(_fileContent, PhoneRegex).Count;

            _fileContent = Regex.Replace(_fileContent, PhoneRegex, "+380 $1 $2 $3 $4");            

            return countOfChanges;
        }

        public int CorrectWrongWords()
        {
            int countOfChanges = 0;

            foreach (string key in _wrongWordsDictionary.Keys)
            {
                List<string> values = _wrongWordsDictionary[key];
                foreach (string value in values)
                {
                    foreach (Match match in Regex.Matches(_fileContent, value))
                    {
                        _fileContent = _fileContent.Replace(value, key);
                        ++countOfChanges;
                    }
                }
            }

            return countOfChanges;
        }

        public void SaveChanges(bool appendToEnd = false)
        {
            using (StreamWriter streamWriter = new StreamWriter(_filePath, appendToEnd))
            {
                streamWriter.Write(_fileContent);
            }
        }
    }
}
