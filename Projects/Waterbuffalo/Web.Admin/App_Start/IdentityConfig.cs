using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Admin.Infrastructure.UserStore;
using Unity.Core.Model;

namespace Web.Admin
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            var client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential("", ""),
                EnableSsl = true,
            };
            var from  = new MailAddress("", "");
            var to = new MailAddress(message.Destination);

            var mail = new MailMessage(from, to)
            {
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = true,
            };

            client.Send(mail);

            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class IdentityUserManager : UserManager<AppUser, Guid>
    {
        public IdentityUserManager(IUserStore<AppUser, Guid> store)
            : base(store)
        {
            var dataProtectorProvider = Startup.DataProtectionProvider;
            var dataProtector = dataProtectorProvider.Create("EmailConfirmation");
            this.UserTokenProvider = new DataProtectorTokenProvider<AppUser, Guid>(dataProtector)
            {
                TokenLifespan = TimeSpan.FromHours(24),
            };
        }

        public static IdentityUserManager Create(IdentityFactoryOptions<IdentityUserManager> options, IOwinContext context)
        {
            var manager = new IdentityUserManager(new UserStore());

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<AppUser, Guid>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<AppUser, Guid>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<AppUser, Guid>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();

            //var dataProtectionProvider = options.DataProtectionProvider;
            //if (dataProtectionProvider != null)
            //{
            //    manager.UserTokenProvider =
            //        new DataProtectorTokenProvider<ApplicationUser, Guid>(dataProtectionProvider.Create("EmailConfirmation"));
            //}
            return manager;
        }

    }

    // Configure the application sign-in manager which is used in this application.
    public class IdentitySignInManager : SignInManager<AppUser, Guid>
    {
        public IdentitySignInManager(IdentityUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {}

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(AppUser user)
        {
            return user.GenerateUserIdentityAsync((IdentityUserManager)UserManager, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public static IdentitySignInManager Create(IdentityFactoryOptions<IdentitySignInManager> options, IOwinContext context)
        {
            var a = context.GetUserManager<IdentityUserManager>();
            return new IdentitySignInManager(context.GetUserManager<IdentityUserManager>(), context.Authentication);
        }
    }

    public class IdentityRoleManager : RoleManager<AppRole, Guid>
    {
        public IdentityRoleManager(IRoleStore<AppRole, Guid> roleStore) : base(roleStore) { }

        public static IdentityRoleManager Create(IdentityFactoryOptions<IdentityRoleManager> options, IOwinContext context)
        {
            var manager = new IdentityRoleManager(new RoleStore());
            return manager;
        }
    }
}
