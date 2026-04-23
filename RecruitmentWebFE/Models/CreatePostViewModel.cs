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
        [Range(0, int.MaxValue, ErrorMessage = "Lương tối thiểu không hợp lệ")]
        public int SalaryMin { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lương tối đa")]
        [Range(0, int.MaxValue, ErrorMessage = "Lương tối đa không hợp lệ")]
        public int SalaryMax { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại liên hệ")]
        public string? ContactType { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn vị trí công việc")]
        public int JobPositionId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại công việc")]
        public int JobTypeId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục công việc")]
        public int JobCategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngôn ngữ CV")]
        public int CVLanguageId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày hết hạn")]
        public DateTime? ExpiryDate { get; set; }

        public int JobStatus { get; set; } = 1;
        
        [MinLength(1, ErrorMessage = "Vui lòng chọn văn phòng")]
        public List<int> OfficeAddressIds { get; set; } = new();

        [MinLength(1, ErrorMessage = "Vui lòng chọn keyword")]
        public List<int> JobKeywordIds { get; set; } = new();



        public List<SelectListItem> ContactTypeOptions { get; set; } = new();
        public List<SelectListItem> JobPositionOptions { get; set; } = new();
        public List<SelectListItem> JobTypeOptions { get; set; } = new();
        public List<SelectListItem> JobCategoryOptions { get; set; } = new();
        public List<SelectListItem> CVLanguageOptions { get; set; } = new();
        public List<SelectListItem> OfficeAddressOptions { get; set; } = new();
        public List<SelectListItem> JobKeywordOptions { get; set; } = new();
    }
}