using IKimWebService.Infrastructure.IRepository;
using IKimWebService.Infrastructure.IService;
using System.Collections.Generic;

namespace IKimWebService.Service
{
    public class UserTypeService : IUserTypeService
    {
        private readonly IUserTypeRepository _repo;

        public UserTypeService(IUserTypeRepository repo)
        {
            _repo = repo;
        }

        public List<Domain.UserType> GetAll()
        {
            return _repo.GetAll();
        }
    }
}
