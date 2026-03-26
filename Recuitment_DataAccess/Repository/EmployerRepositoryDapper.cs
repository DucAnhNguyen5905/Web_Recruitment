using Azure.Core;
using crypto;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Crypto.Generators;
using Recuitment_Common;
using Recuitment_DataAccess.Dapper;
using Recuitment_DataAccess.Data_Object;
using Recuitment_DataAccess.Data_Object.RequestData;
using Recuitment_DataAccess.EFCore;
using Recuitment_DataAccess.IRepository;
using Recuitment_Model.RequestData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Recuitment_DataAccess.Repository
{
    public class EmployerRepositoryDapper : BaseApplicationService, IEmployerRepositoryDapper
    {
        private readonly Recruitment_DBContext _dbContext;

        public EmployerRepositoryDapper(IServiceProvider serviceProvider, Recruitment_DBContext dbContext) : base(serviceProvider)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Employer>>Employer_GetAll(EmployerGetAll_Request requestData)
        {
            var list = new  List<Employer>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@Employer_ID",requestData.Employer_ID);
                list = await DbConnection.QueryAsync<Employer>("SP_Employer_GetAll", param);
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> Employer_Delete(EmployerDelete_Request requestData)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Employer_ID", requestData.Employer_ID);
                param.Add("ResponseCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await DbConnection.ExecuteAsync(
                    "SP_Employer_Delete",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                int responseCode = param.Get<int>("ResponseCode");
                return responseCode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> Employer_Insert(EmployerInsert_Request requestData)
        {
            var hashPassword = SecurityCommon.ComputeSha256Hash(requestData.Password);

            var param = new DynamicParameters();

            param.Add("Name", requestData.Name);
            param.Add("EmployerStatus", requestData.EmployerStatus);
            param.Add("Password", hashPassword);
            param.Add("Email", requestData.Email);
            param.Add("FullName", requestData.FullName);
            param.Add("Gender", requestData.Gender);
            param.Add("Company_Name", requestData.Company_Name);
            param.Add("Company_Description", requestData.Company_Description);
            param.Add("Benefits_ID", requestData.Benefits_ID);
            param.Add("Avartar", requestData.Avartar);
            param.Add("ResponseCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await DbConnection.ExecuteAsync("SP_Employer_Insert", param, commandType: CommandType.StoredProcedure);

            return param.Get<int>("ResponseCode");
        }

        public async Task<Employer> Employer_Login(EmployerLogin_Request requestData)
        {
            try
            {
                var hashPassword = SecurityCommon.ComputeSha256Hash(requestData.Password);

                var result = await _dbContext.employer
                    .Where(x => x.Email == requestData.Email && x.Password == hashPassword)
                    .FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }

}
