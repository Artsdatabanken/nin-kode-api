using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_nin_cmsImpExp
{
    using FluentAssertions;
    using System.Xml.Serialization;
    using Test_nin_cmsImpExp;

    public class ConfigTests
    {
        [Fact]
        public void TestingSomething()
        {
            //string actual = "ABCDEFGHI";
            //actual.Should().StartWith("AB").And.EndWith("HI").And.Contain("EF").And.HaveLength(9);
            1.Should().Be(1);
        }

        [Fact]
        public void TestGetApiBase()
        {
            List<string> current_hostname = NinCmsImpExp.Config.Hosts;
            string hname = System.Environment.MachineName;
            string apibase = NinCmsImpExp.Config.ApiBase;
            string ninversion = NinCmsImpExp.Config.NiNVersion;
            if (NinCmsImpExp.Config.IfHostIsDev())
            {
                Console.WriteLine("Env mode is dev");
                apibase.Should().Be("http://localhost:8002/");                
            }
            else 
            {
                Console.WriteLine("Env mode is other than dev");
                apibase.Should().Be("https://artsdatabanken.no/api/");
            }
            ninversion.Should().Be("NiN%202.0");
        }
    }   
}
