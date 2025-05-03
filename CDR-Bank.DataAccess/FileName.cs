using CDR_Bank.DataAccess.Identity;
using Microsoft.EntityFrameworkCore;

namespace CDR_Bank.DataAccess
{
    internal class FileName
    {
        public void Test()
        {
            var options = new DbContextOptionsBuilder<IdentityDataContext>()
                .UseMySql(
                    "server=localhost;database=cd_bank;user=root;password=your_password;",
                    new MySqlServerVersion(new Version(8, 0, 36)) // <-- Укажи версию своей MySQL
                )
                .Options;

            using var context = new IdentityDataContext(options);

            // Пример: создать пользователя
            var user = new Identity.Entities.User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                PasswordHash = "hashed_password"
            };

            context.Users.Add(user);
            context.SaveChanges();

            // Проверка: найти пользователя по email
            var loadedUser = context.Users.FirstOrDefault(u => u.Email == "test@example.com");
            Console.WriteLine(loadedUser?.Email);
        }
    }
}
