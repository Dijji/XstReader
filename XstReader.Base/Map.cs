// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace XstReader
{
    // A set of static methods to construct C# types from byte arrays
    // These allow us to build usable types from xst file data
    static class Map
    {
        // Read enough bytes from the file stream at the current position to populate specified type T
        public static T ReadType<T>(FileStream fs)
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
            fs.Read(buffer, 0, Marshal.SizeOf(typeof(T)));
            return MapType<T>(buffer);
        }

        //  Map part of a byte array data buffer onto the specified type T
        public static unsafe T MapType<T>(byte[] buffer, int offset = 0)
        {
            int size = Marshal.SizeOf(typeof(T));
            if (offset < 0 || offset + size > buffer.Length)
                throw new XstException($"Out of bounds error attempting to map {typeof(T).Name} from byte array of length {buffer.Length} at offset {offset}");
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            T temp = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject() + offset, typeof(T));
            handle.Free();
            return temp;
        }

        // Map part of a byte array data buffer onto an array of count occurrences of the specified type T
        public static unsafe T[] MapArray<T>(byte[] buffer, int offset, int count)
        {
            T[] temp = new T[count];
            int size = Marshal.SizeOf(typeof(T));
            if (offset < 0 || offset + count * size > buffer.Length)
                throw new XstException($"Out of bounds error attempting to map array of {count} {typeof(T).Name}s from byte array of length {buffer.Length} at offset {offset}");
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            for (int i = 0; i < count; i++)
            {
                temp[i] = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject() + offset + i * size, typeof(T));
            }
            handle.Free();
            return temp;
        }

        // Map part of an unsafe data buffer onto the specified type T
        // The optional entry size can be offered as a crosscheck on the accuracy of the layouts
        public static unsafe T MapType<T>(byte* buffer, int buflen, int offset, int entrySize = 0)
        {
            int size = Marshal.SizeOf(typeof(T));
            if (entrySize != 0 && size > entrySize)
                throw new XstException($"Mapping error: requested map of {typeof(T).Name} whose size is {size} but entry size is only {entrySize}");
            if (offset < 0 || offset + size > buflen)
                throw new XstException($"Out of bounds error attempting to map {typeof(T).Name} from buffer length {buflen} at offset {offset}");
            return (T)Marshal.PtrToStructure(new IntPtr(buffer + offset), typeof(T));
        }
    }
}
