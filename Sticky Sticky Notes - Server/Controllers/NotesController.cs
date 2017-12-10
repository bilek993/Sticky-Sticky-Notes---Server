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
                        LastEditDate = n.LastEditDate,
                        Removed = n.Removed
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

        // Update multiple note
        [BasicAuthentication]
        public ResultItem Post([FromBody] List<NoteItem> notesToBeUpdated)
        {
            using (var database = new DatabaseMainEntities())
            {
                string username = Thread.CurrentPrincipal.Identity.Name;
                var result = new ResultItem(true);

                foreach (NoteItem noteToBeUpdated in notesToBeUpdated)
                {
                    Notes noteFromServer = database.Notes.FirstOrDefault(n => n.Id == noteToBeUpdated.Id
                                                                              && n.Users.Username == username);

                    if (noteFromServer.Removed || noteToBeUpdated.Removed)
                    {
                        noteFromServer.Removed = true;
                        database.SaveChanges();
                        continue;
                    }

                    if (noteFromServer == null)
                        result = new ResultItem(false, "One of notes don't exist.");

                    if (noteFromServer.LastEditDate >= noteToBeUpdated.LastEditDate)
                        continue;

                    noteFromServer.Context = noteToBeUpdated.Context;
                    noteFromServer.LastEditDate = noteToBeUpdated.LastEditDate;
                    database.SaveChanges();
                }

                return result;
            }
        }
    }
}
