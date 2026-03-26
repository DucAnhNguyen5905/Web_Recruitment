using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_Model.RequestData
{
    public class JobPostInsert_Request
    {
        public int Employer_ID { get; set; }
        public string? Job_Title { get; set; }
        public string? Job_Description { get; set; }
        public string? Job_Requirements { get; set; }
        public int Salary_min { get; set; }
        public int Salary_max { get; set; }
        public string? Contact_Type { get; set; }
        public int Job_Position_ID { get; set; }
        public int Job_Type_ID { get; set; }
        public int Job_Category_ID { get; set; }
        public int CV_Language_ID { get; set; }

        public string? Office_Name { get; set; }

        public string? Location_Detail { get; set; }

        public string? Keywords { get; set; }

        public string? PostKeyword_ID { get; set; }
        public string? PostLocation_ID { get; set; }
        public DateTime? Expiry_Date { get; set; }

        public List<OfficeItem> Office_List { get; set; }    
        public List<KeywordItem> Keywords_List { get; set; }
        public int JobStatus { get; set; }

    }
    public class OfficeItem
    {
        public int OfficeAddress_ID { get; set; }
    }

    public class KeywordItem
    {
        public int Job_Keywords_ID { get; set; }
    }

}
