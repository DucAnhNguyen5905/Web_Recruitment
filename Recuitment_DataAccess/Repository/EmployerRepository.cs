using Microsoft.EntityFrameworkCore;
using Recuitment_DataAccess.Data_Object;
using Recuitment_DataAccess.EFCore;
using Recuitment_DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recuitment_DataAccess.Data_Object.RequestData;
using Recuitment_Model.RequestData;

namespace Recuitment_DataAccess.Repository
{
    public class EmployerRepository : IEmployerRepository
    {
        private Recruitment_DBContext _dbContext;


        public EmployerRepository(Recruitment_DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ReturnData_Employer> Employer_Delete(EmployerDelete_Request employerDelete_Request)
        {
            var result = new ReturnData_Employer();
            try
            {
                var employer = _dbContext.employer.Where(x => x.Employer_ID == employerDelete_Request.Employer_ID).FirstOrDefault();
                if (employer == null)
                {
                    result.ResponseCode = -1;
                    result.ResponseMessage = "Employer not found.";
                    return result;
                }

                _dbContext.employer.Remove(employer);
                await _dbContext.SaveChangesAsync();

                result.ResponseCode = 0;
                result.ResponseMessage = "Employer was deleted successfully.";
            }
            catch (Exception ex)
            {
                result.ResponseCode = -1;
                result.ResponseMessage = $"Error deleting employer: {ex.Message}";
            }

            return result;
        }

        public async Task<InsertReturnData_Employer> Insert_Employer(EmployerInsert_Request employerInsert_Request)
        {
            var result = new InsertReturnData_Employer();
            try
            {
                var employer = new Employer
                {
                    Employer_ID = employerInsert_Request.Employer_ID,
                    Name = employerInsert_Request.Name,
                    Password = employerInsert_Request.Password,
                    Email = employerInsert_Request.Email,
                    FullName = employerInsert_Request.FullName,
                    Gender = employerInsert_Request.Gender,
                    Company_Name = employerInsert_Request.Company_Name,
                    Company_Description = employerInsert_Request.Company_Description,
                    Created_at = DateTime.Now,
                    Benefits_ID = employerInsert_Request.Benefits_ID ?? 0,
                    EmployerStatus = employerInsert_Request.EmployerStatus ?? 0,
                    Avartar = employerInsert_Request.Avartar
                };

                _dbContext.employer.Add(employer);
                await _dbContext.SaveChangesAsync();

                result.ResponseCode = 0;
                result.ResponseMessage = "Employer inserted successfully.";
                result.Employer_ID = employer.Employer_ID;
            }
            catch (Exception ex)
            {
                result.ResponseCode = -1;
                result.ResponseMessage = $"Error inserting employer: {ex.InnerException?.Message ?? ex.Message}";
            }

            return result;
        }

        public async Task<ReturnData_Employer> Update_Employer(EmployerUpdate_Request employerUpdate_request)
        {
            var result = new ReturnData_Employer();
            try
            {
                var employer = await _dbContext.employer
                    .FirstOrDefaultAsync(x => x.Employer_ID == employerUpdate_request.Employer_ID);

                if (employer == null)
                {
                    result.ResponseCode = -1;
                    result.ResponseMessage = "Employer not found.";
                    return result;
                }

                // Cập nhật các trường
                employer.Name = employerUpdate_request.Name;
                employer.Password = employerUpdate_request.Password;
                employer.Email = employerUpdate_request.Email;
                employer.FullName = employerUpdate_request.FullName;
                employer.Company_Name = employerUpdate_request.Company_Name;
                employer.Company_Description = employerUpdate_request.Company_Description;

                await _dbContext.SaveChangesAsync();


                result.ResponseCode = 0;
                result.ResponseMessage = "Employer was updated successfully.";
            }
            catch (Exception ex)
            {

                result.ResponseCode = -1;
                result.ResponseMessage = $"Error inserting employer: {ex.InnerException?.Message ?? ex.Message}";
            }
            return result;
        }

        public async Task<Employer> Login_Employer(EmployerLogin_Request request)
        {


            try
            {
                var result = await _dbContext.employer
                     .FirstOrDefaultAsync(x => x.Name == request.Email && x.Password == request.Password);
                return result;

            }
            catch (Exception )
            {
                throw;
            }

        }
    }

}
