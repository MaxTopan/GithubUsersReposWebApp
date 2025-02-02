using GithubUsersSearcher.Services;
using System.Net.Http;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace GithubUsersSearcher
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<HttpClient>();
            container.RegisterType<IGithubService, GithubService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}