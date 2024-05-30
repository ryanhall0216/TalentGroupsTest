using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TalentGroupsTest.Models
{
    [BsonCollection("users")]
    public class User : Document
    {
        public User() { }

        [Required(ErrorMessage = "Enter a first name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter a last name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter a DOB")]
        public DateTime DOB { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Enter a email")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email format")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter your address")]
        public string Address { get; set; } = string.Empty;

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Required]
        public DateTime DateUpdated { get; set; } = DateTime.Now;
    }
}
