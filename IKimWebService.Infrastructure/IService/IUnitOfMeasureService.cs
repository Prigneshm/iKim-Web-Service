using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Infrastructure.IService
{
    public interface IUnitOfMeasureService
    {
        List<Domain.UnitOfMeasure> GetAll();
    }
}
