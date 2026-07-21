using System;
using System.Collections.Generic;
using TicketSystem.Models;

namespace TicketSystem.Interfaces
{
    public interface IRequestRepository
    {
        IEnumerable<UserRequest> GetAll();
        UserRequest GetById(Guid id);
        void Add(UserRequest request);
        void Update(UserRequest request);
        void Delete(Guid id);
    }
}
