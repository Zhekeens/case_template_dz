using MSL.Data;

namespace MSL.Demos
{
    public static class DocumentSearchDemo
    {
        public static void Run()
        {
            Console.Write("\nЛогин сотрудника: ");
            string? login = Console.ReadLine();
            Console.Write("Поисковый запрос по материалам: ");
            string? term = Console.ReadLine();

            using var connection = DatabaseContext.CreateConnection();

            using var levelCommand = connection.CreateCommand();
            levelCommand.CommandText = "SELECT clearance_level FROM Employees WHERE login = $login";
            levelCommand.Parameters.AddWithValue("$login", login ?? "");
            var level = levelCommand.ExecuteScalar();
            if (level is null)
            {
                Console.WriteLine("\n[ОТКАЗ] Сотрудник не найден.");
                return;
            }

            using var command = connection.CreateCommand();
            command.CommandText =
                "SELECT id, title, secret_level, content FROM ClassifiedDocuments WHERE secret_level <= $level AND title LIKE $term";
            command.Parameters.AddWithValue("$level", level);
            command.Parameters.AddWithValue("$term", "%" + (term ?? "") + "%");

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
            catch (Exception)
            {
                Console.WriteLine("[ОШИБКА] Не удалось выполнить поиск.");
            }
        }
    }
}
