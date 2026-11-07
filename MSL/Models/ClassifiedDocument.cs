using System.ComponentModel.DataAnnotations.Schema;

namespace MSL.Models
{
    public enum SecretLevel
    {
        Служебный = 1,
        Секретно = 2,
        СовершенноСекретно = 3
    }

    public class ClassifiedDocument
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public SecretLevel SecretLevel { get; set; }
        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }
        public Employee? Author { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
    }
}
