using NetTopologySuite.Geometries;

namespace Domain.Entities
{
    public class Crime : BaseEntity
    {
        public string? Applicant { get; set; } = string.Empty;
        public Guid TypeId { get; set; }
        public CrimeType Type { get; set; } = null!;
        public Guid? WantedPersonId { get; set; }
        public WantedPerson? WantedPerson { get; set; }
        public string Location { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public DateTime? EditAt { get; set; }
        public DateTime CrimeDate { get; set; }
        public string? Description { get; set; }
        public Point Point { get; set; } = null!;
        public Guid? CreatorId { get; set; }
        public User? Creator { get; set; }
        public Guid? EditorId { get; set; }
        public User? Editor { get; set; }
    }
}
