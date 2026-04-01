using Azure;
using BCrypt.Net;
using Dapper;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Recuitment_DataAccess.Dapper;
using Recuitment_DataAccess.Data_Object;
using Recuitment_DataAccess.Data_Object.RequestData;
using Recuitment_DataAccess.IRepository;
using Recuitment_Model.RequestData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Repository
{
    public class JobPostingRepositoryDapper: BaseApplicationService, IJobpostingRepositoryDapper
    {
        public JobPostingRepositoryDapper(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<int> Jobposting_Insert(JobPostInsert_Request requestData)
        {
            var param = new DynamicParameters();
            param.Add("Employer_ID", requestData.Employer_ID);
            param.Add("Job_Title", requestData.Job_Title);
            param.Add("Job_Description", requestData.Job_Description);
            param.Add("Job_Requirements", requestData.Job_Requirements);
            param.Add("Salary_min", requestData.Salary_min);
            param.Add("Salary_max", requestData.Salary_max);
            param.Add("Contact_Type", requestData.Contact_Type);
            param.Add("Job_Position_ID", requestData.Job_Position_ID);
            param.Add("Job_Type_ID", requestData.Job_Type_ID);
            param.Add("Job_Category_ID", requestData.Job_Category_ID);
            param.Add("CV_Language_ID", requestData.CV_Language_ID);

            // Convert list to JSON
            var officeJson = JsonConvert.SerializeObject(requestData.Office_List);
            var keywordJson = JsonConvert.SerializeObject(requestData.Keywords_List);
            param.Add("OfficeListJson", officeJson);
            param.Add("KeywordListJson", keywordJson);

            param.Add("Expiry_Date", requestData.Expiry_Date.HasValue ?
                (object)requestData.Expiry_Date.Value.AddHours(7) : DBNull.Value);
            param.Add("PostStatus", requestData.JobStatus);
            param.Add("ResponseCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await DbConnection.ExecuteAsync("SP_JobPost_Insert", param, commandType: CommandType.StoredProcedure);
            return param.Get<int>("ResponseCode");
        }


        public async Task<int> Jobposting_Delete(JobPostDelete_Request requestData)
        {
            var param = new DynamicParameters();
            param.Add("Post_ID", requestData.Post_ID);
            param.Add("ResponseCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await DbConnection.ExecuteAsync("SP_JobPost_Delete", param, commandType: CommandType.StoredProcedure);

            var response = param.Get<int>("ResponseCode");
            return response;
        }
        public async Task<int> Jobposting_Update(JobPostUpdate_Request requestData)
        {
            var param = new DynamicParameters();

            
            param.Add("Employer_ID", requestData.Employer_ID);
            param.Add("Job_Title", requestData.Job_Title);
            param.Add("Job_Description", requestData.Job_Description);
            param.Add("Job_Requirements", requestData.Job_Requirements);
            param.Add("Salary_min", requestData.Salary_min);
            param.Add("Salary_max", requestData.Salary_max);
            param.Add("Contact_Type", requestData.Contact_Type);
            param.Add("Job_Position_ID", requestData.Job_Position_ID);
            param.Add("Job_Type_ID", requestData.Job_Type_ID);
            param.Add("CV_Language_ID", requestData.CV_Language_ID);
            param.Add("PostStatus", requestData.PostStatus);
            param.Add("ResponseCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await DbConnection.ExecuteAsync("SP_JobPost_Update", param, commandType: CommandType.StoredProcedure);

            var response = param.Get<int>("ResponseCode");
            return response;
        }

        public async Task<List<JobPost>>Get_All_JobPosts(JobPostGetAll_Request requestData)
        {

            var list = new List<JobPost>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@Post_ID_List", requestData.Post_ID_List);

                param.Add("@Employer_ID_List", requestData.Employer_ID_List); 
                param.Add("@Job_Type_List", requestData.Job_Type_List); 
                param.Add("@Job_Category_List", requestData.Job_Category_List);
                param.Add("@PostStatus", requestData.PostStatus);
                param.Add("@FromDate", requestData.FromDate ?? new DateTime(2000, 1, 1));
                param.Add("@ToDate", requestData.ToDate ?? new DateTime(2030, 1, 1));

                param.Add("@Search", requestData.Search);
                param.Add("@SortBy", requestData.SortBy);
                param.Add("@SortOrder", requestData.SortOrder);

                param.Add("@CurrentEmployerID", requestData.CurrentEmployerID);
                param.Add("@IsAdmin", requestData.IsAdmin);
                list = await DbConnection.QueryAsync<JobPost>("SP_JobPost_GetAll", param);
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<JobPost>> Get_JobPost_By_Id(JobPostGetById_Request requestData)
        {
            var list = new List<JobPost>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@Post_ID", requestData.Post_ID);
                list = await DbConnection.QueryAsync<JobPost>("SP_JobPost_GetById", param);
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> Jobposting_Update_Partial(JobPostUpdate_Request requestData)
        {
            var param = new DynamicParameters();

            param.Add("Post_ID", requestData.Post_ID);
            param.Add("Employer_ID", requestData.Employer_ID);
            param.Add("Job_Title", requestData.Job_Title);
            param.Add("Job_Description", requestData.Job_Description);
            param.Add("Job_Requirements", requestData.Job_Requirements);
            param.Add("Salary_min", requestData.Salary_min);
            param.Add("Salary_max", requestData.Salary_max);
            param.Add("Contact_Type", requestData.Contact_Type);
            param.Add("Job_Position_ID", requestData.Job_Position_ID);
            param.Add("Job_Type_ID", requestData.Job_Type_ID);
            param.Add("CV_Language_ID", requestData.CV_Language_ID);
            param.Add("PostStatus", requestData.PostStatus);
            param.Add("ResponseCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await DbConnection.ExecuteAsync("SP_JobPost_Update", param, commandType: CommandType.StoredProcedure);

            var response = param.Get<int>("ResponseCode");
            return response;
        }
    }


}
