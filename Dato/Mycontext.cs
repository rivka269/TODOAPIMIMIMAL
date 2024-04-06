using Microsoft.EntityFrameworkCore;
namespace TodoApi.Dato
{
    public class Mycontext : DbContext
    {
        public Mycontext(DbContextOptions<Mycontext> options) : base(options)
        {
        }

       public DbSet<Item> Items { get; set; }
        public object Results { get; internal set; }
    }
}