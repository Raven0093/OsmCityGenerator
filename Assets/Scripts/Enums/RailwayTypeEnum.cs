using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Scripts.Enums
{
    public enum RailwayTypeEnum
    {
        [OSMtoSharp.Enums.Helpers.Enum("")]
        None,
        [OSMtoSharp.Enums.Helpers.Enum("abandoned")]
        Abandoned,
        [OSMtoSharp.Enums.Helpers.Enum("construction")]
        Construction,
        [OSMtoSharp.Enums.Helpers.Enum("disused")]
        Disused,
        [OSMtoSharp.Enums.Helpers.Enum("funicular")]
        Funicular,
        [OSMtoSharp.Enums.Helpers.Enum("light_rail")]
        Light_rail,
        [OSMtoSharp.Enums.Helpers.Enum("miniature")]
        Miniature,
        [OSMtoSharp.Enums.Helpers.Enum("monorail")]
        Monorail,
        [OSMtoSharp.Enums.Helpers.Enum("narrow_gauge")]
        Narrow_gauge,
        [OSMtoSharp.Enums.Helpers.Enum("preserved")]
        Preserved,
        [OSMtoSharp.Enums.Helpers.Enum("rail")]
        Rail,
        [OSMtoSharp.Enums.Helpers.Enum("subway")]
        Subway,
        [OSMtoSharp.Enums.Helpers.Enum("tram")]
        Tram,
    }
}
