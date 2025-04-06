namespace Domain.Models
{
    public class CrimeFilterRequest
    {
        public string? SearchQuery { get; set; }
        public Guid? CrimeTypeId { get; set; } // if select one type
        public IEnumerable<Guid>? CrimeTypeIds { get; set; } // if select more types
        public IEnumerable<Guid>? WantedPersonIds { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Radius { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? MyMarks { get; set; }
        public Guid? EditMarks { get; set; }

        public string GetCacheKey()
        {
            var parts = new List<string>
            {
                $"search={SearchQuery}",
                $"crimeTypeId={CrimeTypeId}",
                $"crimeTypeIds={string.Join(",", CrimeTypeIds ?? Enumerable.Empty<Guid>())}",
                $"wantedPersonIds={string.Join(",", WantedPersonIds ?? Enumerable.Empty<Guid>())}",
                $"lat={Latitude}",
                $"lon={Longitude}",
                $"radius={Radius}",
                $"start={StartDate?.ToString("yyyyMMddHHmmss")}",
                $"end={EndDate?.ToString("yyyyMMddHHmmss")}",
                $"myMarks={MyMarks}",
                $"editMarks={EditMarks}"
            };

            return string.Join(";", parts);
        }
    }
}
