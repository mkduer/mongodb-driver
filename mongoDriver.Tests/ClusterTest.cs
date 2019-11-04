using System;
using Xunit;

namespace mongoDriver.Tests
{
    public class ClusterTest
    {
        [Theory]
        [InlineData("mongodb+srv://test:test@airbnb-oluyv.mongodb.net/test?retryWrites=true&w=majority")]
        public void establishConnection_connectionSuccessfullyEstablished(String connection)
        {

            Cluster cluster = new Cluster();
            cluster.Connection = connection;
            Assert.True(cluster.establishConnection());
        }

        [Theory]
        [InlineData("invalidClusterName")]
        public void establishConnection_connectionFails(String connection)
        {

            Cluster cluster = new Cluster();
            cluster.Connection = connection;
            Assert.Throws<UnauthorizedAccessException>(() => cluster.establishConnection());
        }

        [Theory]
        [InlineData("dbname")]
        public void accessDb_accessSuccessful(String db)
        {

            Cluster cluster = new Cluster();
            cluster.Connection = "mongodb+srv://test:test@airbnb-oluyv.mongodb.net/test?retryWrites=true&w=majority";
            cluster.establishConnection();
            Assert.True(cluster.accessDb(db));
        }

        [Theory]
        [InlineData("")]
        [InlineData("invalid.")]
        public void accessDb_accessFails(String db)
        {
            Cluster cluster = new Cluster();
            cluster.Connection = "mongodb+srv://test:test@airbnb-oluyv.mongodb.net/test?retryWrites=true&w=majority";
            cluster.establishConnection();
            Assert.Throws<ArgumentException>(() => cluster.accessDb(db));
        }

        [Theory]
        [InlineData("collectionname")]
        public void accessCollection_accessSuccessful(String collection)
        {
            Cluster cluster = new Cluster();
            cluster.Connection = "mongodb+srv://test:test@airbnb-oluyv.mongodb.net/test?retryWrites=true&w=majority";
            cluster.establishConnection();
            cluster.accessDb("dbname");
            Assert.True(cluster.accessCollection(collection));
        }

        [Theory]
        [InlineData("")]
        public void accessCollection_accessFailed(String collection)
        {
            Cluster cluster = new Cluster();
            cluster.Connection = "mongodb+srv://test:test@airbnb-oluyv.mongodb.net/test?retryWrites=true&w=majority";
            cluster.establishConnection();
            cluster.accessDb("dbname");
            Assert.Throws<ArgumentException>(() => cluster.accessCollection(collection));
        }
    }
}
