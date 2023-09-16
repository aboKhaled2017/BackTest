using Microsoft.Extensions.Options;

namespace Backend.Configurations
{
    /// <summary>
    /// used for managing the data seeding through a configurable options
    /// </summary>
    public class SeederSettings
    {
        public bool EnableSeeding { get; set; }
        public bool DeleteDataBaseOrRestart { get; set; }
    }

    /// <summary>
    /// binds the seeding settings section from app settings to the object class
    /// </summary>
    public class SeederConfigurations : IConfigureOptions<SeederSettings>
    {
        private readonly IConfiguration _config;

        public SeederConfigurations(IConfiguration config)
        {
            _config = config;
        }

        public void Configure(SeederSettings options)
        {
            _config.GetSection(nameof(SeederSettings)).Bind(options);
        }
    }
}
