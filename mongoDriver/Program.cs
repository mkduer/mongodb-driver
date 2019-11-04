using System;
using System.IO;

namespace mongoDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello From Driver!");
            DataSet data = null;
            Cluster cluster = null;

            // If data needs to be imported, find the workingDirectory containing
            // the dataset and import it. Exit the program if the import failed.
            try
            {
                data = new DataSet();
                if (data.getWorkingDir())
                    Console.WriteLine($"Found working directory: {data.WorkingDir}");
                else
                    Environment.Exit(1);
            }
            catch (DirectoryNotFoundException err)
            {
                Console.WriteLine($"The file storing data was not found. " +
                    $"Please check that the directory name or path was not changed:\n{err}");
            }

            /*

            // Connect to the existing cluster, database, and collection(s) to run queries
            if (connectCluster(ref cluster))
            {
                // provide statistics on cluster as proof-of-concept (e.g. count, document contents)
                Console.WriteLine($"total documents in collection: {cluster.countDocuments()}");
                cluster.displayCollectionDocuments();
            }
            */

            // Print out to confirm the program is ending
            Console.WriteLine("Program successfully ended");
        }

        /// <summary>establish connection with cluster, create specific database and collection variables</summary>
        /// <param name="cluster">A Cluster instance</param>
        /// <returns>
        /// True if cluster, database and collection are successfully established and accessed
        /// False if an error occurred or an exception was thrown
        /// </returns>
        static bool connectCluster(ref Cluster cluster)
        {
            cluster = new Cluster();
            cluster.Connection = "mongodb+srv://test:test@airbnb-oluyv.mongodb.net/test?retryWrites=true&w=majority";
            String dbName = "testingDB";
            String collectionName = "testingCollection";

            try
            {
                if (cluster.establishConnection() && cluster.accessDb(dbName) && cluster.accessCollection(collectionName)) {
                    return true;
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("\nConnection was not established. Please check configuration, credentials and connection details");
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("\nConnection was not established. The cluster, database or collection does not exist.");
            }
            catch (ArgumentException)
            { 
                Console.WriteLine("\nConnection was not established. An invalid name was used for access.");
            }
            return false;
        }
    }

}
