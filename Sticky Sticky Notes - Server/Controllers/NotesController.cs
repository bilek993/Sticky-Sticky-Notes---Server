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
        // Get all notes
        [BasicAuthentication]
        public List<NoteItem> Get()
        {
            using (var database = new DatabaseMainEntities())
            {
                string username = Thread.CurrentPrincipal.Identity.Name;

                return database.Notes.Where(n => n.Users.Username == username)
                    .Select(n => new NoteItem
                    {
                        Id = n.Id,
                        Context = n.Context,
                        LastEditDate = n.LastEditDate
                    }).ToList();
            }
        }

        // Create new note
        [BasicAuthentication]
        public int? Put([FromBody] NoteItem newNote)
        {
            using (var database = new DatabaseMainEntities())
            {
                string username = Thread.CurrentPrincipal.Identity.Name;
                Users user = database.Users.FirstOrDefault(u => u.Username == username);

                if (newNote.Context == null || newNote.LastEditDate == null)
                    return null;

                var note = new Notes
                {
                    Context = newNote.Context,
                    LastEditDate = newNote.LastEditDate,
                    Users = user
                };
                database.Notes.Add(note);
                database.SaveChanges();

                return note.Id;
            }
        }

        // Update note
        [BasicAuthentication]
        public ResultItem Post([FromBody] NoteItem noteToBeUpdated)
        {
            using (var database = new DatabaseMainEntities())
            {
                string username = Thread.CurrentPrincipal.Identity.Name;

                Notes noteFromServer = database.Notes.FirstOrDefault(n => n.Id == noteToBeUpdated.Id
                                                                           && n.Users.Username == username);

                if (noteFromServer == null)
                    return new ResultItem(false, "Note doesn't exist.");

                noteFromServer.Context = noteToBeUpdated.Context;
                noteFromServer.LastEditDate = noteToBeUpdated.LastEditDate;
                database.SaveChanges();
                return new ResultItem(true);
            }
        }
    }
}
