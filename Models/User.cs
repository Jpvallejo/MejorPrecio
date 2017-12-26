using mejor_precio_3.Persistence;

namespace mejor_precio_3.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Mail { get; set; }

        public string Password { get; set; }
        public int Age { get; set; }
        public char Gender { get; set; }
        public int Id { get; set; }
        public int Verified { get; set; }
        public string Role { get; set; }
        public void CreateUser(UserAdd userAdd)
        {
            User user=new User();
            user.Name = userAdd.Name;
            user.Mail = userAdd.Mail;
            user.Gender = userAdd.Gender;
            user.Password = userAdd.Password;
            user.Age = userAdd.Age;
            user.Verified = 0;
            user.Role = "user";
            UserPersistence up=new UserPersistence();
            up.Add(user);
        }
        public bool Exist(string mail)
        {
            UserPersistence up=new UserPersistence();
            bool exist= up.Exist(mail);
            return exist;
        }
    }
}