namespace Application.DTOs
{
    public class MonthlyProgressDTO
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public List<WeeklyProgressDTO> Weeks { get; set; } = [];
    }
}
