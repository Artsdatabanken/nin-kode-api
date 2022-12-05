using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using HtmlAgilityPack;
using Json.Net;
namespace NinCmsImpExp;
using System.Formats.Asn1;

using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using CsvHelper;
using System.Net.Http;
using System.Collections.Generic;
using nin_cmsImpExp.models;
using System.Net;
using System.Threading.Tasks;

public class ApiClient
{
    private readonly HttpClient _httpClient = new(){ BaseAddress = new Uri(Config.ApiBase) };
    private readonly string ninVersion = NinCmsImpExp.Config.NiNVersion;
    private Dictionary<string, string> licenses = new Dictionary<string, string>();
    private Dictionary<string, string> authors = new Dictionary<string, string>();
    public ApiClient()
    {
            
    }

    public string Dirinfo() {
        Console.WriteLine("currdir in apiclient", Directory.GetCurrentDirectory());
        return Directory.GetCurrentDirectory();
    }

    public ResourceResult GetResource(string code)
    {
        // https://artsdatabanken.no/api/resource?Type=naturtype&Tags=NiN%202.0&Code=M8
        var test = _httpClient.GetStringAsync($"resource?Type=naturtype&Tags={ninVersion}&Code={code}").GetAwaiter()
            .GetResult();
        var doc = JsonDocument.Parse(test);
        var array = doc.RootElement.EnumerateArray().ToList();
        foreach (var element in array)
        {
            var result = new ResourceResult();
            if (element.TryGetProperty("Id", out JsonElement idElement))
            {
                result.Id = idElement.GetString();
            }

            if (element.TryGetProperty("Title", out JsonElement nameElement))
            {
                result.Name = nameElement.GetString();
            }
            if (element.TryGetProperty("Code", out JsonElement codeElement))
            {
                result.Code = codeElement.GetString();
            }

            if (result.Code == code)
            {
                return result;
            }
        }
        return null;
    }

    public PropertyResult GetProperty(ResourceResult resourceResult)
    {
        // https://artsdatabanken.no/api/property?references=NiN2.0/M8&type=resource
        var test = _httpClient.GetStringAsync($"property?references={resourceResult.Id}").GetAwaiter()
            .GetResult();
        var doc = JsonDocument.Parse(test);
        var array = doc.RootElement.EnumerateArray().ToList();
        foreach (var element in array)
        {
            //Resource:
            //{
            //    Id: "Nodes/171439",
            //    Title: "Helofytt-saltvannssump",
            //    Type: "resource",
            //    Tags: null
            var result = new PropertyResult();
            if (element.TryGetProperty("Resource", out JsonElement resourceElement))
            {
                foreach (var prop in resourceElement.EnumerateObject())
                {
                    if (prop.NameEquals("Id"))
                    {
                        result.Id = prop.Value.GetString();
                    }
                    if (prop.NameEquals("Title"))
                    {
                        result.Name= prop.Value.GetString();
                    }
                    if (prop.NameEquals("Type"))
                    {
                        result.Type = prop.Value.GetString();
                    }
                  
                }
            }
            

            if (result.Type == "resource")
            {
                result.ResourceId = resourceResult.Id;
                return result;
            }
        }

        return null;
    }

    /// <summary>
    /// Method <c>ContentResult</c> retrieves content object from CMS api
    /// </summary>
    /// <param name="propertyResult"></param>
    /// <param name="remove_htmltags">Wether to remove html tags, default = false</param>
    public ContentResult GetContent(PropertyResult propertyResult, bool remove_htmltags = false)
    {
        // r
        var test = _httpClient.GetStringAsync($"content?type=description&references={propertyResult.Id}").GetAwaiter()
            .GetResult();
        //var test = _httpClient.GetStringAsync($"content?type=image&references={propertyResult.Id}").GetAwaiter()
        //    .GetResult();
        var doc = JsonDocument.Parse(test);
        var array = doc.RootElement.EnumerateArray().ToList();
        foreach (var element in array)
        {
            //Resource:
            //{
            //    Id: "Nodes/171439",
            //    Title: "Helofytt-saltvannssump",
            //    Type: "resource",
            //    Tags: null
            var result = new ContentResult();

            foreach (var prop in element.EnumerateObject())
            {
                if (prop.NameEquals("Id"))
                {
                    result.Id = prop.Value.GetString();
                }

                if (prop.NameEquals("Content")) {
                   //var c = prop.Value.GetArray;
                    var contentArray = JsonNet.Deserialize<dynamic>(prop.Value.GetRawText())!;
                    foreach (var contentItem in contentArray) {
                        var OnlyId = contentItem.Id.Replace("Nodes/", "");
                        result.SubContentIds.Add(OnlyId);
                    }
                }

                if (prop.NameEquals("Title"))
                {
                    result.Name = prop.Value.GetString();
                }

                if (prop.NameEquals("Intro"))
                {
                    result.Description = prop.Value.GetString();
                }
                if (prop.NameEquals("Body"))
                {
                    result.Body = prop.Value.GetString();
                }


                if (prop.NameEquals("Descriptioncontent"))
                {
                    result.Body = prop.Value.GetString();
                }

            }

            if (remove_htmltags) {
                result.Body =        !string.IsNullOrWhiteSpace(result.Body) ? Html2Text(result.Body) : null;
                result.Description = !string.IsNullOrWhiteSpace(result.Description) ? Html2Text(result.Description) : null;

            }

            // primitivt - tar første med noe innhold...
            if (!string.IsNullOrWhiteSpace(result.Description))
            {
                result.PropertyId = propertyResult.Id;
                result.SubContentIds.Sort();
                return result;
            }
        }
        return null;
    }

    /// <summary>
    /// Method <c>ContentResult</c> retrieves content object from CMS api
    /// </summary>
    /// <param name="propertyResult"></param>
    /// <param name="remove_htmltags">Wether to remove html tags, default = false</param>
    public List<NinImage> GetImageContentByProperty(PropertyResult prop, bool remove_htmltags = false)
    {
        // https://artsdatabanken.no/api/content?type=files&references=nodes/171439
        //var test = _httpClient.GetStringAsync($"content?type=description&references={prop.Id}").GetAwaiter()
         //   .GetResult();
        var test = _httpClient.GetStringAsync($"content?type=image&references={prop.Id}").GetAwaiter()
            .GetResult();

        //var test = _httpClient.GetStringAsync($"Content/{subcontent_nodeid}").GetAwaiter()
        //.GetResult();
        var doc = JsonDocument.Parse(test);
        var array = doc.RootElement.EnumerateArray().ToList();
        List<NinImage> imglist = new() { };
        
        foreach (var element in array)
        {
            var image = new NinImage();
            image.PropertyId = prop.Id;
            //Resource:
            //{
            //    Id: "Nodes/171439",
            //    Title: "Helofytt-saltvannssump",
            //    Type: "resource",
            //    Tags: null
            //var result = new NinImage();
            var enumerated_element = element.EnumerateObject();
            foreach (var item in enumerated_element)
            {
                if (item.NameEquals("Title"))
                {
                    var t = item.Value.GetString();
                    if (!string.IsNullOrWhiteSpace(t))
                    {
                        image.Title = t;
                    }
                }
                if (item.NameEquals("Mime"))
                {
                    var m = item.Value.GetString();
                    if (!string.IsNullOrWhiteSpace(m))
                    {
                        image.Mimetype = m;
                    }
                }

                if (item.NameEquals("Id"))
                {
                    image.ReferenceId = item.Value.GetString();
                }

                if (item.NameEquals("License")) {
                    var lic = JsonNet.Deserialize<dynamic>(item.Value.GetRawText())!;
                    if (lic != null)
                    {
                        var id = lic.Id.Replace("Nodes/", "");
                        image.Licence = GetLicense(id);
                    }
                    else {
                        image.Licence = "UNKNOWN";
                    }
                }

                if (item.NameEquals("Url")) {
                    var number = item.Value.GetString().Replace("/Pages/", "");
                    image.Url = $"https://artsdatabanken.no/Media/{number}";
                        //https://artsdatabanken.no/Media/F27904
                }
                if (item.NameEquals("Metadata")) { 
                    var mdata = JsonNet.Deserialize<dynamic>(item.Value.GetRawText())!;
                    foreach(var m in mdata) { 
                    var refs = m.References;
                        foreach (var r in refs)
                        {
                            var num = r.Header.Content.Replace("Nodes/", "");
                            var creator_title = GetImageReference(num);
                            if (!string.IsNullOrWhiteSpace(creator_title)) {
                                image.Creator = creator_title;
                            }
                            //break;
                            //Console.WriteLine("num", num);
                        }
                    }
                }
            }
            imglist.Add(image);
        }
        return imglist.OrderBy(o => o.ReferenceId).ToList();
    }


    public ContentResult GetSubContent(string subcontent_nodeid, string ParentContentId, bool remove_htmltags = false) {
        var result = new ContentResult();
        // https://artsdatabanken.no/api/Content/{nodeid}
        try
        {
            var test = _httpClient.GetStringAsync($"Content/{subcontent_nodeid}").GetAwaiter()
                .GetResult();
            var subContent = JsonNet.Deserialize<dynamic>(test)!;
            result.Id = subContent.Id;
            result.Heading = subContent.Heading;
            result.Body = subContent.Body;
            result.Description = subContent.Intro;
            result.ParentContentId = ParentContentId;
            result.Name = subContent.Title;
            if (remove_htmltags)
            {
                result.Body = !string.IsNullOrWhiteSpace(result.Body) ? Html2Text(result.Body) : null;
                result.Description = !string.IsNullOrWhiteSpace(result.Description) ? Html2Text(result.Description) : null;

            }
            result.ParentContentId = ParentContentId;
        }
        catch (Exception ex) {
            //
            Console.WriteLine("Error", ex);
            result = null;           
        }
        return result;
    }

    public string GetImageReference(string num) {
        var test = _httpClient.GetStringAsync($"Content/{num}").GetAwaiter()
    .GetResult();
        var subContent = JsonNet.Deserialize<dynamic>(test)!;
        return subContent.Title;
    }

    public void WriteObjectList2CsvFile<T>(List<T> data, string path)
    {
        try
        {
            //using (var writer = new StreamWriter(path, Encoding.UTF8))
            //FileStreamOptions fso = new();
            //fso.Options = new FileOptions();
            //using (var writer = new StreamWriter(path, Encoding.Latin1, fso))
            using (var writer = new StreamWriter(path))// using default settings, encoding=utf8 etc.
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(data);
                writer.Flush();
            }
        }
        catch (Exception ex) { 
            Console.WriteLine(ex.ToString());
        }
    }

    //public NinImage GetImageContent(bool Remove_Htmltags = false) { 
    //    
    //}
        
    


    /// <summary>
    /// Method <c>Html2Text</c> removes html-tags from a string
    /// </summary>
    /// <param name="propertyResult"></param>
    /// <param name="HtmlText">Wether to remove html tags, default = false</param>
    public string Html2Text(string HtmlText) {
        HtmlDocument htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(HtmlText);
        var res = htmlDoc.DocumentNode.InnerText.Replace("&gt;", ">").Replace("&lt;", "<");
        return res;
    }

    public ResourceResult[] GetAllResources()
    {
        // https://artsdatabanken.no/api/resource?Type=naturtype&Tags=NiN%202.0

        var results = new List<ResourceResult>();
        var page = 0;
        while (true)
        {
            var test = _httpClient.GetStringAsync($"resource?Type=naturtype&Tags={ninVersion}&Take=100&Skip={page*100}").GetAwaiter()
                .GetResult();
            var doc = JsonDocument.Parse(test);
            var array = doc.RootElement.EnumerateArray().ToList();
            foreach (var element in array)
            {
                var result = new ResourceResult();
                if (element.TryGetProperty("Id", out JsonElement idElement))
                {
                    result.Id = idElement.GetString();
                }

                if (element.TryGetProperty("Title", out JsonElement nameElement))
                {
                    result.Name = nameElement.GetString();
                }
                if (element.TryGetProperty("Code", out JsonElement codeElement))
                {
                    result.Code = codeElement.GetString();
                }
                results.Add(result);
            }

            if (array.Count == 0)
            {
                break;
            }
            page += 1;
        }
        
        return results.ToArray();
    }
    public void CreateTextFile(string path, string content)
    {
        Int64 x;
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            //Open the File
            StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8);
            using (StringReader reader = new StringReader(content))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    sw.WriteLine(line);
                }
            }
            
            //close the file
            sw.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
        finally
        {
            Console.WriteLine("Executing finally block.");
        }
    }


    public void MakeCsvs(List<ResourceResult> rrList, string root_path)
    {
        CreatePathIfNotExist(root_path);
        var options = new ParallelOptions() { MaxDegreeOfParallelism = Config.Parallel_pool };
        List<PropertyResult> prList = new ();
        List<object> crList = new List<object>();
        List<object> subCrList = new List<object>();

        Parallel.ForEach(rrList, options, rr =>
        {
            var pr = GetProperty(rr);
            prList.Add(pr);
            var cr = GetContent(pr, true);
            if (cr != null)
            {
                crList.Add(cr);
                foreach (var scId in cr.SubContentIds)
                {
                    var subCr = GetSubContent(scId, cr.Id, true);
                    if (subCr != null)
                    {
                        subCrList.Add(subCr);
                    }
                }
            }
        });
        WriteObjectList2CsvFile(rrList, $@"{root_path}/resources.csv");
        WriteObjectList2CsvFile(prList, $@"{root_path}/properties.csv");
        WriteObjectList2CsvFile(crList, $@"{root_path}/content.csv");
        WriteObjectList2CsvFile(subCrList, $@"{root_path}/subcontent.csv");

        List<NinImage> images = GetImages(prList);
        WriteObjectList2CsvFile(images, $@"{root_path}/images.csv");
        //todo-sat: download each file to image folder.
        DownloadImages(images);
    }

    public void DownloadImages(List<NinImage> images)
    {
        var options = new ParallelOptions() { MaxDegreeOfParallelism = Config.Parallel_pool };
        Parallel.ForEach(images, options, i =>
        {
            Download(20, (NinImage)i);
        });
    }

    public List<NinImage> GetImages(List<PropertyResult> prList)
    {
        List<NinImage> images = new();
        var options = new ParallelOptions() { MaxDegreeOfParallelism = Config.Parallel_pool };
        //foreach (var p in prList)
        Parallel.ForEach(prList, options, p =>
        {
            List<NinImage> tmpImageresults = GetImageContentByProperty(p);
            Parallel.ForEach(tmpImageresults, options, img =>
            //foreach (var img in tmpImageresults)
            {
                images.Add(img);
            });
        });
        return images;
    }

    //public string
    //toto-sat: get image-objekt from key

    public string List2Csv(List<object> anyList) {
        //...
        //if list not empty 
        //get type of first element
        //step into switch with type
        string result = null;
        if (anyList.Any()) { 

            List<string> lines = new List<string>();
            // pick right header based on type
            var type = anyList.First().GetType().Name;
            
            foreach(var item in anyList) {
                switch (type) {
                    case "ResourceResult":
                        ResourceResult rr = (ResourceResult)item;
                        lines.Add($@"{rr.Id};{rr.Name};{rr.Code}");
                        break;
                    case "PropertyResult":
                        PropertyResult pr = (PropertyResult)item;
                        lines.Add($@"{pr.Id};{pr.Name};{pr.Type}");
                        break;
                    case "ContentResult":
                        ContentResult cr = (ContentResult)item;
                        //lines.Add($@"{cr.Id};{cr.Name};"{cr.Heading}";"{cr.Description}";\"{cr.Body}\";{cr.PropertyId};{cr.ParentContentId}");
                        break;
                }
            }
            result = string.Join("\n", lines);
        }
        return result;
    }

    /*public string getLicense(string id) {

        return "";
    }*/

    public string GetLicense(string license_id)
    {
        // https://artsdatabanken.no/api/Content/{nodeid}
        try
        {
            var nullres = string.Empty;
            if (licenses.TryGetValue(license_id, out nullres)) {
                return licenses[license_id];
            }
            var test = _httpClient.GetStringAsync($"Content/{license_id}").GetAwaiter()
                .GetResult();

            var licresult = JsonNet.Deserialize<dynamic>(test)!;
            Console.WriteLine("", licresult);
            if (licenses.TryGetValue(license_id, out nullres))
            {
                return licenses[license_id];
            }
            if (!string.IsNullOrWhiteSpace(licresult.Title)) {
                //add to dict
                licenses.Add(license_id, licresult.Title);
                return licresult.Title;         
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return "";
    }


    /// <summary>
    /// Method <c>Download</c> download image file to a given output folder
    /// </summary>
    /// <param name="nin_version"></param> nature-in-norway version
    /// <param name="image">object of class NinImage, metadata about the image</param>
    /// <param name="dwn_path">path to parent folder of the downloaded image file</param>
    public void Download(int nin_version, NinImage image) {
        using (var client = new WebClient())
        {
            var dwn_path = Config.Image_path;
            CreatePathIfNotExist(dwn_path);
            var num = image.ReferenceId.Replace("Nodes/", "");
            var prop_num = image.PropertyId.Replace("Nodes/", "");
            var path = $"{dwn_path}/{prop_num}_{num}_{image.Title}";
            client.DownloadFile(image.Url, path);
        }
    }

    public bool CreatePathIfNotExist(string path) {
        bool success = true;
        try {
            bool exists = System.IO.Directory.Exists(path);

            if (!exists)
                System.IO.Directory.CreateDirectory(path);
        }
        catch(Exception e){
            success = false;
        }
        return success;
    }
}