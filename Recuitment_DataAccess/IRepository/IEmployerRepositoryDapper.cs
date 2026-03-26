using Recuitment_DataAccess.Data_Object;
using Recuitment_DataAccess.Data_Object.RequestData;
using Recuitment_Model.RequestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.IRepository
{
    public interface IEmployerRepositoryDapper
    {
        Task<List<Employer>> Employer_GetAll(EmployerGetAll_Request requestData);

        Task<int> Employer_Insert(EmployerInsert_Request requestData);

        Task<int> Employer_Delete(EmployerDelete_Request requestData);

        Task<Employer> Employer_Login(EmployerLogin_Request requestData);
    }
}
