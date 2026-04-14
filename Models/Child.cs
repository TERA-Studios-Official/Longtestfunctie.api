public class Child
{
    public int Id { get; set; }
    public required string ParentName { get; set; } // keep this
    public string AnonymousId { get; set; } = Guid.NewGuid().ToString();
    public string? Name { get; set; }
    public int? Age { get; set; }
    public string? Avatar { get; set; }
    public string? DoctorName { get; set; }
    public string? TreatmentType { get; set; }
    public DateTime? TreatmentDate { get; set; }
    public DateTime CreatedAt { get; set; }
}