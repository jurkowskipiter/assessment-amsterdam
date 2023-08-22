namespace Assessment.Domain.Models
{
    public class Order
    {
        public List<Line> Lines { get; set; } = new List<Line>();
    }
}
