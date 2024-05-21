using Microsoft.Extensions.DependencyInjection;

namespace ATM_Machine
{
    public class TestFixture
    {
        public ServiceProvider ServiceProvider { get; private set; }
        public TestFixture()
        {
            var services = new ServiceCollection();
            services.AddTransient<IATM,ATM>();
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
