using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Recuitment_Common.Recuitment_Model.ResponseData;
using Recuitment_DataAccess.IRepository;
using Recuitment_Model.RequestData;
using System.Data;

namespace Recuitment_DataAccess.Repository
{
    public class MasterDataRepository : IMasterDataRepository
    {
        private readonly string _connectionString;

        public MasterDataRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Ten_ConnStr")!;
        }

        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<List<DropdownItemResponse>> GetContactTypesAsync()
        {
            using var connection = CreateConnection();
            var result = await connection.QueryAsync<DropdownItemResponse>(
                "SP_ContactType_GetAll",
                commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<List<DropdownItemResponse>> GetJobPositionsAsync()
        {
            using var connection = CreateConnection();
            var result = await connection.QueryAsync<DropdownItemResponse>(
                "SP_JobPosition_GetAll",
                commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<List<DropdownItemResponse>> GetJobTypesAsync()
        {
            using var connection = CreateConnection();
            var result = await connection.QueryAsync<DropdownItemResponse>(
                "SP_JobType_GetAll",
                commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<List<DropdownItemResponse>> GetJobCategoriesAsync()
        {
            using var connection = CreateConnection();
            var result = await connection.QueryAsync<DropdownItemResponse>(
                "SP_JobCategory_GetAll",
                commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<List<DropdownItemResponse>> GetCvLanguagesAsync()
        {
            using var connection = CreateConnection();
            var result = await connection.QueryAsync<DropdownItemResponse>(
                "SP_CVLanguage_GetAll",
                commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<List<DropdownItemResponse>> GetOfficeAddressesAsync(int employerId)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Employer_ID", employerId);

            var result = await connection.QueryAsync<DropdownItemResponse>(
                "SP_OfficeAddress_GetByEmployer",
                parameters,
                commandType: CommandType.StoredProcedure);


            return result.ToList();
        }

        public async Task<List<DropdownItemResponse>> GetJobKeywordsAsync()
        {
            using var connection = CreateConnection();
            var result = await connection.QueryAsync<DropdownItemResponse>(
                "SP_JobKeyword_GetAll",
                commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
    }
}