using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            string dictionaryPath = "Dictionary.json";
            DirectoryInfo directoryInfo = null;
            bool directoryTyped = false;

            FileChanger.Instance.SetDictionary(dictionaryPath);

            while (!directoryTyped)
            {
                Console.WriteLine("Введите полный путь до папки: ");
                string directoryPath = Console.ReadLine();
                directoryInfo = new DirectoryInfo(directoryPath);
                if (directoryInfo.Exists)
                {
                    directoryTyped = true;
                }
                else
                {
                    Console.WriteLine("\a Введено неверное имя папки!");
                }
            }

            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                if (fileInfo.Extension == ".txt")
                {
                    FileChanger.Instance.SetFile(fileInfo.FullName);
                    int countOfChangedWrongWords = FileChanger.Instance.CorrectWrongWords();
                    int countOfChangedPhones = FileChanger.Instance.ChangeTelephoneNumbers();
                    FileChanger.Instance.SaveChanges();

                    Console.WriteLine($"Файл \"{fileInfo.FullName}\"\n-> кол-во исправленных слов: {countOfChangedWrongWords};" +
                        $" кол-во исправленных телефонов: {countOfChangedPhones}");
                }
            };

            Console.ReadLine();
        }
    }
}
