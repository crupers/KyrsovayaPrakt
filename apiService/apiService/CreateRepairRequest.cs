namespace apiService
{
    public class CreateRepairRequest
    {
        public int CarId { get; set; }
        public int CreatedByUserId { get; set; }
        public string? Organization { get; set; }
        public string ProblemDescription { get; set; } = "";
        public string? Diagnosis { get; set; }
        public int? Mileage { get; set; }
        public int? PreviousRepairId { get; set; }
    }
}
