using Recuitment_DataAccess.EFCore;
using Recuitment_DataAccess.IRepository;
using System;

namespace Recuitment_DataAccess.Recuitment_Unitofwork
{
    public interface IUnitofWork
    {
        Recruitment_DBContext _dbContext { get; set; }

        IEmployerRepositoryDapper EmployerRepositoryDapper { get; }

        void Save();
        void Dispose();
    }
}