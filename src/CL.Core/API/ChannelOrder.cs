namespace CL.Core.API
{
    /// <summary>
    /// Specifies the number of channels and the channel layout i.e. the memory layout in which channels are stored in the image
    /// </summary>
    public enum ChannelOrder
    {
        R = 0b1_0000_1011_0000,
        A = 0b1_0000_1011_0001,
        RG = 0b1_0000_1011_0010,
        RA = 0b1_0000_1011_0011,

        /// <summary>
        /// This format can only be used if channel data type = UNORM_SHORT_565, UNORM_SHORT_555 or UNORM_INT101010.
        /// </summary>
        RGB = 0b1_0000_1011_0100,

        RGBA = 0b1_0000_1011_0101,
        Rx = 0b1_0000_1011_1010,
        RGx = 0b1_0000_1011_1011,

        /// <summary>
        /// This format can only be used if channel data type = CL_UNORM_INT8, CL_SNORM_INT8, CL_SIGNED_INT8 or CL_UNSIGNED_INT8.
        /// </summary>
        BGRA = 0b1_0000_1011_0110,

        /// <summary>
        /// This format can only be used if channel data type = UNORM_INT8, SNORM_INT8, SIGNED_INT8 or UNSIGNED_INT8.
        /// </summary>
        ARGB = 0b1_0000_1011_0111,

        /// <summary>
        /// This format can only be used if channel data type = UNORM_INT8, UNORM_INT16, SNORM_INT8, SNORM_INT16, HALF_FLOAT, or FLOAT.
        /// </summary>
        Intensity = 0b1_0000_1011_1000,

        /// <summary>
        /// This format can only be used if channel data type = UNORM_INT8, UNORM_INT16, SNORM_INT8, SNORM_INT16, HALF_FLOAT, or FLOAT.
        /// </summary>
        Luminance = 0b1_0000_1011_1001,

        /// <summary>
        /// This format can only be used if channel data type = CL_UNORM_SHORT_565, CL_UNORM_SHORT_555 or CL_UNORM_INT101010.
        /// </summary>
        RGBx = 0b1_0000_1011_1100,
    }
}