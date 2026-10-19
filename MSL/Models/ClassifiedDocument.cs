using System;
using System.Collections.Generic;
using System.Text;

namespace MSL.Models
{
    // ОШИБКА ПРОЕКТИРОВАНИЯ:
    // 1. Нет навигационного свойства на Employee (автора).
    // 2. Поле AuthorId оторвано от бизнес-модели.
    // 3. Гриф секретности — просто int без строгой типизации (нет Enum).
    public class ClassifiedDocument
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int SecretLevel { get; set; }
        public int AuthorId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
    }
}
