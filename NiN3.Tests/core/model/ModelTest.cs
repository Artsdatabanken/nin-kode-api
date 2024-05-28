using NiN3.Core.Models;

namespace NiN3.Tests.core.model
{
    public class ModelTest
    {
        [Fact]
        public void TestGrunnTypeToString()
        {
            Hovedtype obj = new Hovedtype();
            string actualtype = obj.ToString();
            Assert.Equal("NiN3.Core.Models.Hovedtype", obj.ToString());
        }
    }
}
