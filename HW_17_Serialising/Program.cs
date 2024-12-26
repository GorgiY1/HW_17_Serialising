using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace HW_17_Serialising
{
    
    [Serializable]
    public class OldLady
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public List<string> Possessions { get; private set; } = new List<string>();

        private static int _wishCount = 0; // Номер текущего желания

        // Метод выполнения желания
        public void FulfillWish(string newPossession)
        {
            _wishCount++;
            Possessions.Add(newPossession);

            // Имя файла для сохранения
            string fileName = $"Wish_{_wishCount}.bin";

            // Сериализация
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, this);
            }

            Console.WriteLine($"Желание выполнено! Добавлено имущество: {newPossession}. Данные сохранены в файл {fileName}.");
        }

        // Метод просмотра имущества после выполнения желания
        public static OldLady ViewWish(int wishNumber)
        {
            string fileName = $"Wish_{wishNumber}.bin";

            if (!File.Exists(fileName))
            {
                Console.WriteLine($"Файл {fileName} не найден!");
                return null;
            }

            // Десериализация
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                var oldLady = (OldLady)formatter.Deserialize(fs);
                Console.WriteLine($"Данные загружены из файла {fileName}.");
                return oldLady;
            }
        }

        // Метод отображения имущества
        public void ShowPossessions()
        {
            Console.WriteLine($"Имя: {Name}, Возраст: {Age}");
            Console.WriteLine("Имущество:");
            foreach (var possession in Possessions)
            {
                Console.WriteLine($"- {possession}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Создание объекта старухи
            var oldLady = new OldLady
            {
                Name = "Старуха",
                Age = 78
            };

            // Выполнение желаний
            oldLady.FulfillWish("Золотое кольцо");
            oldLady.FulfillWish("Дворец");
            oldLady.FulfillWish("Новая яхта");

            // Загрузка данных из файла
            var loadedOldLady = OldLady.ViewWish(2);
            if (loadedOldLady != null)
            {
                loadedOldLady.ShowPossessions();
            }
        }
    }

}
