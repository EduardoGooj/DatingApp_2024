using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.DataEntities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsersAsync(DataContext context)
        {
            // Verifica si ya existen usuarios para evitar duplicados
            if (await context.Users.AnyAsync()) return;

            // Obtiene la ruta del archivo JSON
            var userDataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "UserSeedData.json");

            // Lee y deserializa los datos
            var userData = await File.ReadAllTextAsync(userDataPath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

            if (users == null) return;

            // Crea un hash de contraseña y agrega los usuarios a la base de datos
            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();
                user.UserName = user.UserName.ToLowerInvariant();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("123456")); // Contraseña fija para pruebas
                user.PasswordSalt = hmac.Key;
                context.Users.Add(user);
            }

            // Guarda los cambios en la base de datos
            await context.SaveChangesAsync();
        }
    }
}
