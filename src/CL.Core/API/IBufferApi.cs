using System;

namespace CL.Core.API
{
    public interface IBufferApi
    {
        /// <summary>
        /// Creates a buffer object.
        /// </summary>
        /// <param name="context">A valid OpenCL context used to create the buffer object.</param>
        /// <param name="flags">A bit-field that is used to specify allocation and usage information such as the memory arena that should be used to allocate the buffer object and how it will be used.</param>
        /// <param name="size">The size in bytes of the buffer memory object to be allocated.</param>
        /// <param name="hostPtr">A pointer to the buffer data that may already be allocated by the application. The size of the buffer that host_ptr points to must be greater than or equal to the size bytes. </param>
        /// <param name="errorCode">Returns an appropriate error code. If errorCode is NULL, no error code is returned. </param>
        /// <returns></returns>
        
        //TODO: Make size_t - params ulong?
        IntPtr clCreateBuffer(IntPtr context, MemoryFlags flags, uint size, IntPtr hostPtr, out OpenClErrorCode errorCode);

        /// <summary>
        /// Creates a buffer object (referred to as a sub-buffer object) from an existing buffer object. 
        /// </summary>
        /// <param name="buffer">A valid object. Cannot be a sub-buffer object.</param>
        /// <param name="flags">A bit-field that is used to specify allocation and usage information about the image memory object being created.</param>
        /// <param name="bufferCreateType">The type of buffer object to be created. The supported value for buffer_create_type is CL_BUFFER_CREATE_TYPE_REGION, which create a buffer object that represents a specific region in buffer.</param>
        /// <param name="bufferCreateInfo">buffer_create_info is a pointer to the following structure:
        /// 
        /// typedef struct _cl_buffer_region {
        /// size_t origin;
        /// size_t size;
        /// } cl_buffer_region;
        /// (origin, size) defines the offset and size in bytes in buffer.</param>
        /// <param name="errorCode">Returns an appropriate error code. If errorCode is NULL, no error code is returned. Otherwise, it returns one of the following error in errorCode: 
        /// * CL_INVALID_MEM_OBJECT if buffer is not a valid buffer object or is a sub-buffer object. 
        /// * CL_INVALID_VALUE if values specified in flags are not valid; or if value specified in buffer_create_type is not valid; or if value(s) specified in buffer_create_info (for a given buffer_create_type) is not valid or if buffer_create_info is NULL. 
        /// * CL_OUT_OF_RESOURCES if there is a failure to allocate resources required by the OpenCL implementation on the device. 
        /// * CL_OUT_OF_HOST_MEMORY if there is a failure to allocate resources required by the OpenCL implementation on the host. </param>
        /// <returns>Returns a valid non-zero buffer object and errorCode is set to CL_SUCCESS if the buffer object is created successfully.</returns>
        IntPtr clCreateSubBuffer(IntPtr buffer, MemoryFlags flags, BufferCreateType bufferCreateType,
            IntPtr bufferCreateInfo, out OpenClErrorCode errorCode);

        /// <summary>
        /// Enqueue commands to read from a buffer object to host memory. 
        /// </summary>
        /// <param name="commandQueue">Refers to the command-queue in which the read command will be queued. command_queue and buffer must be created with the same OpenCL context. </param>
        /// <param name="buffer">Refers to a valid buffer object.</param>
        /// <param name="blockingRead">Indicates if the read operations are blocking or non-blocking. If blocking_read is <c>true</c> i.e. the read command is blocking, clEnqueueReadBuffer does not return until the buffer data has been read and copied into memory pointed to by <param name="ptr">ptr</param>. 
        /// If blocking_read is <c>false</c> i.e. the read command is non-blocking, clEnqueueReadBuffer queues a non-blocking read command and returns. The contents of the buffer that ptr points to cannot be used until the read command has completed. The event argument returns an event object which can be used to query the execution status of the read command. When the read command has completed, the contents of the buffer that ptr points to can be used by the application. </param>
        /// <param name="offset">The offset in bytes in the buffer object to read from.</param>
        /// <param name="cb">The size in bytes of data being read.</param>
        /// <param name="ptr">The pointer to buffer in host memory where data is to be read into. </param>
        /// <param name="numEventsInWaitList">event_wait_list and num_events_in_wait_list specify events that need to complete before this particular command can be executed. If event_wait_list is NULL, then this particular command does not wait on any event to complete. If event_wait_list is NULL, num_events_in_wait_list must be 0. If event_wait_list is not NULL, the list of events pointed to by event_wait_list must be valid and num_events_in_wait_list must be greater than 0. The events specified in event_wait_list act as synchronization points. The context associated with events in event_wait_list and command_queue must be the same.</param>
        /// <param name="eventWaitList">event_wait_list and num_events_in_wait_list specify events that need to complete before this particular command can be executed. If event_wait_list is NULL, then this particular command does not wait on any event to complete. If event_wait_list is NULL, num_events_in_wait_list must be 0. If event_wait_list is not NULL, the list of events pointed to by event_wait_list must be valid and num_events_in_wait_list must be greater than 0. The events specified in event_wait_list act as synchronization points. The context associated with events in event_wait_list and command_queue must be the same. </param>
        /// <param name="event">Returns an event object that identifies this particular read command and can be used to query or queue a wait for this particular command to complete. event can be NULL in which case it will not be possible for the application to query the status of this command or queue a wait for this command to complete. </param>
        /// <returns>clEnqueueReadBuffer returns CL_SUCCESS if the function is executed successfully. Otherwise, it returns one of the following errors: 
        /// * CL_INVALID_COMMAND_QUEUE if command_queue is not a valid command-queue. 
        /// * CL_INVALID_CONTEXT if the context associated with command_queue and buffer are not the same or if the context associated with command_queue and events in event_wait_list are not the same. 
        /// * CL_INVALID_MEM_OBJECT if buffer is not a valid buffer object. 
        /// * CL_INVALID_VALUE if the region being read specified by (offset, cb) is out of bounds or if ptr is a NULL value. 
        /// * CL_INVALID_EVENT_WAIT_LIST if event_wait_list is NULL and num_events_in_wait_list greater than 0, or event_wait_list is not NULL and num_events_in_wait_list is 0, or if event objects in event_wait_list are not valid events. 
        /// * CL_MEM_OBJECT_ALLOCATION_FAILURE if there is a failure to allocate memory for data store associated with buffer. 
        /// * CL_OUT_OF_HOST_MEMORY if there is a failure to allocate resources required by the OpenCL implementation on the host. </returns>
        OpenClErrorCode clEnqueueReadBuffer(IntPtr commandQueue, IntPtr buffer, bool blockingRead, uint offset, uint cb, IntPtr ptr, uint numEventsInWaitList, IntPtr[] eventWaitList, out IntPtr @event);

        /// <summary>
        /// Enqueue commands to write to a buffer object from host memory. 
        /// </summary>
        /// <param name="commandQueue">Refers to the command-queue in which the write command will be queued. command_queue and buffer must be created with the same OpenCL context. </param>
        /// <param name="buffer">Refers to a valid buffer object. </param>
        /// <param name="blockingWrite">Indicates if the write operations are blocking or nonblocking. 
        /// If blocking_write is CL_TRUE, the OpenCL implementation copies the data referred to by ptr and enqueues the write operation in the command-queue. The memory pointed to by ptr can be reused by the application after the clEnqueueWriteBuffer call returns. 
        /// If blocking_write is CL_FALSE, the OpenCL implementation will use ptr to perform a nonblocking write. As the write is non-blocking the implementation can return immediately. The memory pointed to by ptr cannot be reused by the application after the call returns. The event argument returns an event object which can be used to query the execution status of the write command. When the write command has completed, the memory pointed to by ptr can then be reused by the application. </param>
        /// <param name="offset">The offset in bytes in the buffer object to write to.</param>
        /// <param name="cb">The size in bytes of data being written. </param>
        /// <param name="ptr">The pointer to buffer in host memory where data is to be written from. </param>
        /// <param name="numEventsInWaitList">event_wait_list and num_events_in_wait_list specify events that need to complete before this particular command can be executed. If event_wait_list is NULL, then this particular command does not wait on any event to complete. If event_wait_list is NULL, num_events_in_wait_list must be 0. If event_wait_list is not NULL, the list of events pointed to by event_wait_list must be valid and num_events_in_wait_list must be greater than 0. The events specified in event_wait_list act as synchronization points. The context associated with events in event_wait_list and command_queue must be the same. </param>
        /// <param name="eventWaitList">event_wait_list and num_events_in_wait_list specify events that need to complete before this particular command can be executed. If event_wait_list is NULL, then this particular command does not wait on any event to complete. If event_wait_list is NULL, num_events_in_wait_list must be 0. If event_wait_list is not NULL, the list of events pointed to by event_wait_list must be valid and num_events_in_wait_list must be greater than 0. The events specified in event_wait_list act as synchronization points. The context associated with events in event_wait_list and command_queue must be the same. </param>
        /// <param name="event">Returns an event object that identifies this particular write command and can be used to query or queue a wait for this particular command to complete. event can be NULL in which case it will not be possible for the application to query the status of this command or queue a wait for this command to complete. </param>
        /// <returns>clEnqueueWriteBuffer returns CL_SUCCESS if the function is executed successfully. Otherwise, it returns one of the following errors: 
        /// * CL_INVALID_COMMAND_QUEUE if command_queue is not a valid command-queue. 
        /// * CL_INVALID_CONTEXT if the context associated with command_queue and buffer are not the same or if the context associated with command_queue and events in event_wait_list are not the same. 
        /// * CL_INVALID_MEM_OBJECT if buffer is not a valid buffer object. 
        /// * CL_INVALID_VALUE if the region being written specified by (offset, cb) is out of bounds or if ptr is a NULL value. 
        /// * CL_INVALID_EVENT_WAIT_LIST if event_wait_list is NULL and num_events_in_wait_list greater than 0, or event_wait_list is not NULL and num_events_in_wait_list is 0, or if event objects in event_wait_list are not valid events. 
        /// * CL_MEM_OBJECT_ALLOCATION_FAILURE if there is a failure to allocate memory for data store associated with buffer. 
        /// * CL_OUT_OF_HOST_MEMORY if there is a failure to allocate resources required by the OpenCL implementation on the host. </returns>
        OpenClErrorCode clEnqueueWriteBuffer(IntPtr commandQueue, IntPtr buffer, bool blockingWrite, uint offset,
            uint cb, IntPtr ptr, uint numEventsInWaitList, IntPtr[] eventWaitList, out IntPtr @event);

        /// <summary>
        /// Decrements the memory object reference count. 
        /// </summary>
        /// <param name="memobj"></param>
        /// <returns>Returns CL_SUCCESS if the function is executed successfully. Otherwise, it returns one of the following errors:
        /// * CL_INVALID_MEM_OBJECT if <param name="memobj">memobj</param> is a not a valid memory object. 
        /// * CL_OUT_OF_RESOURCES if there is a failure to allocate resources required by the OpenCL implementation on the device. 
        /// * CL_OUT_OF_HOST_MEMORY if there is a failure to allocate resources required by the OpenCL implementation on the host. </returns>
        OpenClErrorCode clReleaseMemObject(IntPtr memobj);
    }
}
