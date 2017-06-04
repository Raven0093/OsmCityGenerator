namespace Assets.Scripts.Enums
{
    public enum HighwayTypeEnum
    {

        [OSMtoSharp.Enums.Helpers.Enum("")]
        None,
        [OSMtoSharp.Enums.Helpers.Enum("motorway")]
        Motorway,
        [OSMtoSharp.Enums.Helpers.Enum("trunk")]
        Trunk,
        [OSMtoSharp.Enums.Helpers.Enum("primary")]
        Primary,
        [OSMtoSharp.Enums.Helpers.Enum("secondary")]
        Secondary,
        [OSMtoSharp.Enums.Helpers.Enum("tertiary")]
        Tertiary,
        [OSMtoSharp.Enums.Helpers.Enum("unclassified")]
        Unclassified,
        [OSMtoSharp.Enums.Helpers.Enum("residential")]
        Residential,
        [OSMtoSharp.Enums.Helpers.Enum("service")]
        Service,
        [OSMtoSharp.Enums.Helpers.Enum("proposed")]
        Proposed,
        [OSMtoSharp.Enums.Helpers.Enum("Construction")]
        Construction,
        [OSMtoSharp.Enums.Helpers.Enum("cycleway")]
        Cycleway
    }
}