using Microsoft.AspNetCore.Mvc;
using Backend.Logic;
using Backend.Models;
using System.Collections.Generic;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly DatabaseHandler _dbHandler = new DatabaseHandler();

        [HttpGet]
        public ActionResult<List<Ticket>> GetTickets()
        {
            var tickets = _dbHandler.GetTickets();
            return Ok(tickets);
        }

        [HttpPost]
        public ActionResult CreateTicket([FromBody] Ticket ticket)
        {
            _dbHandler.CreateTicket(ticket);
            return Ok();
        }

        public class UpdateStatusRequest
        {
            public int StatusId { get; set; }
        }

        [HttpPut("{id}/status")]
        public ActionResult UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            _dbHandler.UpdateTicketStatus(id, request.StatusId);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteTicket(int id)
        {
            _dbHandler.DeleteTicket(id);
            return Ok();
        }
    }
}