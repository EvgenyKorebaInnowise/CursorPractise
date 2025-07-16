using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using VideoNotetaker.Api.Dtos;
using Xunit;

namespace VideoNotetaker.Api.Tests.Integration
{
    public class NotesControllerTests : IClassFixture<TestApiFactory>
    {
        private readonly TestApiFactory _factory;

        public NotesControllerTests(TestApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetNotes_Empty_ReturnsEmptyList()
        {
            var client = _factory.CreateClient();
            var videoId = Guid.NewGuid().ToString();

            var response = await client.GetAsync($"/api/notes/{videoId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var notes = await response.Content.ReadFromJsonAsync<NoteDto[]>();
            notes.Should().NotBeNull();
            notes.Should().BeEmpty();
        }

        [Fact]
        public async Task PostNote_ThenGetNote_ReturnsCreatedAndThenNote()
        {
            var client = _factory.CreateClient();
            var videoId = Guid.NewGuid().ToString();

            var createRequest = new CreateNoteRequest
            {
                TimestampSeconds = 42,
                Text = "Test note"
            };

            var postResponse = await client.PostAsJsonAsync($"/api/notes/{videoId}", createRequest);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var createdNotes = await postResponse.Content.ReadFromJsonAsync<NoteDto[]>();
            createdNotes.Should().NotBeNull();
            createdNotes.Should().HaveCount(1);
            var createdNote = createdNotes[0];
            createdNote.TimestampSeconds.Should().Be(42);
            createdNote.Text.Should().Be("Test note");

            var getResponse = await client.GetAsync($"/api/notes/{videoId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var notes = await getResponse.Content.ReadFromJsonAsync<NoteDto[]>();
            notes.Should().NotBeNull();
            notes.Should().ContainSingle();
            notes[0].Id.Should().Be(createdNote.Id);
            notes[0].Text.Should().Be("Test note");
        }
    }
} 