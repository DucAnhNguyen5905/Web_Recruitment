using Recuitment_DataAccess.Data_Object.RequestData;
using Recuitment_DataAccess.Data_Object;
using Recuitment_Model.RequestData;

namespace Recuitment_DataAccess.IRepository
{
    public interface ICandidateRepositoryDapper
    {
        Task<List<Candidate>> Candidate_GetAll(CandidateGetAll_Request requestData);
        Task<List<Candidate>> Candidate_GetById(CandidateGetAll_Request requestData);
        Task<Candidate> Candidate_UpLoadCv(CandidateUpLoadCv_Request requestData);
    }
}
