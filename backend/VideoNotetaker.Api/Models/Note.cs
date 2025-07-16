using System;

namespace VideoNotetaker.Api.Models
{
    public class Note
    {
        public Guid Id { get; set; }
        public string VideoId { get; set; } = default!;
        public int TimestampSeconds { get; set; }
        public string Text { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
} 