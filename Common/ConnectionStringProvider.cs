using Microsoft.Extensions.Configuration;

namespace Common
{
    public sealed class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IConfiguration _configuration;
        public ConnectionStringProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string DefaultConnectionString => _configuration.GetConnectionString("DefaultConnection");
    }
}
