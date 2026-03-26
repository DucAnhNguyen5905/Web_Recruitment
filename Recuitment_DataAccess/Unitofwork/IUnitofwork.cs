using Microsoft.Identity.Client;
using Recuitment_DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Unitofwork
{
    public interface IUnitofwork
    {
        public  IEmployerRepositoryDapper _employerRepositoryDapper { get; set; }

        public void Save();
        public void Dispose();

    }
}
