using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sticky_Sticky_Notes___Server.Models
{
    public static class UsersHelper
    {
        public static ResultItem VerifyUserForEmptyValues(UserItem usetToBeVeryfied)
        {
            if (usetToBeVeryfied.Username == null || usetToBeVeryfied.Password == null)
                return new ResultItem(false, "Null value on required field.");
            else if (usetToBeVeryfied.Username.Length == 0 || usetToBeVeryfied.Password.Length == 0)
                return new ResultItem(false, "Empty value on required field.");
            else
                return new ResultItem(true);
        }

        public static bool CheckUserCredentials(string username, string password)
        {
            using (var database = new DatabaseMainEntities())
            {
                return database.Users.Any(u => u.Username == username &&
                                               u.Password == password);
            }
        }
    }
}