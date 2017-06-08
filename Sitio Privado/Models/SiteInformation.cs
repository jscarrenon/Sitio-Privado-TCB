namespace Sitio_Privado.Models
{
    public class SiteInformation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AbbreviateName { get; set; }
        public string SiteName { get; set; }
        public string SiteType { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Cn { get; set; }
        public int Priority { get; set; }
    }
}