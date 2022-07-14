
using System.Collections.Generic;

namespace AppWeb.CyaConsultorias.Models
{
    public class ConfiguracionServiciosModel
    {
        public List<ServicesModel> ServicesModel { get; set; }
    }

    public class ServicesModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string UrlSmallBackgroundImage { get; set; }
        public string UrlBigBackgroundImage { get; set; }
        public string OnClick { get; set; }
        public string Href { get; set; }
        public string UrlIcon { get; set; }
        public string TitleHover { get; set; }
        public string SummeryIndex { get; set; }
        public List<SectionModel> Sections { get; set; }
        public List<DetailModel> Details { get; set; }
    }

    public class SectionModel
    {
        public string Title { get; set; }
        public string Summery { get; set; }
        public string UrlIcon { get; set; }
        public string Href { get; set; }
    }

    public class DetailModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string UrlImage { get; set; }
        public string Id { get; set; }
    }
}
