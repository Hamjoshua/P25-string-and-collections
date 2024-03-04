using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace app
{
    class Program
    {
        static bool GetChoice(string header)
        {
            Console.WriteLine($"{header}\nДля смены введите любой символ, чтобы оставить - нажмите Enter");
            string response = Console.ReadLine();
            return response == String.Empty;
        }

        static string GetDirectoryPath(string header, string defaultPath = null)
        {
            if (defaultPath != null)
            {
                string choiceHeader = $"Путь для \"{header}\" уже задан: {defaultPath}.";
                bool isDefaultSaved = GetChoice(choiceHeader);
                if (isDefaultSaved)
                {
                    return defaultPath;
                }
            }

            string directoryPath = "";

            while (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Введите полный путь до файла\\папки, где есть {header}: ");
                directoryPath = Console.ReadLine();
                if (!Directory.Exists(directoryPath))
                {
                    Console.WriteLine("\a Введено неверное имя папки!");
                }
            }

            return directoryPath;
        }

        static void Main(string[] args)
        {
            string dictionaryPath = "Dictionary.json";
            DirectoryInfo directoryInfo = null;

            FileChanger fileChanger = new FileChanger(GetDirectoryPath("словарь формата json", dictionaryPath));
            directoryInfo = new DirectoryInfo(GetDirectoryPath("текстовые файлы с неправильными словами"));

            bool isOverwriteFiles = GetChoice("По умолчанию файлы не перезаписываются.");

            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                if (fileInfo.Extension == ".txt")
                {
                    fileChanger.SetFile(fileInfo.FullName);
                    int countOfChangedWrongWords = fileChanger.CorrectWrongWords();
                    int countOfChangedPhones = fileChanger.ChangeTelephoneNumbers();
                    fileChanger.SaveChanges(isOverwriteFiles);

                    Console.WriteLine($"Файл \"{fileInfo.FullName}\"\n-> кол-во исправленных слов: {countOfChangedWrongWords};" +
                        $" кол-во исправленных телефонов: {countOfChangedPhones}");
                }
            };

            Console.ReadLine();
        }
    }
}
