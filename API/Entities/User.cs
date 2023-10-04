using System;

namespace API.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            User otherUser = (User)obj;
            return Id == otherUser.Id;
        }

        // Override GetHashCode as well when Equals is overridden
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
