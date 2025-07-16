using System;

namespace VideoNotetaker.Api.Dtos
{
    /// <summary>
    /// Represents a note attached to a YouTube video.
    /// </summary>
    public class NoteDto
    {
        /// <summary>
        /// The unique identifier for the note.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The timestamp (in seconds) in the video this note refers to.
        /// </summary>
        public int TimestampSeconds { get; set; }

        /// <summary>
        /// The note text.
        /// </summary>
        public string Text { get; set; } = default!;

        /// <summary>
        /// The UTC date and time when the note was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
} 