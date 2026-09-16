using IKimWebService.Infrastructure.CustomException;
using IKimWebService.Infrastructure.IRepository;
using IKimWebService.Infrastructure.IService;
using IKimWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Service
{
    public class ChangePasswordService : IChangePasswordService
    {
        private readonly IUserRepository _userRepo;

        public ChangePasswordService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public void Upsert(Domain.ChangePassword mChangePassword)
        {
            var currentPassword = _userRepo.GetPasswordBy(mChangePassword.UserId);
            if (currentPassword.IsNullOrEmpty())
                throw new APIRequestFailedException("Something is missing at your profile, please contact to administrator!");

            var encryptedNewPassword = PasswordEncryptor.Encrypt(mChangePassword.CurrentPassword, ConfigurationManager.AppSettings.Get("Salt"));

            if (currentPassword != encryptedNewPassword)
                throw new APIRequestFailedException("The current password you entered is incorrect.");

            if (currentPassword != PasswordEncryptor.Encrypt(mChangePassword.CurrentPassword, ConfigurationManager.AppSettings.Get("Salt")))
                throw new APIRequestFailedException("Current password is invalid!");

            _userRepo.SetPasswordBy(mChangePassword.UserId, PasswordEncryptor.Encrypt(mChangePassword.NewPassword, ConfigurationManager.AppSettings.Get("Salt")));
        }

    }
}
