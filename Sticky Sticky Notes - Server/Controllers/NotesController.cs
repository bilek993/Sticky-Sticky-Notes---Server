using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            return _database.Notes.Select(n => new NoteItem
            {
                Context = n.Context,
                LastEditDate = n.LastEditDate
            }).ToList();
        }

        // Create new note
        [BasicAuthentication]
        public ResultItem Put([FromBody] NoteItem newNote)
        {
            if (newNote.Context == null || newNote.LastEditDate == null)
                return new ResultItem(false, "Null value on required field.");

            var note = new Notes
            {
                Context = newNote.Context,
                LastEditDate = newNote.LastEditDate
                // TODO: Add current user here
            };
            _database.Notes.Add(note);
            _database.SaveChanges();

            return new ResultItem(true);
        }
    }
}
