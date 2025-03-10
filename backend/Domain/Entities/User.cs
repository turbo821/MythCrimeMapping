﻿namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string Position { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? ResetCode { get; set; }
        public DateTime? ResetCodeExpiration { get; set; }
        public List<Crime> CreateCrimes { get; set; } = new List<Crime>();
        public List<Crime> EditCrimes { get; set; } = new List<Crime>();
    }
}
