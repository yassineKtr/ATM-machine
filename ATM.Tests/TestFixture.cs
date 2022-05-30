using Microsoft.Extensions.DependencyInjection;

namespace ATM_Machine
{
    public class TestFixture
    {
        public ServiceProvider ServiceProvider { get; private set; }
        public TestFixture()
        {
            var Services = new ServiceCollection();
            Services.AddTransient<IATM,ATM>();
            ServiceProvider = Services.BuildServiceProvider();
        }
    }
}
