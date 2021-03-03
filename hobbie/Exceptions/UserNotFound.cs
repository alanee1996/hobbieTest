using System;
namespace hobbie.Exceptions
{
    public class UserNotFound : Exception
    {
        public UserNotFound(long id) : base($"User with id : {id} not found")
        {
        }
    }
}
