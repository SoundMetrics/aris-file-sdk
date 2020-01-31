// Copyright 2014-2020 Sound Metrics Corp. All Rights Reserved.

namespace Aris.FileTypes
{
    /// <summary>
    /// ARIS models are defined by which frequencies
    /// they support. In that sense there are three
    /// models.
    /// </summary>
    public enum ArisSystemType
    {
        /// <summary>
        /// Signifies an ARIS 1800.
        /// </summary>
        Aris1800 = 0,

        /// <summary>
        /// Signifies an ARIS 3000.
        /// </summary>
        Aris3000 = 1,

        /// <summary>
        /// Signifies an ARIS 1200.
        /// </summary>
        Aris1200 = 2
    }
}
