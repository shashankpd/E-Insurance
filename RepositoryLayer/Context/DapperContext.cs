
﻿using Dapper;
using Microsoft.Extensions.Configuration;
using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Context
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection");
        }
        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public CustomerPolicy GetCustomerPolicyById(int customerPolicyId)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                var query = "SELECT * FROM CustomerPolicies WHERE CustomerPolicyId = @CustomerPolicyId";
                return connection.QueryFirstOrDefault<CustomerPolicy>(query, new { CustomerPolicyId = customerPolicyId });
            }
        }

        public bool RenewPolicy(int customerPolicyId)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();

                // Execute SQL command to update the database and renew the policy
                var commandText = "UPDATE CustomerPolicies SET Renewed = 1 WHERE CustomerPolicyId = @CustomerPolicyId";

                // Execute the command and capture the number of affected rows
                var affectedRows = connection.Execute(commandText, new { CustomerPolicyId = customerPolicyId });

                // Return true if the policy was successfully renewed (affected rows > 0)
                return affectedRows > 0;
            }
        }

    }
}
