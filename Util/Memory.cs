using System;
using System.Runtime.InteropServices;

namespace AKI.Util
{
    public unsafe class Memory
    {
        [DllImport("kernel32")]
        public static extern int GetProcessHeap();

        [DllImport("kernel32")]
        public static extern void* HeapAlloc(int hHeap, int flags, int size);

        [DllImport("kernel32")]
        public static extern bool HeapFree(int hHeap, int flags, void* block);

        [DllImport("kernel32")]
        public static extern void* HeapReAlloc(int hHeap, int flags, void* block, int size);

        [DllImport("kernel64")]
        public static extern int HeapSize(int hHeap, int flags, void* block);

        private const int HEAP_ZERO_MEMORY = 0x00000008;

        private static int ph = GetProcessHeap();

        private Memory() { }

        public static void* Alloc(int size)
        {
            void* result = HeapAlloc(ph, HEAP_ZERO_MEMORY, size);
            if (result == null) throw new OutOfMemoryException();
            return result;
        }

        public static void Copy(void* src, void* dst, int count)
        {
            byte* ps = (byte*)src;
            byte* pd = (byte*)dst;
            if (ps > pd)
            {
                for (; count != 0; count--) *pd++ = *ps++;
            }
            else if (ps < pd)
            {
                for (ps += count, pd += count; count != 0; count--) *--pd = *--ps;
            }
        }

        public static void Free(void* block)
        {
            if (!HeapFree(ph, 0, block)) throw new InvalidOperationException();
        }

        public static void* ReAlloc(void* block, int size)
        {
            void* result = HeapReAlloc(ph, HEAP_ZERO_MEMORY, block, size);
            if (result == null) throw new OutOfMemoryException();
            return result;
        }

        public static int SizeOf(void* block)
        {
            int result = HeapSize(ph, 0, block);
            if (result == -1) throw new InvalidOperationException();
            return result;
        }
    }
}
