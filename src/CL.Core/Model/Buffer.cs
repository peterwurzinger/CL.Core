using CL.Core.API;
using System;
using System.Collections.Generic;

namespace CL.Core.Model
{
    public class Buffer : BufferBase
    {
        private bool _disposed;
        private readonly IList<SubBuffer> _attachedSubBuffers;

        internal Buffer(IOpenClApi api, Context context)
            : base(api, context)
        {
            throw new NotImplementedException();

            _attachedSubBuffers = new List<SubBuffer>();
        }

        public SubBuffer CreateSubBuffer()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            var subBuffer = new SubBuffer(Api, this);
            _attachedSubBuffers.Add(subBuffer);

            return subBuffer;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    foreach (var subBuffer in _attachedSubBuffers)
                        subBuffer.Dispose();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}
