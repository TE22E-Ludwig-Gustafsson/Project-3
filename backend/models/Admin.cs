using Backend.Models;

using System;

namespace Backend.Models
{
    public class Admin : User
    {

        public void ChangeTicketStatus(Ticket ticket, int newStatusId)
        {
            if (ticket == null)
            {
                return;
            }

            ticket.StatusId = newStatusId;
        }
    }
}
