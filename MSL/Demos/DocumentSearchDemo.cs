using MSL.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSL.Demos
{
    public static class DocumentSearchDemo
    {
        public static void Run()
        {
            Console.Write("\nВведите ваш допуск (1 - Служебный, 2 - Секретно, 3 - Сов. Секретно): ");
            string? level = Console.ReadLine();

            Console.Write("Поисковый запрос по материалам: ");
            string? term = Console.ReadLine();

            using var connection = DatabaseContext.CreateConnection();
            using var command = connection.CreateCommand();

            // ОШИБКА 1: Уровень допуска передается напрямую от клиента 
            // ОШИБКА 2: Вторая SQL-инъекция в LIKE: можно закомментировать условие допуска (-- или /*)
            command.CommandText = $"SELECT id, title, secret_level, content FROM ClassifiedDocuments WHERE secret_level <= {level} AND title LIKE '%{term}%'";

            try
            {
                using var reader = command.ExecuteReader();
                Console.WriteLine("\n--- РЕЗУЛЬТАТЫ ПОИСКА ---");
                bool found = false;
                while (reader.Read())
                {
                    found = true;
                    Console.WriteLine($"[{reader["id"]}] (Гриф: {reader["secret_level"]}) {reader["title"]}");
                    Console.WriteLine($"Содержание: {reader["content"]}\n");
                }
                if (!found) Console.WriteLine("Документы не найдены.");
            }
            catch (Exception ex)
            {
                // ОШИБКА 3: Утечка внутренней структуры базы в сообщении об ошибке (Information Disclosure)
                Console.WriteLine($"[CRITICAL DB ERROR]: {ex.Message}");
            }
        }
    }
}
