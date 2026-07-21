using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TicketSystem.Core.Enums;
using TicketSystem.Interfaces;
using TicketSystem.Models;

namespace TicketSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestsController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserRequest>> GetAll()
        {
            return Ok(_requestService.GetAllRequests());
        }

        [HttpGet("{id}")]
        public ActionResult<UserRequest> GetById(Guid id)
        {
            var request = _requestService.GetRequestById(id);
            if (request == null) return NotFound();
            return Ok(request);
        }

        [HttpPost]
        public ActionResult<UserRequest> Create(UserRequest request)
        {
            var createdRequest = _requestService.CreateRequest(request);
            return CreatedAtAction(nameof(GetById), new { id = createdRequest.Id }, createdRequest);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, UserRequest request)
        {
            if (id != request.Id) return BadRequest();
            _requestService.UpdateRequest(request);
            return NoContent();
        }

        [HttpPut("{id}/status")]
        public IActionResult ChangeStatus(Guid id, [FromBody] RequestStatus status)
        {
            _requestService.ChangeStatus(id, status);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _requestService.DeleteRequest(id);
            return NoContent();
        }
    }
}
