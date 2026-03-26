using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_Model.RequestData
{
    public class CandidateUpLoadCv_Request
    {
        public int candidate_id { get; set; }
        public string? cv_file_path { get; set; }
    }
}
