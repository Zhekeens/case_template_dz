using MSL.Data;

namespace MSL.Demos
{
    public static class AuthenticationDemo
    {
        public static void Run()
        {
            Console.Write("\nВведите логин сотрудника: ");
            string? login = Console.ReadLine();
            Console.Write("Введите пароль: ");
            string? password = Console.ReadLine();

            using var connection = DatabaseContext.CreateConnection();
            using var command = connection.CreateCommand();
            command.CommandText =
                "SELECT e.full_name, e.clearance_level, d.name AS department_name " +
                "FROM Employees e " +
                "JOIN Departments d ON d.id = e.department_id " +
                "WHERE e.login = $login AND e.password = $password";
            command.Parameters.AddWithValue("$login", login ?? "");
            command.Parameters.AddWithValue("$password", password ?? "");

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                Console.WriteLine($"\n[АВТОРИЗОВАН] {reader["full_name"]}");
                Console.WriteLine($"Отдел: {reader["department_name"]} | Допуск: Уровень {reader["clearance_level"]}");
            }
            else
            {
                Console.WriteLine("\n[ОТКАЗ] Неверные учетные данные.");
            }
        }
    }
}
