using System;
using System.Collections.Generic;
using System.Text;

namespace MSL.Models
{
    // ОШИБКА ПРОЕКТИРОВАНИЯ:
    // 1. Хранение открытого пароля прямо в свойствах модели.
    // 2. Персональные данные (паспорт) не изолированы и не помечены как чувствительные.
    // 3. Отсутствие сущности Department (департамент хранится строкой).
    public class Employee
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string RawPassword { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PassportData { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public int ClearanceLevel { get; set; } // 1, 2 или 3
    }
}
