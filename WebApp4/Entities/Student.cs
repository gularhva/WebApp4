using WebApp4.Entities.Common;

namespace WebApp4.Entities
{
    public class Student:BaseEntity
    {
        public string Name { get; set; }
        public int School_Id { get; set; }
        public School School { get; set; }
    }
}
