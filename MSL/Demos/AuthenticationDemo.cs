using MSL.Data;
using System;
using System.Collections.Generic;
using System.Text;

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

            // ОШИБКА 1: Конкатенация строк в SQL
            // ОШИБКА 2: Потенциальный перехват паролей (логирование открытого пароля в консоль/дамп)
            string sql = $"SELECT full_name, clearance_level, department_name FROM Employees WHERE login = '{login}' AND password = '{password}'";

            using var command = connection.CreateCommand();
            command.CommandText = sql;

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
