using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Scripts.Enums
{
    public enum PowerTypeEnum
    {
        [OSMtoSharp.Enums.Helpers.Enum("")]
        None,
        [OSMtoSharp.Enums.Helpers.Enum("line")]
        Line,
        [OSMtoSharp.Enums.Helpers.Enum("minor_line")]
        MinorLine,
        [OSMtoSharp.Enums.Helpers.Enum("tower")]
        Tower,
        [OSMtoSharp.Enums.Helpers.Enum("pole")]
        Pole
    }
}
