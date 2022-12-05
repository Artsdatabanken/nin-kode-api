// See https://aka.ms/new-console-template for more information

using nin_cmsImpExp.models;
using NinCmsImpExp;
using Sharprompt;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using static Sharprompt.Prompt;
using static System.Net.Mime.MediaTypeNames;

Console.WriteLine("Hello there, write command or type 'Help' for options");

// api klient
var apiClient = new ApiClient();
var run = true;
var meny = @"Commands:
            info    : show settings/config
            testexp : Testexport of 2 pages
            export  : Run the full (all pages with text and images)            
            exit    : Close program";
while (run) {
    var input = Prompt.Input<string>("cmsimpexp ");
    switch (input) {
        case "help":
            Console.WriteLine(meny);
            break;
        case "testexp":
            Console.WriteLine($"Fetcing 2 Pages (L1-C-5 & L1-C-6)");
            string code1 = "L1-C-5", code2 = "L1-C-6";
            List<ResourceResult> resources = new() { apiClient.GetResource(code1), apiClient.GetResource(code2) };
            apiClient.MakeCsvs(resources, Config.Csv_path);
            Console.WriteLine("Done :)");
            break;
        case "exit":
            return;
            break;
        case "export":
            Console.WriteLine($"Running full import...");
            var resourceResults = apiClient.GetAllResources();
            apiClient.MakeCsvs(resourceResults.ToList(), Config.Csv_path);
            break;
        case "info": // Vise config params
            Console.WriteLine($"Showing settings...");
            Console.WriteLine($"\t Parallel pool size:      {Config.Parallel_pool}");
            Console.WriteLine($"\t Ninfolder version:       {Config.NiNFolderVersion}");
            Console.WriteLine($"\t NinVersion:              {Config.NiNVersion}");
            Console.WriteLine($"\t Hosts (dev):             {String.Join(", ", Config.Hosts.ToArray())}");
            Console.WriteLine($"\t Export folder (csv's):   {Config.Csv_path}");
            Console.WriteLine($"\t Export folder (img's):   {Config.Image_path}");
            break;
        default:
            Console.WriteLine("No knows command, options:");
            Console.WriteLine(meny);
            break;
    }
}
 
//// hent resurs M8
//var resourceResult = apiClient.GetResource("M8");

//// hent kopling til description artikler indirekte via propertyindex
//var propertyResult = apiClient.GetProperty(resourceResult);

//// hent description via content
//var contentResult = apiClient.GetContent(propertyResult);

//Console.WriteLine($"Naturtype:{resourceResult.Code} - name: {resourceResult.Name} - Description: {contentResult.Description}");


//apiClient.Get
/*
foreach (var result in resourceResults.OrderBy(x=>x.Code.Length).ThenBy(x=>x.Code))
{
    // hent kopling til description artikler indirekte via propertyindex
    var pResult = apiClient.GetProperty(result);

    // hent description via content
    var cResult = apiClient.GetContent(pResult);

    Console.WriteLine($"Naturtype:{result.Code} - name: {result.Name}");
    Console.WriteLine($"Description: {(cResult!= null ? cResult.Description : "MANGLER")}");
    Console.WriteLine($"Body: {(cResult != null ? cResult.Body : "MANGLER")}");
}*/
