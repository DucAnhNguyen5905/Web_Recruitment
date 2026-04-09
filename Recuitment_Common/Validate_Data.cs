using System.Security.Cryptography.X509Certificates;

using Recuitment_Model.RequestData;

namespace Recruitment_Common
{
    public static class Validate_Data
    {
        public static void Validate_Data_JobPostInsert(JobPostInsert_Request request)
        {
            if (request == null)
                throw new ArgumentException("Dữ liệu không hợp lệ.");

            if (request.Employer_ID <= 0)
                throw new ArgumentException("Employer_ID không hợp lệ.");

            if (string.IsNullOrWhiteSpace(request.Job_Title))
                throw new ArgumentException("Vui lòng nhập tiêu đề công việc.");

            if (string.IsNullOrWhiteSpace(request.Job_Description))
                throw new ArgumentException("Vui lòng nhập mô tả công việc.");

            if (string.IsNullOrWhiteSpace(request.Job_Requirements))
                throw new ArgumentException("Vui lòng nhập yêu cầu công việc.");

            if (request.Salary_min < 0 || request.Salary_max < 0)
                throw new ArgumentException("Mức lương không hợp lệ.");

            if (request.Salary_min > request.Salary_max)
                throw new ArgumentException("Lương tối thiểu không được lớn hơn lương tối đa.");

            if (string.IsNullOrWhiteSpace(request.Contact_Type))
                throw new ArgumentException("Vui lòng chọn hình thức liên hệ.");

            if (request.Job_Position_ID <= 0)
                throw new ArgumentException("Job_Position_ID không hợp lệ.");

            if (request.Job_Type_ID <= 0)
                throw new ArgumentException("Job_Type_ID không hợp lệ.");

            if (request.Job_Category_ID <= 0)
                throw new ArgumentException("Job_Category_ID không hợp lệ.");

            if (request.CV_Language_ID <= 0)
                throw new ArgumentException("CV_Language_ID không hợp lệ.");

            if (!request.Expiry_Date.HasValue)
                throw new ArgumentException("Vui lòng chọn ngày hết hạn.");

            if (request.Expiry_Date.Value.Date < DateTime.Today)
                throw new ArgumentException("Ngày hết hạn phải lớn hơn hoặc bằng ngày hiện tại.");

            if (request.Office_List == null || !request.Office_List.Any())
                throw new ArgumentException("Vui lòng chọn ít nhất một văn phòng.");

            if (request.Office_List.Any(x => x.OfficeAddress_ID <= 0))
                throw new ArgumentException("OfficeAddress_ID không hợp lệ.");

            if (request.Keywords_List == null || !request.Keywords_List.Any())
                throw new ArgumentException("Vui lòng chọn ít nhất một keyword.");

            if (request.Keywords_List.Any(x => x.Job_Keywords_ID <= 0))
                throw new ArgumentException("Job_Keywords_ID không hợp lệ.");
        }
    }
}