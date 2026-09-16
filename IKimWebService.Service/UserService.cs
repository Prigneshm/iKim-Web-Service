using IKimWebService.Infrastructure.CustomException;
using IKimWebService.Infrastructure.IRepository;
using IKimWebService.Infrastructure.IService;
using IKimWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace IKimWebService.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IFileReaderService _fileReaderService;
        private readonly IEmailSenderService _emailSenderService;

        public UserService(IUserRepository repo, IEmailSenderService emailSenderService, IFileReaderService fileReaderService)
        {
            _repo = repo;
            _emailSenderService = emailSenderService;
            _fileReaderService = fileReaderService;
        }

        public Domain.UserLister GetAll(Domain.UserLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.User Upsert(Domain.User mUser)
        {
            bool isNewUser = false;
            if (mUser == null)
                throw new APIRequestFailedException("User object must not be null.");
            else if (mUser.Id == default(int))
                isNewUser = true;

            SetUserPassword(mUser);

            var savedUser = _repo.Upsert(mUser);

            if (IsValidUser(savedUser) && isNewUser)
                SendEmailOnCreateUser(savedUser);

            return savedUser;
        }

        public Domain.User Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public bool CheckEmailAddressExist(Domain.User mUser)
        {
            return _repo.CheckEmailAddressExist(mUser);
        }

        #region Private Methods
        private void SendEmailOnCreateUser(Domain.User mUser)
        {
            string content = _fileReaderService.ReadFileContent("~/EmailTemplates/SendCredentialsOnCreateUser.html");
            string finalContent = PopulateEmailTemplate(content, mUser);
            Task.Run(async () =>
            {
                try
                {
                    await _emailSenderService.SendEmailAsync(finalContent, mUser.EmailAddress, "Welcome to iKim!");
                }
                catch (Exception ex)
                {
                    System.IO.File.AppendAllText("ErrorLog.txt", $"{DateTime.Now}: Background send failed - {ex.Message}{Environment.NewLine}");
                }
            });
        }

        private string PopulateEmailTemplate(string template, Domain.User mUser)
        {
            var replacements = new Dictionary<string, string>
            {
                { "{{FirstName}}", mUser.FirstName },
                { "{{EmailAddress}}", mUser.EmailAddress },
                { "{{Password}}", mUser.PlainTextPassword},
            };

            foreach (var pair in replacements)
            {
                template = template.Replace(pair.Key, pair.Value);
            }

            return template;
        }

        private void SetUserPassword(Domain.User user)
        {
            user.PlainTextPassword = StaticMethods.GeneratePassword(8);
            var salt = ConfigurationManager.AppSettings.Get("Salt");
            user.Password = PasswordEncryptor.Encrypt(user.PlainTextPassword, salt);
        }


        private bool IsValidUser(Domain.User user)
        {
            return user != null && user.Id > 0;
        }
        #endregion
    }
}
