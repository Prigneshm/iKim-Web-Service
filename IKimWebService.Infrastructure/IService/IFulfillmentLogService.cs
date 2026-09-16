using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Infrastructure.IService
{
    public interface IFulfillmentLogService
    {
        Domain.FulfillmentLogLister GetAll(Domain.FulfillmentLogLister mLister);

        Domain.FulfillmentLog Upsert(Domain.FulfillmentLog mFulfillmentLog);

        Domain.FulfillmentLog Get(int id);

        void Delete(int id, int loginUserId);
    }
}
