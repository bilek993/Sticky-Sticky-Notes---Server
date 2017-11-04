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
                    Id = n.Id,
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

        // Update note
        [BasicAuthentication]
        public ResultItem Post([FromBody] NoteItem noteToBeUpdated)
        {
            string username = Thread.CurrentPrincipal.Identity.Name;

            Notes noteFromServer = _database.Notes.FirstOrDefault(n => n.Id == noteToBeUpdated.Id
                                                                    && n.Users.Username == username);

            if (noteFromServer == null)
                return new ResultItem(false, "Note doesn't exist.");

            noteFromServer.Context = noteToBeUpdated.Context;
            noteFromServer.LastEditDate = noteToBeUpdated.LastEditDate;
            _database.SaveChanges();
            return new ResultItem(true);
        }
    }
}
