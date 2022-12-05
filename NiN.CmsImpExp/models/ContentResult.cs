namespace nin_cmsImpExp.models;
using System.Collections.Generic;

public class ContentResult
{
    private string body;

    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Body { get => body; set => body = value; }
    public string PropertyId { get; set; }
    public string ParentContentId { get; set; }
    public List<string> SubContentIds { get; set; }
    public string Heading { get; set; }


    public ContentResult()
    {
        SubContentIds = new List<string>();
    }
}
