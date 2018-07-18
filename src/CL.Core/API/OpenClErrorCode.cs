namespace CL.Core.API
{
    public enum OpenClErrorCode
    {
        Success = 0,

        #region Runtime & JIT errors

        DeviceNotFound = -1,
        DeviceNotAvailable = -2,
        CompilerNotAvailable = -3,
        MemoryObjectAllocationFailure = -4,
        OutOfResources = -5,
        OutOfHostMemory = -6,
        ProfilingInfoNotAvailable = -7,
        MemoryCopyOverlap = -8,
        ImageFormatMismatch = -9,
        ImageFormatNotSupported = -10,
        BuildProgramFailure = -11,
        MapFailure = -12,
        MisalignedSubBufferOffset = -13,
        ExecutionStatusErrorForEventsInWaitList = -14,
        CompileProgramFailure = -15,

        #endregion

        #region Compiler errors

        InvalidValue = -30,
        InvalidDeviceType = -31,
        InvalidPlatform = -32,
        InvalidDevice = -33,
        InvalidContext = -34,
        InvalidQueueProperties = -35,
        InvalidCommandQueue = -36,
        InvalidHostPointer = -37,
        InvalidMemoryObject = -38,
        InvalidIMageFormatDescriptor = -39,
        InvalidImageSize = -40,
        InvalidSampler = -41,
        InvalidBinary = -42,
        InvalidBuildOptions = -43,
        InvalidProgram = -44,
        InvalidProgramExecutable = -45,
        InvalidKernelName = -46,
        InvalidKernelDefinition = -47,
        InvalidKernel = -48,
        InvalidArgumentIndex = -49,
        InvalidArgumentValue = -50,
        InvalidArgumentSize = -51,
        InvalidKernelArguments = -52,
        InvalidWorkDimension = -53,
        InvalidWorkGroupSize = -54,
        InvalidWorkItemSize = -55,
        InvalidGlobalOffset = -56,
        InvalidEventWaitList = -57,
        InvalidEvent = -58,
        InvalidOperation = -59,
        // ReSharper disable once InconsistentNaming
        InvalidGLObject = -60,
        InvalidBufferSize = -61,
        // ReSharper disable once InconsistentNaming
        InvalidMIPLevel = -62,
        InvalidGlobalWorkSize = -63,
        InvalidProperty = -64,

        #endregion

    }
}
