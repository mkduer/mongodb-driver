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
            set { _connection = value; }
        }

        private MongoClient _client;
        public MongoClient Client
        {
            get { return this._client; }
            set { _client = value; }
        }

        private IMongoDatabase _db;
        public IMongoDatabase Db
        {
            get { return this._db; }
            set { _db = value; }
        }

        private IMongoCollection<BsonDocument> _collection;
        public IMongoCollection<BsonDocument> Collection
        {
            get { return this._collection; }
            set { _collection = value; }
        }

        public bool establishConnection()
        {
            return this._establishConnection();
        }

        public bool accessDb(String databaseName)
        {
            return this._accessDb(databaseName);
        }
        public bool accessCollection(String collectionName)
        {
            return this._accessCollection(collectionName);
        }
        public long countDocuments()
        {
            return this._countDocuments();
        }

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

        /// <summary>Establishes connection to cluster</summary>
        /// <returns>Returns an initialized MongoClient</returns>
        private bool _establishConnection()
        {
            try
            {
                this._client = new MongoClient(this._connection);
            }
            catch (MongoConfigurationException err)
            {
                Console.WriteLine($"\nFailed to connect: {err}");
                throw new UnauthorizedAccessException();
            }
            return true;
        }

        /// <summary>Accesses and returns a specified database</summary>
        /// <param name="dbName">The database name</param>
        /// <returns>ImongoDatabase type that is the database</returns>
        private bool _accessDb(String dbName)
        {
            this._db = this._client.GetDatabase(dbName);
            return true;
        }

        /// <summary>Accesses and returns a specified collection</summary>
        /// <param name="collectionName">The collection name</param>
        /// <returns>Returns an ImongoCollection with a list of BSON documents</returns>
        private bool _accessCollection(String collectionName)
        {
            this._collection = this._db.GetCollection<BsonDocument>(collectionName);
            return true;
        }

        private long _countDocuments()
        {
            BsonDocument filter = new BsonDocument();
            return this._collection.CountDocuments(filter);
        }
        private void _displayCollectionDocumentsBSON(BsonDocument filter)
        {
            using (var cursor = this._collection.Find(filter).ToCursor())
            {
                while (cursor.MoveNext())
                {
                    foreach (var doc in cursor.Current)
                    {
                        Console.WriteLine($"BSON doc: \n{doc}\n\n");
                    }
                }
            }
        }
        private void _displayCollectionDocumentsJSON(BsonDocument filter)
        {
            using (var cursor = this._collection.Find(filter).ToCursor())
            {
                while (cursor.MoveNext())
                {
                    foreach (var doc in cursor.Current)
                    {
                        String docJSON = doc.ToJson();
                        Console.WriteLine($"JSON doc: \n{docJSON}\n\n");
                    }
                }
            }
        }
        private void _displayCollectionDocumentsJSONformatted(BsonDocument filter)
        {
            using (var cursor = this._collection.Find(filter).ToCursor())
            {
                while (cursor.MoveNext())
                {
                    foreach (var doc in cursor.Current)
                    {
                        String docFormatted = JsonConvert.SerializeObject(doc.ToJson(), Formatting.Indented);
                        Console.WriteLine($"JSON doc: \n{docFormatted}\n\n");
                    }
                }
            }
        }
    }
}
