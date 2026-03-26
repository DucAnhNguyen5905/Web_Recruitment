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
    public interface IJobPostingRepository
    {
        Task<ReturnData_JobPost> JobPost_Delete(JobPostDelete_Request postDelete_Request);
        Task<InsertReturnData> Insert_JobPost(JobPostInsert_Request postInsert_Request);
        Task<ReturnData_JobPost> JobPost_Update(JobPostUpdate_Request postUpdate_Request);
        Task<List<JobPost>> Get_All_JobPosts();
    }
}
