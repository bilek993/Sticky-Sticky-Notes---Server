using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sticky_Sticky_Notes___Server.Models;

namespace Sticky_Sticky_Notes___Server.Controllers
{
    public class UsersController : ApiController
    {
        private DatabaseMainEntities _database = new DatabaseMainEntities();

        // Add new user into db
        public ResultItem Put([FromBody] UserItem userFromBody)
        {
            if (userFromBody.Username == null || userFromBody.Password == null)
                return new ResultItem(false, "Null value on required field.");

            if (userFromBody.Username.Length == 0 || userFromBody.Password.Length == 0)
                return new ResultItem(false, "Empty value on required field.");

            if (_database.Users.Where(u => u.Username == userFromBody.Username).ToList().Count != 0)
                return new ResultItem(false, "User already exist.");

            var user = new Users
            {
                Username = userFromBody.Username,
                Password = userFromBody.Password
            };
            _database.Users.Add(user);
            _database.SaveChanges();

            return new ResultItem(true);
        }
    }
}
