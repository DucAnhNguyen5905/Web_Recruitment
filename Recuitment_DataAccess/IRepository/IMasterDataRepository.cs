using Recuitment_Common.Recuitment_Model.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.IRepository
{
    public interface IMasterDataRepository
    {
        Task<List<DropdownItemResponse>> GetContactTypesAsync();
        Task<List<DropdownItemResponse>> GetJobPositionsAsync();
        Task<List<DropdownItemResponse>> GetJobTypesAsync();
        Task<List<DropdownItemResponse>> GetJobCategoriesAsync();
        Task<List<DropdownItemResponse>> GetCvLanguagesAsync();
        Task<List<DropdownItemResponse>> GetOfficeAddressesAsync(int employerId);
        Task<List<DropdownItemResponse>> GetJobKeywordsAsync();
    }
}
