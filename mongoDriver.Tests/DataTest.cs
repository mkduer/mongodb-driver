using System;
using System.IO;
using Xunit;

namespace mongoDriver.Tests
{
    public class DataTest
    {
        [Theory]
        [InlineData("Somefilepath/mongodb-driver/mongoDriver/random/pathDetails")]
        public void findDirectory_directoryFoundInPath(String path)
        {
            Data data = new Data();
            data.getWorkingDirectory(path);
            bool containTrue = data.DataDir.Contains("mongoDriver");
            Assert.True(containTrue);
        }

        [Theory]
        [InlineData("Somefilepath/mongodb-driver/mongoDriver/random/pathDetails")]
        public void findDirectory_getDataDirSuccessful(String path)
        {
            Data data = new Data();
            Assert.True(data.getWorkingDirectory(path));
        }

        [Theory]
        [InlineData("Somefilepath/mongodb-driver/invalidString/random/pathDetails")]
        public void findDirectory_directoryNotFound(String path)
        {
            Data data = new Data();
            Assert.Throws<DirectoryNotFoundException>(() => data.getWorkingDirectory(path));
        }
    }
}
