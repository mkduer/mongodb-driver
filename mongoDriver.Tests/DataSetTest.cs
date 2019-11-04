using System;
using System.IO;
using Xunit;

namespace mongoDriver.Tests
{
    public class DataSetTest
    {
        [Theory]
        [InlineData("Somefilepath/mongodb-driver/mongoDriver/random/pathDetails")]
        public void findDirectory_directoryFoundInPath(String path)
        {
            DataSet data = new DataSet();
            data.getWorkingDirectory(path);
            bool containTrue = data.DataDir.Contains("mongoDriver");
            Assert.True(containTrue);
        }

        [Theory]
        [InlineData("Somefilepath/mongodb-driver/mongoDriver/random/pathDetails")]
        public void findDirectory_getDataDirSuccessful(String path)
        {
            DataSet data = new DataSet();
            Assert.True(data.getWorkingDirectory(path));
        }

        [Theory]
        [InlineData("Somefilepath/mongodb-driver/invalidString/random/pathDetails")]
        public void findDirectory_directoryNotFound(String path)
        {
            DataSet data = new DataSet();
            Assert.Throws<DirectoryNotFoundException>(() => data.getWorkingDirectory(path));
        }
    }
}
