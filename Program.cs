using DbDiffConsole;

var sourceConnectionString = @"Data Source=.\MSSQL3;Initial Catalog=Test1;Integrated Security=True";
var destinationConnectionString = @"Data Source=.\MSSQL3;Initial Catalog=Test2;Integrated Security=True";

var script = DatabaseSynchronizer.CompareAndGenerateSynchronizedScript(sourceConnectionString, destinationConnectionString);
Console.WriteLine(script);

Console.ReadLine();

