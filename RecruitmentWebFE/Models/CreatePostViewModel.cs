using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RecruitmentWebFE.Models
{
    public class CreatePostViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề công việc")]
        public string? JobTitle { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả công việc")]
        public string? JobDescription { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập yêu cầu công việc")]
        public string? JobRequirements { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lương tối thiểu")]
        public int SalaryMin { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lương tối đa")]
        public int SalaryMax { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại liên hệ")]
        public int ContactType { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Job Position ID")]
        public int JobPositionId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Job Type ID")]
        public int JobTypeId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Job Category ID")]
        public int JobCategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập CV Language ID")]
        public int CVLanguageId { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public int JobStatus { get; set; } = 1;
        public List<string> OfficeList { get; set; } = new();
        public List<string> KeywordsList { get; set; } = new();

        public List<SelectListItem> ContactTypeOptions { get; set; } = new();
        public List<SelectListItem> JobPositionOptions { get; set; } = new();
        public List<SelectListItem> JobTypeOptions { get; set; } = new();
        public List<SelectListItem> JobCategoryOptions { get; set; } = new();
        public List<SelectListItem> CVLanguageOptions { get; set; } = new();
    }
}