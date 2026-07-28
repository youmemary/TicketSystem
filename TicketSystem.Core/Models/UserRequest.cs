using System;
using TicketSystem.Core.Enums;
// Финальная сборка
namespace TicketSystem.Models
{
    public class UserRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public RequestStatus Status { get; set; } = RequestStatus.New;
        public string Priority { get; set; }
    }
}
