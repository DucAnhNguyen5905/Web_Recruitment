using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recuitment_DataAccess.Data_Object;

namespace Recuitment_DataAccess.IRepository
{
    public interface IFunctionRepository
    {
        Task<Function> GetFunctionByCode(string FunctionCode);
    }
}
