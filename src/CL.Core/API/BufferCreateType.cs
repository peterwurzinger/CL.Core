namespace CL.Core.API
{
    /// <summary>
    /// The type of buffer object to be created. The supported value for buffer_create_type is CL_BUFFER_CREATE_TYPE_REGION, which create a buffer object that represents a specific region in buffer.
    /// </summary>
    public enum BufferCreateType
    {
        Region = 0b1_0010_0010_0000
    }
}