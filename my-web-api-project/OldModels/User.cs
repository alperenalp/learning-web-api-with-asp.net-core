using System;
using System.Collections.Generic;

#nullable disable

namespace Karya.Ege.Nutrition.API.Models
{
    public partial class User
    {
        public User()
        {
            User2AllergenCategory = new HashSet<User2AllergenCategory>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreateUserId { get; set; }
        public string CreateIpAddress { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public int? LastUpdateUserId { get; set; }
        public string LastUpdateIpAddress { get; set; }
        public int? Gender { get; set; }
        public decimal? WeightKg { get; set; }
        public decimal? HeightCm { get; set; }
        public DateTime? BirthDate { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<User2AllergenCategory> User2AllergenCategory { get; set; }
    }
}
