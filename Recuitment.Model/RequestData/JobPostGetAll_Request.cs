using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Recuitment_DataAccess.Data_Object.RequestData
{
    public class JobPostGetAll_Request
    {
        
        public string? Post_ID_List       {get; set;}
        public string? Employer_ID_List   {get; set;}
        public string? Job_Type_List      {get; set;}
        public string? Job_Category_List  {get; set;}

        public string? Search             {get; set;}
        public string? SortBy             {get; set;}
        public string? SortOrder { get; set; }

        public int PostStatus { get; set; }


        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

    }
}
