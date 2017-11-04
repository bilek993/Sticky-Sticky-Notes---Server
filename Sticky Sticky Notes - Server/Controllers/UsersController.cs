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
            ResultItem veryficationResult = UsersHelper.VerifyUserForEmptyValues(userFromBody);
            if (!veryficationResult.Successful)
                return veryficationResult;

            if (_database.Users.Any(u => u.Username == userFromBody.Username))
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

        // Verify username and password
        public ResultItem Post([FromBody] UserItem userFromBody)
        {
            ResultItem veryficationResult = UsersHelper.VerifyUserForEmptyValues(userFromBody);
            if (!veryficationResult.Successful)
                return veryficationResult;

            if (!_database.Users.Any(u => u.Username == userFromBody.Username  && u.Password == userFromBody.Password))
                return new ResultItem(false, "Wrong username and/or password.");

            return new ResultItem(true);
        }
    }
}
