using System.ComponentModel.DataAnnotations;

namespace DatatableCRUD.Models
{
    public class SignupViewModel
    {
        



        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }

        [Required] public string Email { get;
       set; }


        [Required]
        public string Username { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		


	}
}
