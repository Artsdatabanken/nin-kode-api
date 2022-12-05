namespace Test_nin_cmsImpExp
{
    using FluentAssertions;
    //https://fluentassertions.com/introduction

    public class TestToolsInPlace
    {
        [Fact]
        public void TestingFulentAssertionWorks()
        {
            string actual = "ABCDEFGHI";
            actual.Should().StartWith("AB").And.EndWith("HI").And.Contain("EF").And.HaveLength(9);
        }
    }
}