﻿namespace CL.Core.API
{
    public enum DeviceInfoParameter
    {
        Type = 0b1_0000_0000_0000,
        VendorId = 0b1_0000_0000_0001,

        MaxComputeUnits = 0b1_0000_0000_0010,
        MaxWorkItemDimensions = 0b1_0000_0000_0011,
        MaxWorkGroupSize = 0b1_0000_0000_0100,
        MaxWorkItemSizes = 0b1_0000_0000_0101,

        PreferredVectorWidthChar = 0b1_0000_0000_0110,
        PreferredVectorWidthShort = 0b1_0000_0000_0111,
        PreferredVectorWidthInt = 0b1_0000_0000_1000,
        PreferredVectorWidthLong = 0b1_0000_0000_1001,
        PreferredVectorWidthFloat = 0b1_0000_0000_1010,
        PreferredVectorWidthDouble = 0b1_0000_0000_1011,

        MaxClockFrequency = 0b1_0000_0000_1100,
        AddressBits = 0b1_0000_0000_1101,
        MaxReadImageArgs = 0b1_0000_0000_1110,
        MaxWriteImageArgs = 0b1_0000_0000_1111,
        MaxMemAllocSize = 0b1_0000_0001_0000,
        Image2DMaxWidth = 0b1_0000_0001_0001,
        Image2DMaxHeight = 0b1_0000_0001_0010,
        Image3DMaxWidth = 0b1_0000_0001_0011,
        Image3DMaxHeight = 0b1_0000_0001_0100,
        Image3DMaxDepth = 0b1_0000_0001_0101,
        ImageSupport = 0b1_0000_0001_0110,
        MaxParameterSize = 0b1_0000_0001_0111,
        MaxSamplers = 0b1_0000_0001_1000,
        MemBaseAddrAlign = 0b1_0000_0001_1001,
        MinDataTypeAlignSize = 0b1_0000_0001_1010,
        SingleFpConfig = 0b1_0000_0001_1011,

        GlobalMemCacheType = 0b1_0000_0001_1100,
        GlobalMemCachelineSize = 0b1_0000_0001_1101,
        GlobalMemCacheSize = 0b1_0000_0001_1111,
        GlobalMemSize = 0b1_0000_0001_1111,
        MaxConstantBufferSize = 0b1_0000_0010_0000,
        MaxConstantArgs = 0b1_0000_0010_0001,
        LocalMemType = 0b1_0000_0010_0010,
        LocalMemSize = 0b1_0000_0010_0011,

        ErrorCorrectionSupport = 0b1_0000_0010_0100,
        ProfilingTimerResolution = 0b1_0000_0010_0101,
        // ReSharper disable once IdentifierTypo
        EndianLittle = 0b1_0000_0010_0110,
        DeviceAvailable = 0b1_0000_0010_0111,
        CompilerAvailable = 0b1_0000_0010_1000,
        ExecutionCapabilities = 0b1_0000_0010_1001,
        QueueProperties = 0b1_0000_0010_1010,
        Name = 0b1_0000_0010_1011,
        Vendor = 0b1_0000_0010_1100,
        DriverVersion = 0b1_0000_0010_1101,
        Profile = 0b1_0000_0010_1110,
        Version = 0b1_0000_0010_1111,
        Extensions = 0b1_0000_0011_0000,
        Platform = 0b1_0000_0011_0001,
        DoubleFpConfig = 0b1_0000_0011_0010,
        HalfFpConfig = 0b1_0000_0011_0011,
        PreferredVectorWidthHalf = 0b1_0000_0011_0100,
        HostUnifiedMemory = 0b1_0000_0011_0101,

        NativeVectorWidthChar = 0b1_0000_0011_0110,
        NativeVectorWidthShort = 0b1_0000_0011_0111,
        NativeVectorWidthInt = 0b1_0000_0011_1000,
        NativeVectorWidthLong = 0b1_0000_0011_1001,
        NativeVectorWidthFloat = 0b1_0000_0011_1010,
        NativeVectorWidthDouble = 0b1_0000_0011_1011,
        NativeVectorWidthHalf = 0b1_0000_0011_1100,

        OpenClCVersion = 0b1_0000_0011_1101
    }
}