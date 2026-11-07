using MSL.Data;

namespace MSL.Demos
{
    public static class CitizenServiceDemo
    {
        public static void Run()
        {
            Console.Write("\nЛогин оператора: ");
            string? operatorLogin = Console.ReadLine();
            Console.Write("ФИО Гражданина: ");
            string? fio = Console.ReadLine();
            Console.Write("Суть обращения: ");
            string? text = Console.ReadLine();
            Console.Write("Назначить сотрудника (ID): ");
            string? empId = Console.ReadLine();

            if (!int.TryParse(empId, out int employeeId))
            {
                Console.WriteLine("[ОШИБКА] Некорректный ID сотрудника.");
                return;
            }

            using var connection = DatabaseContext.CreateConnection();
            using var transaction = connection.BeginTransaction();

            try
            {
                using var cmdInsert = connection.CreateCommand();
                cmdInsert.Transaction = transaction;
                cmdInsert.CommandText =
                    "INSERT INTO CitizenRequests (citizen_fio, request_text, assigned_employee_id, status) VALUES ($fio, $text, $empId, 'В работе')";
                cmdInsert.Parameters.AddWithValue("$fio", fio ?? "");
                cmdInsert.Parameters.AddWithValue("$text", text ?? "");
                cmdInsert.Parameters.AddWithValue("$empId", employeeId);
                cmdInsert.ExecuteNonQuery();

                using var cmdAudit = connection.CreateCommand();
                cmdAudit.Transaction = transaction;
                cmdAudit.CommandText =
                    "INSERT INTO AuditLog (action_time, user_login, action_description) VALUES ($time, $login, $description)";
                cmdAudit.Parameters.AddWithValue("$time", DateTime.UtcNow.ToString("o"));
                cmdAudit.Parameters.AddWithValue("$login", operatorLogin ?? "");
                cmdAudit.Parameters.AddWithValue("$description", "Создано обращение гражданина: " + fio);
                cmdAudit.ExecuteNonQuery();

                transaction.Commit();
                Console.WriteLine("[УСПЕХ] Обращение успешно зафиксировано.");
            }
            catch (Exception)
            {
                transaction.Rollback();
                Console.WriteLine("[ОШИБКА] Не удалось зафиксировать обращение.");
            }
        }
    }
}
