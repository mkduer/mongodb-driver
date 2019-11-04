using System;

namespace mongoDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello From Driver!");
            String connection = "mongodb+srv://test:test@airbnb-oluyv.mongodb.net/test?retryWrites=true&w=majority";
            String databaseName = "dbname";
            String collectionName = "collectionname";

            Cluster cluster = new Cluster();
            cluster.Connection = connection;

            try
            {
                // establish connection with cluster, create specific database and collection variables
                cluster.establishConnection();
                cluster.accessDb(databaseName);
                cluster.accessCollection(collectionName);
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("\nConnection was not established. Please check configuration, credentials and connection details");
                Environment.Exit(1);
            }

            // provide statistics on cluster as proof-of-concept (e.g. count, document contents)
            Console.WriteLine($"total documents in collection: {cluster.countDocuments()}");
            cluster.displayCollectionDocuments();

            // Temporary print out to confirm the program is ending correctly
            Console.WriteLine("Program successfully ended");
        }
    }
}
