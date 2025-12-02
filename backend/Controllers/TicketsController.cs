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

        [HttpPut("{id}/status")]
        public ActionResult UpdateStatus(int id, [FromBody] int statusId)
        {
            _dbHandler.UpdateTicketStatus(id, statusId);
            return Ok();
        }
    }
}