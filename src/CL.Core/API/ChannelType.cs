namespace CL.Core.API
{
    /// <summary>
    /// Describes the size of the channel data type. The number of bits per element determined by the image_channel_data_type and image_channel_order must be a power of two.
    /// </summary>
    public enum ChannelType
    {
        /// <summary>
        ///Each channel component is a normalized signed 8-bit integer value.
        /// </summary>
        SNORM_INT8 = 0b1_0000_1101_0000,

        /// <summary>
        ///Each channel component is a normalized signed 16-bit integer value.
        /// </summary>
        SNORM_INT16 = 0b1_0000_1101_0001,

        /// <summary>
        ///Each channel component is a normalized unsigned 8-bit integer value.
        /// </summary>
        UNORM_INT8 = 0b1_0000_1101_0010,

        /// <summary>
        ///Each channel component is a normalized unsigned 16-bit integer value.
        /// </summary>
        UNORM_INT16 = 0b1_0000_1101_0011,

        /// <summary>
        ///Represents a normalized 5-6-5 3-channel RGB image. The channel order must be RGB.
        /// </summary>
        UNORM_SHORT_565 = 0b1_0000_1101_0100,

        /// <summary>
        ///Represents a normalized x-5-5-5 4-channel xRGB image. The channel order must be RGB.
        /// </summary>
        UNORM_SHORT_555 = 0b1_0000_1101_0101,

        /// <summary>
        ///Represents a normalized x-10-10-10 4-channel xRGB image. The channel order must be RGB.
        /// </summary>
        UNORM_INT_101010 = 0b1_0000_1101_0110,

        /// <summary>
        ///Each channel component is an unnormalized signed 8-bit integer value.
        /// </summary>
        SIGNED_INT8 = 0b1_0000_1101_0111,

        /// <summary>
        ///Each channel component is an unnormalized signed 16-bit integer value.
        /// </summary>
        SIGNED_INT16 = 0b1_0000_1101_1000,

        /// <summary>
        ///Each channel component is an unnormalized signed 32-bit integer value.
        /// </summary>
        SIGNED_INT32 = 0b1_0000_1101_1001,

        /// <summary>
        ///Each channel component is an unnormalized unsigned 8-bit integer value.
        /// </summary>
        UNSIGNED_INT8 = 0b1_0000_1101_1010,

        /// <summary>
        ///Each channel component is an unnormalized unsigned 16-bit integer value.
        /// </summary>
        UNSIGNED_INT16 = 0b1_0000_1101_1011,

        /// <summary>
        ///Each channel component is an unnormalized unsigned 32-bit integer value.
        /// </summary>
        UNSIGNED_INT32 = 0b1_0000_1101_1100,

        /// <summary>
        ///Each channel component is a 16-bit half-float value.
        /// </summary>
        HALF_FLOAT = 0b1_0000_1101_1101,

        /// <summary>
        ///Each channel component is a single precision floating-point value.
        /// </summary>
        FLOAT = 0b1_0000_1101_1110
    }
}