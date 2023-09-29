namespace Auth_system.Models
{
    public class User
    {
        /*
         * A model class to describe the user auth 
         * stuff
         */

        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
