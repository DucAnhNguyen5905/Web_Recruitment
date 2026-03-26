using Recuitment_DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using Recuitment_DataAccess.Data_Object;
using System.Text;
using Recuitment_DataAccess.Recuitment_Unitofwork;
using System.Threading.Tasks;
using Recuitment_DataAccess.EFCore;
using System.Runtime.InteropServices.JavaScript;


namespace Recuitment_DataAccess.Repository
{
    public class FunctionRepository : IFunctionRepository
    {
        public readonly Recruitment_DBContext _dbContext;
        public FunctionRepository(Recruitment_DBContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<Function> GetFunctionByCode(string functionCode)
        {
            return _dbContext.function.Where(f => f.FunctionCode == functionCode).FirstOrDefault();
        }


    }
}
