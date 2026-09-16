using IKimWebService.Infrastructure.IRepository;
using IKimWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Service
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repo;
        private readonly IStoreRepository _storeRepo;

        public AddressService(IAddressRepository repo, IStoreRepository storeRepo)
        {
            _repo = repo;
            _storeRepo = storeRepo;
        }

        public Domain.Address Upsert(Domain.Address mAddress)
        {
            mAddress = _repo.Upsert(mAddress);
            if (mAddress.StoreId >= default(int))
            {
                _storeRepo.UpdateAddress(mAddress.StoreId, mAddress.Id, mAddress.LastModifiedBy);
            }
            return mAddress;
        }

        public Domain.Address Get(int id)
        {
            return _repo.Get(id);
        }
    }
}
