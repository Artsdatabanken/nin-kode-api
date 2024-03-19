using FluentAssertions;
using NinCmsImpExp;
using nin_cmsImpExp.models;
//using nin_cmsImpExp.models;
//using NinCmsImpExp;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Test_nin_cmsImpExp
{
    public class ApiClientTests
    {
        [Fact]
        public void TestGetResource()
        {
            //Visningsside  = https://www.artsdatabanken.no/NiN2.0/L1-C-5
            // https://artsdatabanken.no/api/resource?Type=naturtype&Tags=NiN%202.0&Code=L1-C-5
            ApiClient apiClient = new();
            string code = "L1-C-5";
            ResourceResult rr = apiClient.GetResource(code);
            rr.Id.Should().Be("NiN2.0/L1-C-5");
            rr.Name.Should().Be("Noe kalkfattig og eksponert fast strandkant-innsjøbunn");
            rr.Code.Should().Be("L1-C-5");
        }

        [Fact]
        public void TestGetResource2() {
            //L1-C-7 , 8 , 10 , 12
            ApiClient apiClient = new();
            string code = "L1-C-7";
            PropertyResult pr = apiClient.GetProperty (apiClient.GetResource("L1-C-12"));
            Console.WriteLine (pr);
        }

        [Fact]
        public void TestGetProperty()
        {
            //https://artsdatabanken.no/api/property?references=L1-C-5/M8&type=resource
            ApiClient apiClient = new ApiClient();
            PropertyResult pr= apiClient.GetProperty(apiClient.GetResource("L1-C-5"));//uses ResourceResult.Id -> "NiN2.0/L1-C-5"
            pr.Id.Should().Be("Nodes/317956");
            pr.Name.Should().Be("Noe kalkfattig og eksponert fast strandkant-innsjøbunn");
            pr.Type.Should().Be("resource");
        }

        [Fact]
        public void TestGetContent()
        {
            // https://artsdatabanken.no/api/content?type=description&references=nodes/317956
            ApiClient ac = new ApiClient();
            ContentResult cr = ac.GetContent(new PropertyResult { Id = "Nodes/317956" });//uses ResourceResult.Id -> "NiN2.0/L1-C-5"
            cr.Id.Should().Be("Nodes/316657");

            cr.Name.Should().Be("L1-C-5 Noe kalkfattig og" +
                " eksponert fast strandkant-innsjøbunn");

            cr.Description.Should().Be("<p>Kartleggingsenheten " +
                "omfatter eksponert strandkanter med fastbunn i noe kalkfattig innsjø. " +
                "Finnes på vindeksponerte strender i store og mellomstore innsjøer (&gt; 0,5 km<sup>2</sup>) " +
                "og i vindeksponerte innsjøer over tregrensa.</p>\n");
            cr.SubContentIds.Count().Should().Be(5);
            cr.SubContentIds.First().Should().Be("317004");
            cr.SubContentIds.Last().Should().Be("317008");
        }


        [Fact]
        public void TestGetContentRemoveHTML()
        {
            // https://artsdatabanken.no/api/content?type=description&references=nodes/317956
            ApiClient ac = new ApiClient();
            ContentResult cr = ac.GetContent(new PropertyResult { Id = "Nodes/317956" }, true);//uses ResourceResult.Id -> "NiN2.0/L1-C-5"
            cr.Id.Should().Be("Nodes/316657");

            cr.Name.Should().Be("L1-C-5 Noe kalkfattig og" +
                " eksponert fast strandkant-innsjøbunn");

            cr.Description.Should().Be("Kartleggingsenheten " +
                "omfatter eksponert strandkanter " +
                "med fastbunn i noe kalkfattig innsjø. " +
                "Finnes på vindeksponerte strender i store og" +
                " mellomstore innsjøer (> 0,5 km2) " +
                "og i vindeksponerte innsjøer over tregrensa.\n");
        }

        [Fact]
        public void TestGetSubContentRemoveHTML()
        {//317006 (Paragraph: 'Utbredelse og regional fordeling' of Nodes/317956)
            ApiClient ac = new ApiClient();
            ContentResult cr = ac.GetSubContent("317006", "316657", true);
            cr.Id.Should().Be("Nodes/317006");
            cr.Name.Should().Be("L1-C-5 Utbredelse og regional fordeling");
            cr.Description.Should().Be(null);
            cr.Heading.Should().Be("Utbredelse og regional fordeling");
            cr.Body.Should().Be("Forekommer i større innsjøer (> 5 km2) i hele landet, over tregrensa også i mellomstore innsjøer (> 0,5 km2).\n");
            cr.ParentContentId.Should().Be("316657");
            cr.PropertyId.Should().Be(null); //subcontent do not have property as parent
        }


        [Fact]
        public void TestCreateTextFile() {
            ApiClient ac = new ApiClient();
            var someText = $@"Name;Age
Bob;20
Roy;55";
            ac.CreateTextFile("test.csv", someText);
            string textFromFile = System.IO.File.ReadAllText("test.csv");
            Console.WriteLine(textFromFile);
            textFromFile.Should().Be("Name;Age\r\nBob;20\r\nRoy;55\r\n");
        }


        [Fact]
        public void TestMakeCsvs() 
        {
            //Send in 2 resources 
            ApiClient apiClient = new ApiClient();
            string code1="L1-C-5", code2 = "L1-C-6";
            List<ResourceResult> resources = new(){ apiClient.GetResource(code1), apiClient.GetResource(code2)};
            //List<ResourceResult> resources = apiClient.GetAllResources().ToList();
            //ResourceResult[] resources = apiClient.GetAllResources();

            // check if files are created
            apiClient.MakeCsvs(resources, Config.Csv_path);
            // check that 3 first files have 2 record in them 
            string textFromFile = System.IO.File.ReadAllText($"{Config.Csv_path}/resources.csv");
            Console.WriteLine(textFromFile);
            // last file should have more
            var expected = "Id,Name,Code\r\n" +
                "NiN2.0/L1-C-5,Noe kalkfattig og eksponert fast strandkant-innsjøbunn,L1-C-5\r\n" +
                "NiN2.0/L1-C-6,Noe kalkfattig beskyttet grunn fast innsjøbunn,L1-C-6\r\n";
            textFromFile.Should().Be(expected);
        }



        

        [Fact]
        public void TestGetLicense() 
        {
            ApiClient apiClient = new ApiClient();
            var lic = apiClient.GetLicense("T72");
            lic.Should().Be("CC BY 4.0 (Navngivelse)");
        }

        [Fact]
        public void TestGetImageReference()
        {
            var t = new ApiClient().GetImageReference("316379");
            t.Should().Be("Børre K. Dervo");

        }

        [Fact]
        public void DirClarity() { 
            var dir4TEST = Directory.GetCurrentDirectory();
            var other = new ApiClient().Dirinfo();
            Console.WriteLine("dir4test", dir4TEST);
            Console.WriteLine("dirsrc", other);
        }


        // L1-C-5 (1:1 property Nodes/317956) skal returnere 3 bilder F41278, F41279, F41291
        [Fact]
        public void TestGetImageContentByProperty()
        {
            // https://artsdatabanken.no/api/content?type=description&references=nodes/317956
            ApiClient ac = new ApiClient();
            List<NinImage> imageresult = ac.GetImageContentByProperty(new PropertyResult { Id = "Nodes/317956" });//uses ResourceResult.Id -> "NiN2.0/L1-C-5"
            Console.WriteLine("__", imageresult);
            imageresult.Count.Should().Be(3);
            var firstImage = imageresult[0];
            firstImage.PropertyId.Should().Be("Nodes/317956");
            firstImage.Licence.Should().Be("CC BY 4.0 (Navngivelse)");
            firstImage.Creator.Should().Be("Børre K. Dervo");
            firstImage.Mimetype.Should().Be("image/jpeg");
            firstImage.ReferenceId.Should().Be("Nodes/F41278");
            firstImage.Title.Should().Be("Bildenr-1-L_L1-5_Femunden.jpg");
        }

        [Fact]
        public void TestDownload() {
            ApiClient ac = new ApiClient();
            List<NinImage> imageresult = ac.GetImageContentByProperty(new PropertyResult { Id = "Nodes/317956" });//uses ResourceResult.Id -> "NiN2.0/L1-C-5"
            var firstImage = imageresult[0];
            ac.Download(20, firstImage);
            var filename = "317956_F41278_Bildenr-1-L_L1-5_Femunden.jpg";
            var path_to_dwn_file = $"data/v20/images/{filename}";
            File.Exists(path_to_dwn_file).Should().Be(true);
            // assert that the file has a certain size
            new FileInfo(path_to_dwn_file).Length.Should().BeLessThan(6001739L).And.BeGreaterThan(10L);
            //cleaning up 
            string[] paramz = { "data/v20/images", "F41278_Bildenr-1-L_L1-5_Femunden.jpg" };
            CleanUp("TestDownload", paramz);
        }

        [Fact]
        public void TestGetImagesAndDownload() {
            string[] paramz = { Config.Image_path };
            CleanUp("TestDownload", paramz);
            /*
             *  L1-C-5:  Nodes/317956
             *  L1-C-6:  Nodes/317959
                L1-C-7:  Nodes/317963
                L1-C-8:  Nodes/317968
                L1-C-10: Nodes/317940 
                L1-C-12: Nodes/317943
             */
            //string[] props = { "Nodes/317956" };
            string[] props = { "Nodes/317956", "Nodes/317963", "Nodes/317968", "Nodes/317940", "Nodes/317943" };
            //string[] props = { "Nodes/183564", "Nodes/183565", "Nodes/183566", "Nodes/183567", "Nodes/183568", "Nodes/183570", "Nodes/183571", "Nodes/183572", "Nodes/183573", "Nodes/183574", "Nodes/183575", "Nodes/183576", "Nodes/183577", "Nodes/183578", "Nodes/183579", "Nodes/183580", "Nodes/183581", "Nodes/183582", "Nodes/183583", "Nodes/183584", "Nodes/183585", "Nodes/183586", "Nodes/183587", "Nodes/183588", "Nodes/183589", "Nodes/183590", "Nodes/183591", "Nodes/183592", "Nodes/183593", "Nodes/183594" };
            List<PropertyResult> properties = new();
            foreach (var p in props) {
                properties.Add(new PropertyResult { Id = p });
            }
            var ac = new ApiClient();
            List<NinImage> images = ac.GetImages(properties);
            images.Count.Should().NotBe(0);
            ac.WriteObjectList2CsvFile(images, $@"{Config.Csv_path}/images.csv");
            ac.DownloadImages(images);
            //todo-sat: download each file to image folder.
            int count = new System.IO.DirectoryInfo(Config.Image_path).GetFiles().Length;
            count.Should().Be(7);
            
            CleanUp("TestDownload", paramz);
        }




        public void CleanUp(string testname, Object[] paramz) {
            // helper logic for cleaning 
            switch (testname) {
                case "TestDownload":
                        // If file found, delete it    
                        //File.Delete(Path.Combine((string)paramz[0], (string)paramz[1]));
                        //Console.WriteLine($"File {(string)paramz[1]} deleted.");
                    Array.ForEach(Directory.GetFiles((string)paramz[0]), File.Delete);
                    break;
                default:
                    Console.WriteLine("nada");
                    break;
            }
        }
    }
}
