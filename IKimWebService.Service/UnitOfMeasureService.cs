using IKimWebService.Infrastructure.IRepository;
using IKimWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Service
{
    public class UnitOfMeasureService : IUnitOfMeasureService
    {
        private readonly IUnitOfMeasureRepository _repo;

        public UnitOfMeasureService(IUnitOfMeasureRepository repo)
        {
            _repo = repo;
        }

        public List<Domain.UnitOfMeasure> GetAll()
        {
            return _repo.GetAll();
        }

    }
}
