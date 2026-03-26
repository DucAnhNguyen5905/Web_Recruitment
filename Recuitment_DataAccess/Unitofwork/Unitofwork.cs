using Recuitment_DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Unitofwork
{
    public class Unitofwork : IUnitofwork
    {


        public IEmployerRepositoryDapper _employerRepositoryDapper { get; set ; }
        public Unitofwork(IEmployerRepositoryDapper employerRepositoryDapper)
        {
            _employerRepositoryDapper = employerRepositoryDapper;   
        }
        void IUnitofwork.Dispose()
        {
            throw new NotImplementedException();
        }

        void IUnitofwork.Save()
        {
            throw new NotImplementedException();
        }
    }
}
