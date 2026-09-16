using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Infrastructure.IService
{
    public interface IAddressService
    {
        Domain.Address Upsert(Domain.Address mAddress);

        Domain.Address Get(int id);
    }
}
