using System.ComponentModel.DataAnnotations;

namespace VideoNotetaker.Api.Dtos
{
    /// <summary>
    /// Request body for creating a new note.
    /// </summary>
    public class CreateNoteRequest
    {
        /// <summary>
        /// The timestamp (in seconds) in the video this note refers to.
        /// </summary>
        [Range(0, int.MaxValue)]
        public int TimestampSeconds { get; set; }

        /// <summary>
        /// The note text.
        /// </summary>
        [Required]
        [MaxLength(1000)]
        public string Text { get; set; } = default!;
    }
} 