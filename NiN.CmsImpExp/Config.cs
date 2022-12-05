using System.Collections;
using System.ComponentModel.Design;

namespace NinCmsImpExp;

public class Config {
    private static bool _ignoreEnvs = true;
    private static readonly string _ninFolderVersion = "20";
    private static readonly string _ninVersion = $"NiN%202.0";
    private static readonly List<string> _hosts = new() { "4997ADB" };//dev machines by hostname
    private static readonly string _apiBase = "https://artsdatabanken.no/api/";
    private static readonly string _imageUri = "https://artsdatabanken.no/Media/";
    private static readonly string _hostname = System.Environment.MachineName;
    private static readonly int _parallel_pool = 8;
    private static readonly string _csv_path = $"./data/v{NiNFolderVersion}";
    private static readonly string _image_path = $"./data/v{NiNFolderVersion}/images";
    public static string ApiBase {
        get {
            string apibase =  IfHostIsDev() ? "https://test.artsdatabanken.no/api/" : _apiBase;
            return apibase;
        }
    }

    public static int Parallel_pool {
        get{ return _parallel_pool; }
    }

    public static string Csv_path
    {
        get { return _csv_path; }
    }

    public static string Image_path
    {
        get { return _image_path; }
    }

    public static string NiNVersion {
        get { return _ninVersion; }
    }

    public static string NiNFolderVersion {
        get { return _ninFolderVersion; }
    }


    public static bool IfHostIsDev() {
        return _ignoreEnvs ? false : _hosts.Contains(_hostname);
    }

    public static List<string> Hosts { get => _hosts;}
    public static string Hostname { get => _hostname; }
}
