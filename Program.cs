using ApplicationTemplate.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationTemplate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var startup = new Startup();
                var serviceProvider = startup.ConfigureServices();
                var service = serviceProvider.GetService<IMainService>();

                service?.Invoke();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }
    }
}