using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApiEmploye
{

    [TestClass]
    public static class TestEmploye
    {

      
            [AssemblyInitialize]
            public static void InitialiserAssembly(TestContext context)
            {
                string SqlServerInstance = "(localdb)\\mssqllocaldb";
                string dbName = "DbangolaTest1";

                // Restaure la sauvegarde de la base de données
                //string backupPath = $@"D:\BDD\Backups\{dbName}.bak";
                string backupPath = $@"C:\Users\HP\Desktop\projet\asp.net.mvc6\gestion_employeV2\{dbName}.bak";
            using SqlConnection connection = new SqlConnection($"Server={SqlServerInstance};Trusted_Connection=True");
                connection.Open();

                string sql = $"RESTORE DATABASE {dbName} FROM DISK = '{backupPath}' WITH REPLACE;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            
        }
    }
}
