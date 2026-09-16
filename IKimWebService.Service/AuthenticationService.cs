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
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepo;
        private readonly IJWTokenService _jwtService;

        public AuthenticationService(IUserRepository userRepo, IJWTokenService jwtService)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
        }

        public Domain.User Authenticate(Domain.Credential mCredential)
        {
            var mUser = new Domain.User();

            if (mCredential == null)
                throw new APIRequestFailedException("Object must have a value!");

            if (mCredential.EmailAddress.IsNullOrEmpty())
                throw new APIRequestFailedException("Email Address is required!");

            if (mCredential.Password.IsNullOrEmpty())
                throw new APIRequestFailedException("Password is required!");

            mUser = _userRepo.GetBy(mCredential.EmailAddress);
            if (mUser == null)
                throw new APIRequestFailedException("Authentication failed: user identity could not be verified!");

            if (mUser.Password != PasswordEncryptor.Encrypt(mCredential.Password, ConfigurationManager.AppSettings.Get("Salt")))
                throw new APIRequestFailedException("Authentication failed: user identity could not be verified!");

            mUser.Token = _jwtService.GenerateJwtToken(mUser);

            return mUser;
        }
    }
}
