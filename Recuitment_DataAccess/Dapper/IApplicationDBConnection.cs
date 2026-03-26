using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Dapper
{
    public interface IApplicationDBConnection
    {
        IDbConnection GetConnection { get; }

        // Execute
        Task<int> ExecuteAsync(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure, IDbTransaction transaction = null, CancellationToken cancellationToken = default);

        // Query    
        Task<List<T>> QueryAsync<T>(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure, IDbTransaction transaction = null, CancellationToken cancellationToken = default);

        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure, IDbTransaction transaction = null, CancellationToken cancellationToken = default);

        Task<T> QuerySingleAsync<T>(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure, IDbTransaction transaction = null, CancellationToken cancellationToken = default);

    }
}
