using WebApp4.Entities.Common;

namespace WebApp4.Entities
{
    public class School:BaseEntity
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
