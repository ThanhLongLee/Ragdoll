using System.Web.UI.WebControls;
using Unity.Core.Interface;
using Unity.Core.Interface.Service;
using Unity.Core.Model;
using Unity.Service;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Web.Client.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Web.Client.App_Start.NinjectWebCommon), "Stop")]

namespace Web.Client.App_Start
{
    using Unity.Core.Interface;
    using Unity.Core.Model;
    using Unity.Service.Telegram;
    using Unity.Core.Interface.Data;
    using Unity.Data;
    using Unity.Data.Interface;
    using Microsoft.Owin.Security;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using System;
    using System.Web;
    using System.Web.Services.Description;
    using Unity.Service.CheckIn;
    using Unity.Core.Interface.Service.UserActions;
    using Unity.Service.UserActionService;
    using Unity.Core.Interface.Service.Link;
    using Unity.Service.Link;
    using Unity.Core.Interface.Service.UserBoosters;
    using Unity.Core.Interface.Service.Booster;
    using Unity.Service.BoostService;
    using Unity.Service.UserBoosters;
    using Unity.Core.Interface.Service.Invite;
    using Unity.Service.Invite;
    using Unity.Core.Interface.Service.UserWallet;
    using Unity.Service.UserWallet;

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


            // Device Service
            kernel.Bind<IRepository<Device>>().To<Repository<Device>>().InRequestScope();
            kernel.Bind<IRepository<UserTelegrams>>().To<Repository<UserTelegrams>>().InRequestScope();
            kernel.Bind<IRepository<UserDetails>>().To<Repository<UserDetails>>().InRequestScope();
            kernel.Bind<IRepository<UserCheckins>>().To<Repository<UserCheckins>>().InRequestScope();
            kernel.Bind<IRepository<CheckinTrackers>>().To<Repository<CheckinTrackers>>().InRequestScope();
            kernel.Bind<IRepository<UserLink>>().To<Repository<UserLink>>().InRequestScope();
            kernel.Bind<IRepository<Links>>().To<Repository<Links>>().InRequestScope();
            kernel.Bind<IRepository<Boost>>().To<Repository<Boost>>().InRequestScope();
            kernel.Bind<IRepository<UserBoost>>().To<Repository<UserBoost>>().InRequestScope();
            kernel.Bind<IRepository<InviteDetails>>().To<Repository<InviteDetails>>().InRequestScope();
            kernel.Bind<IRepository<UserWalletConnection>>().To<Repository<UserWalletConnection>>().InRequestScope();


            kernel.Bind<IDeviceService>().To<DeviceService>().InRequestScope();
            kernel.Bind<IUserTelegramService>().To<UserTelegramService>().InRequestScope();
            kernel.Bind<IUserDetailService>().To<UserDetailService>().InRequestScope();
            kernel.Bind<IUserCheckinsService>().To<UserCheckinsService>().InRequestScope();
            kernel.Bind<ICheckinTrackersService>().To<CheckinTrackersService>().InRequestScope();
            kernel.Bind<IUserLinkService>().To<UserLinkService>().InRequestScope();
            kernel.Bind<ILinkService>().To<LinkService>().InRequestScope();         
            kernel.Bind<IBoostService>().To<BoostService>().InRequestScope();
            kernel.Bind<IUserBoostersService>().To<UserBoostService>().InRequestScope();
            kernel.Bind<IInviteService>().To<InviteService>().InRequestScope();
            kernel.Bind<IUserWallet>().To<UserWalletService>().InRequestScope();
        }
    }
}