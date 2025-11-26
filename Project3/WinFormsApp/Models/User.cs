namespace WinFormsApp.Models
{
    public class User
    {
        public int Id {get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }

        public User() {}


        public User(string name, string email)
        {
            Name = name;
            Email = email;
            IsAdmin = false;
        }


        public void CreateTicket(string title, string description)
        {
            var db = new Data.DatabaseHandler();
            db.InsertTicket(title, description, Id);
        }
    }
}