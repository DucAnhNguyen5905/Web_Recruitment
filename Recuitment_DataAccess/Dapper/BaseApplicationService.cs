using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Dapper
{
    public class BaseApplicationService
    {
        public BaseApplicationService(IServiceProvider serviceProvider)
        {
            DbConnection = serviceProvider.GetRequiredService<IApplicationDBConnection>();
        }
        protected IApplicationDBConnection DbConnection { get; }
    }

}
