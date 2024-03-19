using FluentAssertions;

namespace Test_NiN3KodeAPI
{
    public class FluentAssExample
    {
        [Fact]
        public void TestingFulentAssertionWorks()
        {
            string actual = "ABCDEFGHI";
            actual.Should().StartWith("AB").And.EndWith("HI").And.Contain("EF").And.HaveLength(9);
        }
    }
}