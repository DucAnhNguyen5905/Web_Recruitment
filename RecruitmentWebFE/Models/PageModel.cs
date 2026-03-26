using System.ComponentModel.DataAnnotations;

namespace RecruitmentWebFE.Models
{
    public class PageModel
    {
        [MaxLength(2)]
        public String? PageTitle { get; set; }
    }
}
