using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriveIn.Models.Domain
{
    public class Profile
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? LicenceNumber { get; set; }
        public string? Issue { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}