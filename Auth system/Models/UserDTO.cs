namespace Auth_system.Models
{
    public class UserDTO
    {
        /*
         * The class model that will send data to the server
         */

        public required string UserName { get; set; }
        public required string Password { get; set; }

    }
}
