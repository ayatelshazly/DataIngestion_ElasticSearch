using Events.Models.ReferenceModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events
{
    public class AlbumModelEvent
    {
        public CollectionRefModel Collection { get; set; }
        public List<ArtistRefModel> Artist { get; set; }
    }
}
