using Recuitment_DataAccess.EFCore;
using Recuitment_DataAccess.IRepository;
using System;

namespace Recuitment_DataAccess.Recuitment_Unitofwork
{
    public class UnitofWork : IUnitofWork
    {
        public Recruitment_DBContext _dbContext { get; set; }

        public IEmployerRepositoryDapper EmployerRepositoryDapper { get; }

        public UnitofWork(
            Recruitment_DBContext dbContext,
            IEmployerRepositoryDapper employerRepositoryDapper
        )
        {
            _dbContext = dbContext;
            EmployerRepositoryDapper = employerRepositoryDapper;
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