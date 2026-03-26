using Microsoft.EntityFrameworkCore;
using Recuitment_DataAccess.EFCore;
using Recuitment_DataAccess.IRepository;
using Recuitment_DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Recuitment_Unitofwork
{
    public class UnitofWork : IUnitofWork
    {
        public IEmployerRepository _employerRepository { get ; set; }

        public IJobPostingRepository _jobPostingRepository { get; set; }
        public Recruitment_DBContext _dbContext { get; set; }

        public UnitofWork(IEmployerRepository employerRepository, Recruitment_DBContext dbContext, IJobPostingRepository jobPostingRepository )
        {
            _employerRepository = employerRepository;
            _jobPostingRepository = jobPostingRepository;
            _dbContext = dbContext;
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }

}
