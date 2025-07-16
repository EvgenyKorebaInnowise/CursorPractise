using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoNotetaker.Api.Data;
using VideoNotetaker.Api.Dtos;
using VideoNotetaker.Api.Models;

namespace VideoNotetaker.Api.Controllers
{
    [ApiController]
    [Route("api/notes/{videoId}")]
    public class NotesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public NotesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all notes for a given video ID, sorted by timestamp.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<NoteDto>), 200)]
        public async Task<IActionResult> GetNotes(string videoId)
        {
            var notes = await _dbContext.Notes
                .Where(n => n.VideoId == videoId)
                .OrderBy(n => n.TimestampSeconds)
                .Select(n => new NoteDto
                {
                    Id = n.Id,
                    TimestampSeconds = n.TimestampSeconds,
                    Text = n.Text,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync();

            return Ok(notes);
        }

        /// <summary>
        /// Creates a new note for a given video.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(NoteDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateNote(string videoId, [FromBody] CreateNoteRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var note = new Note
            {
                Id = Guid.NewGuid(),
                VideoId = videoId,
                TimestampSeconds = request.TimestampSeconds,
                Text = request.Text,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Notes.Add(note);
            await _dbContext.SaveChangesAsync();

            var dto = new NoteDto
            {
                Id = note.Id,
                TimestampSeconds = note.TimestampSeconds,
                Text = note.Text,
                CreatedAt = note.CreatedAt
            };

            return CreatedAtAction(nameof(GetNotes), new { videoId }, new[] { dto });
        }
    }
} 