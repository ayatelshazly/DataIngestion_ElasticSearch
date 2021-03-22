using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Models.ReferenceModel
{
    public class CollectionRefModel
    {
         

        public Int64? Id { get; set; }
        public string Name { get; set; }
        public Int64? UPC { get; set; }
        public string URL { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public bool? IsCompilation { get; set; }
        public string Label { get; set; }
        public string ImageUrl { get; set; }
    }
}
