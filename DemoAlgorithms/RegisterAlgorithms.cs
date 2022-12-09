using DemoAlgorithms.Algorithms;
using Microsoft.Extensions.DependencyInjection;

namespace DemoAlgorithms
{
    public static class RegisterAlgorithms
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<ISlowConvexHull, SlowConvexHull>();
        }
    }
}