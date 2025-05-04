using Microsoft.EntityFrameworkCore;
using MinimalAPI.Model;

namespace MinimalAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {

        }

        public DbSet<UsuarioModel> usuarios {  get; set; }
    }
}
