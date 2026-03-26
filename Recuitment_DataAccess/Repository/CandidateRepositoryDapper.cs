using Dapper;
using Recuitment_DataAccess.Dapper;
using Recuitment_DataAccess.Data_Object;
using Recuitment_DataAccess.EFCore;
using Recuitment_DataAccess.IRepository;
using Recuitment_Model.RequestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Repository
{
    public class CandidateRepositoryDapper
    : BaseApplicationService, ICandidateRepositoryDapper
    {
        private readonly Recruitment_DBContext _dbContext;

        public CandidateRepositoryDapper(
            IServiceProvider serviceProvider,
            Recruitment_DBContext dbContext)
            : base(serviceProvider)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Candidate>> Candidate_GetAll(CandidateGetAll_Request requestData)
        {
            var list = new List<Candidate>();

            try
            {


                list = await DbConnection.QueryAsync<Candidate>("SP_Candidate_GetAll");
                return list;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Candidate>> Candidate_GetById(CandidateGetAll_Request requestData)
        {
            var list = new List<Candidate>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@Candidate_ID", requestData.candidate_id);
                list = (await DbConnection.QueryAsync<Candidate>(
                    "SP_Candidate_GetById",
                    param)).ToList();
                return list;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Candidate> Candidate_UpLoadCv(CandidateUpLoadCv_Request requestData)
        {
            var candidate = new Candidate();
            try
            {
                var param = new DynamicParameters();
                param.Add("@Candidate_ID", requestData.candidate_id);
                param.Add("@Candidate_CV", requestData.cv_file_path);
                candidate = await DbConnection.QueryFirstOrDefaultAsync<Candidate>(
                    "SP_Candidate_UpCv",
                    param);
                return candidate;
            }
            catch
            {
                throw;
            }
        }
    }

}
