using System.ComponentModel.DataAnnotations;

namespace Connecta.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }
        [Required]
        public string PhoneNumber { get; set; }

        [StringLength(200)]
        public string Address { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}