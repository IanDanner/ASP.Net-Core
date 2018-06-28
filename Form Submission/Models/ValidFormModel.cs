using System;
using System.ComponentModel.DataAnnotations;

namespace Form_Submission.Models
{
    public class ValidFormModel
    {
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2)]
        public string LastName { get; set; }
        [Required]
        [Range(15,120)]
        public int Age { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType (DataType.Password)]
        [MinLength(8)]        
        public string Password { get; set; }
    }
}