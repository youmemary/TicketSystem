using System;
using System.Collections.Generic;
using TicketSystem.Core.Enums;
using TicketSystem.Models;

namespace TicketSystem.Interfaces
{
    public interface IRequestService
    {
        IEnumerable<UserRequest> GetAllRequests();
        UserRequest GetRequestById(Guid id);
        UserRequest CreateRequest(UserRequest request);
        void UpdateRequest(UserRequest request);
        void ChangeStatus(Guid id, RequestStatus newStatus);
        void DeleteRequest(Guid id);
    }
}
