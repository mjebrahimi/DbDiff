using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.SqlServer.Schema.Generates;
using OpenDBDiff.SqlServer.Schema.Options;
using System.Data.SqlClient;

namespace DbDiffConsole
{
    public static class DatabaseSynchronizer
    {
        public static string CompareAndGenerateSynchronizedScript(string connectionStringSource, string connectionStringDestination)
        {
            try
            {
                var sourceDatabaseName = new SqlConnectionStringBuilder(connectionStringSource).InitialCatalog;
                var destinationDatabaseName = new SqlConnectionStringBuilder(connectionStringDestination).InitialCatalog;

                //It's default option but you can specify you desired option
                var option = new SqlOption();
                var sourceGenerator = new Generate { ConnectionString = connectionStringSource, Options = option };
                var destinationGenerator = new Generate { ConnectionString = connectionStringDestination, Options = option };

                var sourcePair = new KeyValuePair<string, Generate>(sourceDatabaseName, sourceGenerator);
                var destinationPair = new KeyValuePair<string, Generate>(destinationDatabaseName, destinationGenerator);

                Console.WriteLine($"Loading Source Database '{sourceDatabaseName}' ...");
                var source = sourceGenerator.Process();
                Console.WriteLine("Completed.");

                Console.WriteLine($"Loading Destination Database '{destinationDatabaseName}' ...");
                var destination = destinationGenerator.Process();
                Console.WriteLine("Completed.");

                Console.WriteLine("Comparing Databases ...");
                destination = Generate.Compare(source, destination);
                Console.WriteLine("Completed.");

                //You can define schemas you want to compare
                //Empty list means all schemas
                var selectedSchemas = new List<ISchemaBase>();

                Console.WriteLine("Generating Synchronized Script ...");
                var script = destination.ToSqlDiff(selectedSchemas).ToSQL();
                Console.WriteLine("Completed.");

                return script;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
