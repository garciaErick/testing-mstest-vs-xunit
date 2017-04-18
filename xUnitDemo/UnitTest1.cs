using Xunit;

namespace xUnitDemo
{
    public class Class1
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, 2+2);
        }
    }
}