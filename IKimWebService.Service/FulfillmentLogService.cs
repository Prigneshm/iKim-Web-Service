using IKimWebService.Infrastructure.CustomException;
using IKimWebService.Infrastructure.Helper;
using IKimWebService.Infrastructure.IRepository;
using IKimWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Service
{
    public class FulfillmentLogService : IFulfillmentLogService
    {
        private readonly IFulfillmentLogRepository _repo;

        public FulfillmentLogService(IFulfillmentLogRepository repo)
        {
            _repo = repo;
        }

        public Domain.FulfillmentLogLister GetAll(Domain.FulfillmentLogLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.FulfillmentLog Upsert(Domain.FulfillmentLog mFulfillmentLog)
        {
            int qtyRequested = mFulfillmentLog.QuantityRequested;
            int qtyfulfilled = mFulfillmentLog.QuantityFulfilled;

            if (qtyfulfilled > qtyRequested)
                throw new APIRequestFailedException("Please ensure the fulfilled quantity does not exceed the requested quantity.");
            else if (qtyfulfilled == 0)
                mFulfillmentLog.Status = EnumsHelper.GetEnumString(Infrastructure.Utils.Enums.Status.NextDay);
            else if (qtyfulfilled < qtyRequested)
                mFulfillmentLog.Status = EnumsHelper.GetEnumString(Infrastructure.Utils.Enums.Status.Partially_Fulfilled);
            else if (qtyfulfilled == qtyRequested)
                mFulfillmentLog.Status = EnumsHelper.GetEnumString(Infrastructure.Utils.Enums.Status.Fulfilled);

            return _repo.Upsert(mFulfillmentLog);
        }

        public Domain.FulfillmentLog Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }
    }
}
