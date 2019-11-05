using System;
using MongoDB.Driver;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace mongoDriver
{
    public class Cluster
    {
        private String _connection;
        public String Connection
        {
            get { return this._connection; }
            set { this._connection = value; }
        }

        private MongoClient _client;
        public MongoClient Client
        {
            get { return this._client; }
            set { this._client = value; }
        }

        private IMongoDatabase _db;
        public IMongoDatabase Db
        {
            get { return this._db; }
            set { this._db = value; }
        }

        private IMongoCollection<BsonDocument> _collection;
        public IMongoCollection<BsonDocument> Collection
        {
            get { return this._collection; }
            set { this._collection = value; }
        }

        /// <summary>Establishes connection to cluster</summary>
        /// <returns>True if connection was established, False, otherwise</returns>
        public bool establishConnection()
        {
            return this._establishConnection();
        }

        /// <summary>Accesses and returns a specified database</summary>
        /// <param name="dbName">The database name</param>
        /// <returns>True if database was successfully accessed, false otherwise</returns>
        public bool accessDb(String databaseName)
        {
            return this._accessDb(databaseName);
        }

        /// <summary>Accesses and returns a specified collection</summary>
        /// <param name="collectionName">The collection name</param>
        /// <returns>True if successfully accessed, False, otherwise</returns>
        public bool accessCollection(String collectionName)
        {
            return this._accessCollection(collectionName);
        }

        /// <summary>Counts the number of total documents in a collection</summary>
        /// <returns>A long integer with a count of documents or zero</returns>
        public long countDocuments()
        {
            return this._countDocuments();
        }

        /// <summary>Display document contents in a specific format that is defaulted to BSON</summary>
        /// <param name="filter">A BsonDocument object to use as a filter for like objects</param>
        public void displayCollectionDocuments(String displayType = "BSON")
        {
            BsonDocument filter = new BsonDocument();

            if (displayType == "BSON")
            {
                this._displayCollectionDocumentsBSON(filter);
            }
            else if (displayType == "JSON")
            {
                this._displayCollectionDocumentsJSON(filter);
            }
            else
            {
                this._displayCollectionDocumentsJSONformatted(filter);
            }
        }

        /// <summary>Import parameterized data into collection</summary>
        /// <param name="data">A BSON document of data</param>
        /// <returns>True if import was successful, False, otherwise or if null</returns>
        public void importData(BsonDocument data)
        {
            if (data.IsBsonNull)
                return;

            this._importData(data);
        }

        /// <summary>Establishes connection to cluster</summary>
        /// <returns>True if connection was established, False, otherwise</returns>
        private bool _establishConnection()
        {
            try
            {
                this._client = new MongoClient(this._connection);
            }
            catch (MongoConfigurationException err)
            {
                Console.WriteLine($"\nFailed to connect:\n{err}");
                throw new UnauthorizedAccessException();
            }
            return true;
        }

        /// <summary>Accesses and returns a specified database</summary>
        /// <param name="dbName">The database name</param>
        /// <returns>True if database was successfully accessed, false otherwise</returns>
        private bool _accessDb(String dbName)
        {
            try
            {
                this._db = this._client.GetDatabase(dbName);
            }
            catch (ArgumentException err)
            {
                Console.WriteLine($"\nThe database name must be composed of valid characters:\n{err}");
                throw new ArgumentException();
            }
            return true;
        }

        /// <summary>Accesses and returns a specified collection</summary>
        /// <param name="collectionName">The collection name</param>
        /// <returns>True if successfully accessed, False, otherwise</returns>
        private bool _accessCollection(String collectionName)
        {
            try
            {
                this._collection = this._db.GetCollection<BsonDocument>(collectionName);
            }
            catch (ArgumentException err)
            {
                Console.WriteLine($"\nThe collection name must be composed of valid characters:\n{err}");
                throw new ArgumentException();
            }
            return true;
        }

        /// <summary>Counts the number of total documents in a collection</summary>
        /// <returns>A long integer with a count of documents or zero</returns>
        private long _countDocuments()
        {
            BsonDocument filter = new BsonDocument();
            return this._collection.CountDocuments(filter);
        }

        /// <summary>Display document contents in BSON</summary>
        /// <param name="filter">A BsonDocument object to use as a filter for like objects</param>
        private void _displayCollectionDocumentsBSON(BsonDocument filter)
        {
            using (var cursor = this._collection.Find(filter).ToCursor())
            {
                while (cursor.MoveNext())
                {
                    foreach (var doc in cursor.Current)
                    {
                        Console.WriteLine($"\nBSON doc: \n{doc}\n\n");
                    }
                }
            }
        }

        /// <summary>Create a new collection in the cluster</summary>
        /// <param name="collectionName">The name of the collection to create</param>
        public void createCollection(String collectionName)
        {
            this._createCollection(collectionName);
        }

        /// <summary>Drop a collection in the cluster</summary>
        /// <param name="collectionName">The name of the collection to drop</param>
        public void dropCollection(String collectionName)
        {
            this._dropCollection(collectionName);
        }

        /// <summary>Display document contents in JSON</summary>
        /// <param name="filter">A BsonDocument object to use as a filter for like objects</param>
        private void _displayCollectionDocumentsJSON(BsonDocument filter)
        {
            using (var cursor = this._collection.Find(filter).ToCursor())
            {
                while (cursor.MoveNext())
                {
                    foreach (var doc in cursor.Current)
                    {
                        String docJSON = doc.ToJson();
                        Console.WriteLine($"\nJSON doc: \n{docJSON}\n\n");
                    }
                }
            }
        }

        /// <summary>Display document contents in JSON with added formatting for pretty display</summary>
        /// <param name="filter">A BsonDocument object to use as a filter for like objects</param>
        private void _displayCollectionDocumentsJSONformatted(BsonDocument filter)
        {
            using (var cursor = this._collection.Find(filter).ToCursor())
            {
                while (cursor.MoveNext())
                {
                    foreach (var doc in cursor.Current)
                    {
                        String docFormatted = JsonConvert.SerializeObject(doc.ToJson(), Formatting.Indented);
                        Console.WriteLine($"\nJSON doc: \n{docFormatted}\n\n");
                    }
                }
            }
        }

        /// <summary>Create a new collection in the cluster</summary>
        /// <param name="collectionName">The name of the collection to create</param>
        private void _createCollection(String collectionName)
        {
            this._db.CreateCollection(collectionName);
        }

        /// <summary>Drop a collection in the cluster</summary>
        /// <param name="collectionName">The name of the collection to drop</param>
        private void _dropCollection(String collectionName)
        {
            this._db.DropCollection(collectionName);
        }

        /// <summary>Import parameterized data into collection</summary>
        /// <param name="data">A BSON document of data</param>
        /// <returns>True if import was successful, False, otherwise or if null</returns>
        public void _importData(BsonDocument data)
        {
            this._collection.InsertOne(data);
        }
    }
}
