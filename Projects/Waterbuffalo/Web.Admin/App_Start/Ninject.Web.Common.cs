using Unity.Core.Interface;
using Unity.Core.Interface.Service;
using Unity.Core.Model;
using Unity.Service;
using Web.Admin.Infrastructure.UserStore;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Web.Admin.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Web.Admin.App_Start.NinjectWebCommon), "Stop")]

namespace Web.Admin.App_Start
{
    using Unity.Core.Interface;
    using Unity.Core.Model;
    using Unity.Service; 
    using Unity.Core.Interface.Data;  
    using Unity.Data;
    using Unity.Data.Interface;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using System;
    using System.Web;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IDatabaseFactory>().To<DatabaseFactory>().InRequestScope();


            #region --- IdentityModel
            // MODEL Application
            kernel.Bind<IRepository<AppUser>>().To<Repository<AppUser>>().InRequestScope();
            kernel.Bind<IRepository<AppUserProfile>>().To<Repository<AppUserProfile>>().InRequestScope();
            kernel.Bind<IRepository<AppUserLogin>>().To<Repository<AppUserLogin>>().InRequestScope();
            kernel.Bind<IRepository<AppUserRole>>().To<Repository<AppUserRole>>().InRequestScope();
            kernel.Bind<IRepository<AppRole>>().To<Repository<AppRole>>().InRequestScope();

            kernel.Bind<IRepository<GroupRole>>().To<Repository<GroupRole>>().InRequestScope();
            kernel.Bind<IRepository<RolesInGroupRole>>().To<Repository<RolesInGroupRole>>().InRequestScope();
            kernel.Bind<IRepository<UsersInGroupRole>>().To<Repository<UsersInGroupRole>>().InRequestScope();

            //...

            kernel.Bind<IUserStore<AppUser, Guid>>().To<UserStore>().InRequestScope();
            kernel.Bind<IRoleStore<AppRole, Guid>>().To<RoleStore>().InRequestScope();

            kernel.Bind<IAuthenticationManager>().ToMethod(c => HttpContext.Current.GetOwinContext().Authentication).InRequestScope();

            #endregion


            #region --- IdentityService
            // SERVICE Application

            kernel.Bind<IAppUserService>().To<AppUserService>().InRequestScope();
            kernel.Bind<IAppRoleService>().To<AppRoleService>().InRequestScope();
            kernel.Bind<IAppUserLoginService>().To<AppUserLoginService>().InRequestScope();
            kernel.Bind<IAppUserRoleService>().To<AppUserRoleService>().InRequestScope();

            kernel.Bind<IGroupRoleService>().To<GroupRoleService>().InRequestScope();
            kernel.Bind<IRolesInGroupRoleService>().To<RolesInGroupRoleService>().InRequestScope();
            kernel.Bind<IUsersInGroupRoleService>().To<UsersInGroupRoleService>().InRequestScope();

            //...


            #endregion

            // Device Service
            kernel.Bind<IRepository<Device>>().To<Repository<Device>>().InRequestScope();
            kernel.Bind<IDeviceService>().To<DeviceService>().InRequestScope();


        }
    }
}