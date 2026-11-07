namespace MSL.Data
{
    public static class DatabaseInitializer
    {
        // Этот DDL забирают в CASE-средства (dbdiagram.io, DataGrip, pgModeler)
        public static string GetDdl()
        {
            return @"
                CREATE TABLE IF NOT EXISTS Departments (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Employees (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    login TEXT NOT NULL,
                    password TEXT NOT NULL,
                    full_name TEXT NOT NULL,
                    department_id INTEGER NOT NULL,
                    clearance_level INTEGER NOT NULL,
                    FOREIGN KEY (department_id) REFERENCES Departments(id)
                );

                CREATE TABLE IF NOT EXISTS EmployeePassports (
                    employee_id INTEGER PRIMARY KEY,
                    passport_data TEXT NOT NULL,
                    FOREIGN KEY (employee_id) REFERENCES Employees(id)
                );

                CREATE TABLE IF NOT EXISTS ClassifiedDocuments (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    title TEXT NOT NULL,
                    content TEXT NOT NULL,
                    secret_level INTEGER NOT NULL,
                    author_id INTEGER NOT NULL,
                    department_name TEXT NOT NULL,
                    FOREIGN KEY (author_id) REFERENCES Employees(id)
                );

                CREATE TABLE IF NOT EXISTS CitizenRequests (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    citizen_fio TEXT NOT NULL,
                    request_text TEXT NOT NULL,
                    assigned_employee_id INTEGER,
                    status TEXT NOT NULL CHECK (status IN ('В обработке', 'В работе', 'Закрыто')),
                    FOREIGN KEY (assigned_employee_id) REFERENCES Employees(id)
                );

                CREATE TABLE IF NOT EXISTS AuditLog (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    action_time TEXT NOT NULL,
                    user_login TEXT NOT NULL,
                    action_description TEXT NOT NULL
                );
                ";
        }

        public static void Initialize()
        {
            using var connection = DatabaseContext.CreateConnection();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = GetDdl();
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT COUNT(*) FROM Employees";
            long count = (long)(cmd.ExecuteScalar() ?? 0);
            if (count > 0) return;

            cmd.CommandText = @"
                INSERT INTO Departments (name) VALUES
                ('Департамент Специальной Связи'),
                ('Аналитический Департамент'),
                ('Общий Отдел');

                INSERT INTO Employees (login, password, full_name, department_id, clearance_level) VALUES
                ('minister', 'SuperMinister2026!', 'Воронов В.В.', 1, 3),
                ('analyst_petrov', 'petrovPass', 'Петров П.П.', 2, 2),
                ('clerk_sidorov', '123456', 'Сидоров С.С.', 3, 1);

                INSERT INTO EmployeePassports (employee_id, passport_data) VALUES
                (1, '4510 998877'),
                (2, '4512 112233'),
                (3, '4515 445566');

                INSERT INTO ClassifiedDocuments (title, content, secret_level, author_id, department_name) VALUES
                ('Спецификация квантовой криптографии', 'Секретные частоты и узлы: 144.800 MHz...', 3, 1, 'Департамент Специальной Связи'),
                ('Анализ угроз информбезопасности', 'Выявлены сетевые аномалии в подсети B...', 2, 2, 'Аналитический Департамент'),
                ('График дежурств охраны', 'Список ответственных за КПП...', 1, 3, 'Общий Отдел');

                INSERT INTO CitizenRequests (citizen_fio, request_text, assigned_employee_id, status) VALUES
                ('Иванов И.И.', 'Запрос на выдачу сертификата ключа', 3, 'В работе');
                ";
            cmd.ExecuteNonQuery();
        }
    }
}
