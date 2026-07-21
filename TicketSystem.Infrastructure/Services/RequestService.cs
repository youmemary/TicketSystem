using System;
using System.Collections.Generic;
using TicketSystem.Core.Enums;
using TicketSystem.Interfaces;
using TicketSystem.Models;

namespace TicketSystem.Infrastructure.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _repository;

        public RequestService(IRequestRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<UserRequest> GetAllRequests() => _repository.GetAll();

        public UserRequest GetRequestById(Guid id) => _repository.GetById(id);

        public UserRequest CreateRequest(UserRequest request)
        {
            request.Id = Guid.NewGuid();
            request.CreatedAt = DateTime.UtcNow;
            request.Status = RequestStatus.New;
            _repository.Add(request);
            return request;
        }

        public void UpdateRequest(UserRequest request)
        {
            _repository.Update(request);
        }

        public void ChangeStatus(Guid id, RequestStatus newStatus)
        {
            var request = _repository.GetById(id);
            if (request != null)
            {
                request.Status = newStatus;
                _repository.Update(request);
            }
        }

        public void DeleteRequest(Guid id) => _repository.Delete(id);
    }
}
