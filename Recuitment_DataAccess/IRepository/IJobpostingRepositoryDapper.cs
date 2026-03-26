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
    public interface IJobpostingRepositoryDapper
    {
        Task<int> Jobposting_Insert(JobPostInsert_Request requestData);
        Task<int> Jobposting_Delete(JobPostDelete_Request requestData);
        Task<int> Jobposting_Update(JobPostUpdate_Request requestData);
        Task<int> Jobposting_Update_Partial(JobPostUpdate_Request requestData);
        Task<List<JobPost>> Get_All_JobPosts(JobPostGetAll_Request requestData);
        Task<List<JobPost>> Get_JobPost_By_Id(JobPostGetById_Request requestData);


    }
}
