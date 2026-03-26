using Recuitment_DataAccess.Data_Object;
using Recuitment_DataAccess.Data_Object.RequestData;
using Recuitment_DataAccess.IRepository;
using Recuitment_DataAccess.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Recuitment_Model.RequestData;

namespace Recuitment_DataAccess.Repository
{
    public class JobPostingRepository : IJobPostingRepository
    {
        private Recruitment_DBContext _dbcontext;
        public JobPostingRepository(Recruitment_DBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<JobPost>> Get_All_JobPosts()
        {
            var list = new List<JobPost>();
            try
            {
                list = await _dbcontext.jobpost.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tìm kiếm bài đăng: {ex.Message}");
            }
            return list;
        }

        public Task<InsertReturnData> Insert_JobPost(JobPostInsert_Request jobPostInsert_Request)
        {
            var result = new InsertReturnData();
            try
            {
                var jobPost = new JobPost
                {
                    Employer_ID = jobPostInsert_Request.Employer_ID,
                    Job_Title = jobPostInsert_Request.Job_Title,
                    Job_Description = jobPostInsert_Request.Job_Description,
                    Job_Requirements = jobPostInsert_Request.Job_Requirements,
                    Salary_min = jobPostInsert_Request.Salary_min,
                    Salary_max = jobPostInsert_Request.Salary_max,
                    Created_at = DateTime.Now,
                    Contact_Type = jobPostInsert_Request.Contact_Type,
                    Job_Position_ID = jobPostInsert_Request.Job_Position_ID,
                    Job_Type_ID = jobPostInsert_Request.Job_Type_ID,
                    Job_Category_ID = jobPostInsert_Request.Job_Category_ID,
                    CV_Language_ID = jobPostInsert_Request.CV_Language_ID,
                    PostStatus = jobPostInsert_Request.JobStatus


                };

                _dbcontext.jobpost.Add(jobPost);
                _dbcontext.SaveChanges();
                result.Post_ID = jobPost.Post_ID;
                result.ResponseCode = 0;
                result.ResponseMessage = "Tạo bài đăng thành công.";
            }
            catch (Exception ex)
            {
                result.ResponseCode = -1;
                result.ResponseMessage = $"Lỗi khi tạo bài đăng: {ex.Message}";
            }
            return Task.FromResult(result);

        }

        public Task<ReturnData_JobPost> JobPost_Delete(JobPostDelete_Request jobPostDelete_Request)
        {
            throw new NotImplementedException();
        }

        public Task<ReturnData_JobPost> JobPost_Update(JobPostUpdate_Request jobPostUpdate_Request)
        {
            throw new NotImplementedException();
        }
    }
}
