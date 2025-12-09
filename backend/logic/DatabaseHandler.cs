using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Backend.Models;

namespace Backend.Logic
{
    public class DatabaseHandler
    {
        private readonly string _connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=Project3DB;Trusted_Connection=True;Connection Timeout=30;";

        public List<Ticket> GetTickets()
        {
            var tickets = new List<Ticket>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"SELECT t.Id, t.Title, t.Description, t.UserId, t.StatusId, t.CreatedAt,
                                     u.Name as UserName, u.Email, u.IsAdmin,
                                     s.Name as StatusName
                              FROM Tickets t
                              JOIN Users u ON t.UserId = u.UserID
                              JOIN TicketStatus s ON t.StatusId = s.Id";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tickets.Add(new Ticket
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                            UserId = reader.GetInt32(3),
                            StatusId = reader.GetInt32(4),
                            CreatedAt = reader.GetDateTime(5),
                            User = new User
                            {
                                UserID = reader.GetInt32(3),
                                Name = reader.GetString(6),
                                Email = reader.GetString(7),
                                IsAdmin = reader.GetBoolean(8)
                            },
                            Status = new TicketStatus
                            {
                                Id = reader.GetInt32(4),
                                Name = reader.GetString(9)
                            }
                        });
                    }
                }
            }
            return tickets;
        }

        public void UpdateTicketStatus(int ticketId, int newStatusId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "UPDATE Tickets SET StatusId = @StatusId WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StatusId", newStatusId);
                    command.Parameters.AddWithValue("@Id", ticketId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void CreateTicket(Ticket ticket)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"INSERT INTO Tickets (Title, Description, UserId, StatusId) 
                              VALUES (@Title, @Description, @UserId, @StatusId)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", ticket.Title);
                    command.Parameters.AddWithValue("@Description", ticket.Description ?? "");
                    command.Parameters.AddWithValue("@UserId", ticket.UserId);
                    command.Parameters.AddWithValue("@StatusId", ticket.StatusId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTicket(int ticketId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Tickets WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", ticketId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public User GetUserByEmail(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
            var query = "SELECT UserID, Name, Email, PasswordHash, IsAdmin FROM Users WHERE Email = @Email";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                PasswordHash = reader.GetString(3),
                                IsAdmin = reader.GetBoolean(4)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void CreateUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO Users (Name, Email, PasswordHash, IsAdmin) VALUES (@Name, @Email, @PasswordHash, @IsAdmin)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@IsAdmin", user.IsAdmin);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}