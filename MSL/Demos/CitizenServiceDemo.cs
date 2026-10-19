using MSL.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSL.Demos
{
    public static class CitizenServiceDemo
    {
        public static void Run()
        {
            Console.Write("\nФИО Гражданина: ");
            string? fio = Console.ReadLine();
            Console.Write("Суть обращения: ");
            string? text = Console.ReadLine();
            Console.Write("Назначить сотрудника (ID): ");
            string? empId = Console.ReadLine();

            using var connection = DatabaseContext.CreateConnection();

            // ОШИБКА 1: Отсутствие транзакции
            // Если упадет вторая операция (аудит), первая останется в базе.
            using var cmdInsert = connection.CreateCommand();
            cmdInsert.CommandText = $"INSERT INTO CitizenRequests (citizen_fio, request_text, assigned_employee_id, status) VALUES ('{fio}', '{text}', {empId}, 'В работе')";
            cmdInsert.ExecuteNonQuery();

            // ОШИБКА 2: Небезопасный и неконсистентный аудит
            // Разработчик просто «забыл» передать текущего пользователя и пишет константу,
            // а если упадет связь, событие аудита потеряется навсегда
            using var cmdAudit = connection.CreateCommand();
            cmdAudit.CommandText = $"INSERT INTO AuditLog (action_time, user_login, action_description) VALUES ('{DateTime.UtcNow:o}', 'UNKNOWN_OPERATOR', 'Создано обращение гражданина: {fio}')";
            cmdAudit.ExecuteNonQuery();

            Console.WriteLine("[УСПЕХ] Обращение успешно зафиксировано.");
        }
    }
}
