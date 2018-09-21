using System;

namespace CL.Core.API
{
    public static class OpenClErrorCodeExtensions
    {
        public static void ThrowOnError(this OpenClErrorCode error)
        {
            //TODO: Decide which exception to throw based on errorcode
            if (error != OpenClErrorCode.Success)
                throw new ClCoreException(error);
        }

    }

    //TODO: ClCoreException should only be the base exception. Introduce a bunch of exceptions to cover groups of errors
    public class ClCoreException : Exception
    {
        public ClCoreException(string errorMessage) : base(errorMessage)
        {

        }

        public ClCoreException(OpenClErrorCode error) : this(error.ToString())
        {

        }

        public ClCoreException()
        {
        }

        public ClCoreException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
