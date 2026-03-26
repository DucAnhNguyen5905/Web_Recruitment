using Recuitment_DataAccess.Data_Object;
using Recuitment_DataAccess.EFCore;
using Recuitment_DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        public readonly Recruitment_DBContext _dbContext;

        public PermissionRepository(Recruitment_DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Permission> CheckPermission(int employerId, int functionID)
        {
            return _dbContext.permission.Where(p => p.EmployerID == employerId && p.FunctionID == functionID).FirstOrDefault();
        }
    }
}
