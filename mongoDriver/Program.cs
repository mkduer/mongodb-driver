using System;
using System.IO;

namespace mongoDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            Data data = null;
            Cluster cluster = null;
            String countries = "";

            // If data needs to be imported, find the workingDirectory containing
            // the dataset and import it. Exit the program if the import failed.
            try
            {
                data = new Data();
                if (data.getDataDirectory())
                { 
                    Console.WriteLine($"\nFound working directory: {data.DataDir}");
                    countries = data.getSubDirectories();
                }
            }
            catch (DirectoryNotFoundException err)
            {
                Console.WriteLine($"\nThe directory containing data was not found. Please check that the directory name or path was not changed:\n{err}");
                Environment.Exit(1);
            }

            // Connect to the existing cluster, database, and collection(s) to run queries
            if (connectCluster(ref cluster))
            {
                try
                {
                    // provide statistics on cluster as proof-of-concept (e.g. count, document contents)

                    Console.WriteLine("\nTotal documents in collection:");
                    Console.WriteLine(cluster.countDocuments());

                    Console.WriteLine("\nDocument Contents:");
                    cluster.displayCollectionDocuments();

                    // Insert data
                    Console.WriteLine($"\nAttempting to import JSON data.");
                    if (cluster.importData(countries))
                        Console.WriteLine("\nYay! Data was successfully imported. Validate by check Atlas.");
                    else
                    {
                        Console.WriteLine("\nCrap -- some error with importing data. Further investigation needed.");
                        Environment.Exit(1);
                    }

                }
                catch (TimeoutException err)
                {
                    Console.WriteLine("\nTimeout Error.\nA common mistake is not white-listing the current IP. Make sure the current IP is WHITELISTED!\n\n\n");
                    Console.WriteLine(err);
                    Environment.Exit(1);
                }
            }
            else
            {
                Console.WriteLine("Failed to connect to cluster, db, or collection.");
            }

            // Print out to confirm the program is ending
            Console.WriteLine("\nProgram successfully ended");
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
                    Console.WriteLine($"\nConnection successfully established with cluster, database {dbName}, and collection {collectionName}.");
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
