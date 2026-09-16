using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Infrastructure.IRepository
{
    public interface IFulfillmentLogRepository
    {
        Domain.FulfillmentLogLister GetAll(Domain.FulfillmentLogLister mLister);

        Domain.FulfillmentLog Upsert(Domain.FulfillmentLog mItem);

        Domain.FulfillmentLog Get(int id);

        void Delete(int id, int loginUserId);
    }
}
