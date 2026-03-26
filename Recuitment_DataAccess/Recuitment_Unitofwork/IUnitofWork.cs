using Recuitment_DataAccess.EFCore;
using Recuitment_DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Recuitment_Unitofwork
{
    public interface IUnitofWork
    {
        public  IEmployerRepository _employerRepository { get; set; }
        public IJobPostingRepository _jobPostingRepository { get; set; }
        public Recruitment_DBContext _dbContext { get; set; }
        void Save();
        void Dispose();
        
    }
}
