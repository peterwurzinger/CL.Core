using System;

namespace CL.Core.API
{
    [Flags]
    public enum MemoryFlags
    {
        /// <summary>
        /// This flag specifies that the memory object will be read and written by a kernel. This is the default. 
        /// </summary>
        ReadWrite = 0b0000_0001,

        /// <summary>
        /// his flags specifies that the memory object will be written but not read by a kernel.
        /// Reading from a buffer or image object created with CL_MEM_WRITE_ONLY inside a kernel is undefined.
        /// </summary>
        WriteOnly = 0b0000_0010,

        /// <summary>
        /// This flag specifies that the memory object is a read-only memory object when used inside a kernel.
        /// Writing to a buffer or image object created with CL_MEM_READ_ONLY inside a kernel is undefined.
        /// </summary>
        ReadOnly = 0b0000_0100,

        /// <summary>
        /// This flag is valid only if host_ptr is not NULL. If specified, it indicates that the application wants the OpenCL implementation to use memory referenced by host_ptr as the storage bits for the memory object.
        /// OpenCL implementations are allowed to cache the buffer contents pointed to by host_ptr in device memory. This cached copy can be used when kernels are executed on a device.
        /// The result of OpenCL commands that operate on multiple buffer objects created with the same host_ptr or overlapping host regions is considered to be undefined.
        /// </summary>
        UseHostPointer = 0b0000_1000,

        /// <summary>
        /// This flag specifies that the application wants the OpenCL implementation to allocate memory from host accessible memory.
        /// CL_MEM_ALLOC_HOST_PTR and CL_MEM_USE_HOST_PTR are mutually exclusive.
        /// </summary>
        AllocHostPointer= 0b0001_0000,

        /// <summary>
        /// This flag is valid only if host_ptr is not NULL. If specified, it indicates that the application wants the OpenCL implementation to allocate memory for the memory object and copy the data from memory referenced by host_ptr. 
        /// CL_MEM_COPY_HOST_PTR and CL_MEM_USE_HOST_PTR are mutually exclusive. 
        /// CL_MEM_COPY_HOST_PTR can be used with CL_MEM_ALLOC_HOST_PTR to initialize the contents of the cl_mem object allocated using host-accessible (e.g. PCIe) memory. 
        /// </summary>
        CopyHostPointer = 0b0010_0000
    }
}