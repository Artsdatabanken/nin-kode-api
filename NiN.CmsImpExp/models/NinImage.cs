namespace nin_cmsImpExp.models;

public class NinImage
{
    public string ReferenceId { get; set; }
    public string Url { get; set; }
    public string Licence { get; set; }
    public string Creator { get; set; }
    public string Publisher { get; set; }
    public bool Cover { get; set; }
    public string PropertyId { get; set; } // Propertyobject that image is bound to.

    public string Mimetype { get; set; }

    public string Title { get; set; }
}