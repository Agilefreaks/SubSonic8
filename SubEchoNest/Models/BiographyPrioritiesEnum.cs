namespace SubEchoNest.Models
{
    using System;

    [Flags]
    public enum BiographyPrioritiesEnum
    {
        None = 0x0,
        LongestText = 0x01,
        Wikipedia = 0x02,
        LastFm = 0x04
    }
}