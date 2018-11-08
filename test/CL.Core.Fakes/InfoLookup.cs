using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CL.Core.Fakes
{
    public sealed class InfoLookup<TKey> : IReadOnlyDictionary<TKey, Memory<byte>>
        where TKey : Enum
    {
        private readonly Dictionary<TKey, Memory<byte>> _infos;

        public InfoLookup()
        {
            _infos = new Dictionary<TKey, Memory<byte>>();
        }

        public void Add<TValue>(TKey key, TValue value)
            where TValue : unmanaged
        {
            if (ContainsKey(key))
                throw new InvalidOperationException();

            var size = typeof(TValue).IsEnum
                ? Marshal.SizeOf(Enum.GetUnderlyingType(typeof(TValue)))
                : Marshal.SizeOf(value);

            var storage = new byte[size];
            MemoryMarshal.Write(storage, ref value);
            _infos.Add(key, storage);
        }

        public void Add<TValue>(TKey key, ReadOnlySpan<TValue> value)
            where TValue : struct
        {
            if (ContainsKey(key))
                throw new InvalidOperationException();

            _infos.Add(key, MemoryMarshal.Cast<TValue, byte>(value).ToArray());
        }

        public void Add(TKey key, string value)
        {
            Add(key, value.AsSpan());
        }

        public unsafe void CopyTo(TKey key, IntPtr memoryLocation, int length)
        {
            var memorySpan = new Span<byte>(memoryLocation.ToPointer(), length);
            _infos[key].Span.CopyTo(memorySpan);
        }

        public IEnumerator<KeyValuePair<TKey, Memory<byte>>> GetEnumerator()
        {
            return _infos.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _infos.Count;

        public bool ContainsKey(TKey key)
        {
            return _infos.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out Memory<byte> value)
        {
            return _infos.TryGetValue(key, out value);
        }

        public Memory<byte> this[TKey key] => _infos[key];

        public IEnumerable<TKey> Keys => _infos.Keys;
        public IEnumerable<Memory<byte>> Values => _infos.Values;
    }
}