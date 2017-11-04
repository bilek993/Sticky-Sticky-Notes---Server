using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using Sticky_Sticky_Notes___Server.Models;

namespace Sticky_Sticky_Notes___Server.Controllers
{
    public class NotesController : ApiController
    {
        private DatabaseMainEntities _database = new DatabaseMainEntities();

        // Get all notes
        [BasicAuthentication]
        public List<NoteItem> Get()
        {
            string username = Thread.CurrentPrincipal.Identity.Name;

            return _database.Notes.Where(n => n.Users.Username == username)
                .Select(n => new NoteItem
            {
                Context = n.Context,
                LastEditDate = n.LastEditDate
            }).ToList();
        }

        // Create new note
        [BasicAuthentication]
        public ResultItem Put([FromBody] NoteItem newNote)
        {
            string username = Thread.CurrentPrincipal.Identity.Name;
            Users user = _database.Users.FirstOrDefault(u => u.Username == username);

            if (newNote.Context == null || newNote.LastEditDate == null)
                return new ResultItem(false, "Null value on required field.");

            var note = new Notes
            {
                Context = newNote.Context,
                LastEditDate = newNote.LastEditDate,
                Users = user
            };
            _database.Notes.Add(note);
            _database.SaveChanges();

            return new ResultItem(true);
        }
    }
}
