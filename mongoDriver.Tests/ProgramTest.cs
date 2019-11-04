using Xunit;

namespace mongoDriver.Tests
{
    public class ProgramTest
    {
        [Fact]
        public void helloWorldTest_passesWhenXunitWorks()
        {
            Assert.Equal("hello world", "hello world");

        }
    }
}
