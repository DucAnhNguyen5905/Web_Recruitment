using Recuitment_DataAccess.Data_Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.IRepository
{
    public interface IPermissionRepository
    {
        Task<Permission> CheckPermission(int employerId, int functionID);
    }
}
