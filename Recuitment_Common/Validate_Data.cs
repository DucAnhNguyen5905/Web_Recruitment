using System.Security.Cryptography.X509Certificates;

using Recuitment_Model.RequestData;

namespace Recruitment_Common
{
    public static class Validate_Data
    {
        public static void Validate_Data_JobPostInsert(object requestData)
        {
            var jobPostInsert_Request = requestData as JobPostInsert_Request;

            if (jobPostInsert_Request == null)
            {
                throw new ArgumentException("Dữ liệu yêu cầu không hợp lệ.");
            }

            if (string.IsNullOrWhiteSpace(jobPostInsert_Request.Job_Title))
            {
                throw new ArgumentException("Title không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(jobPostInsert_Request.Job_Description))
            {
                throw new ArgumentException("Description không được để trống.");
            }

            if (jobPostInsert_Request.Salary_min > jobPostInsert_Request.Salary_max)
            {
                throw new ArgumentException("Salary min không được lớn hơn salary max.");
            }
        }
    }
}