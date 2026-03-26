using Recuitment_DataAccess.Data_Object;
using Recuitment_DataAccess.Data_Object.RequestData;
using Recuitment_Model.RequestData;

namespace Recuitment_DataAccess.IRepository
{
    public interface IEmployerRepository
    {
        Task <ReturnData_Employer> Employer_Delete(EmployerDelete_Request employerDelete_Request);

        Task<InsertReturnData_Employer>  Insert_Employer(EmployerInsert_Request request);

        Task<ReturnData_Employer> Update_Employer(EmployerUpdate_Request request);

        Task<Employer> Login_Employer(EmployerLogin_Request request);

    }
}
