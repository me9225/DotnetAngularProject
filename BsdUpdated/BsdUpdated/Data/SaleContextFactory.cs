using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BsdUpdated.Data
{
    public class SaleContextFactory
    {
        private readonly IConfiguration _configuration;

        public SaleContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SaleContext CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SaleContext>();
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
            return new SaleContext(optionsBuilder.Options);
        }
    }
}