using System;
using System.IO;
using System.Text;

namespace Frc1360.DriverStation.RobotComm.Utilities
{
    public static class IOUtils
    {
        public static string ReadString1360(this BinaryReader r) => Encoding.UTF8.GetString(r.ReadBytes(r.ReadBytes(4).UInt32Big(0).Signed()));

        public static void WriteString1360(this BinaryWriter w, string s)
        {
            var b = Encoding.UTF8.GetBytes(s);
            w.Write(b.Length.Unsigned().BigEndian());
            w.Write(b);
        }

        public static float ReadFloat1360(this BinaryReader r) => BitConverter.ToSingle(BitConverter.GetBytes(UInt32Big(r.ReadBytes(4), 0)), 0);
        
        public static Guid ReadGuid(this BinaryReader r) => new Guid(r.ReadBytes(16));

        public static void WriteGuid(this BinaryWriter w, Guid g) => w.Write(g.ToByteArray());

        public static Version ReadVersion(this BinaryReader r) => new Version(r.ReadInt32(), r.ReadInt32(), r.ReadInt32(), r.ReadInt32());

        public static void WriteVersion(this BinaryWriter w, Version v)
        {
            w.Write(v.Major);
            w.Write(v.Minor);
            w.Write(v.Build);
            w.Write(v.Revision);
        }

        public static ushort ReadUInt16Big(this BinaryReader r) => (ushort)((r.ReadByte() << 8) + r.ReadByte());

        public static void WriteUInt16Big(this BinaryWriter w, ushort value)
        {
            w.Write((byte)(value >> 8));
            w.Write((byte)(value & 0xFF));
        }

        public static void CopyTo(this DirectoryInfo src, DirectoryInfo dest)
        {
            foreach (var d in src.EnumerateDirectories())
                d.CopyTo(dest.CreateSubdirectory(d.Name));
            foreach (var f in src.EnumerateFiles())
                f.CopyTo(dest.FullName + "\\" + f.Name);
        }

        public static string String1360(this byte[] a, int offset)
        {
            int len = a.UInt16Big(offset);
            if (a.Length - offset - 2 < len)
                throw new ArgumentOutOfRangeException("Too few bytes available after offset!");
            return Encoding.UTF8.GetString(a, offset + 2, len);
        }

        public static byte[] String1360(this string s)
        {
            byte[] a = new byte[s.Length + 4];
            Array.Copy(s.Length.Unsigned().BigEndian(), a, 4);
            Array.Copy(Encoding.UTF8.GetBytes(s), 0, a, 4, s.Length);
            return a;
        }

        public static ushort UInt16Big(this byte[] a, int offset)
        {
            if (a.Length - offset < 2)
                throw new ArgumentOutOfRangeException("Less than 2 bytes available after offset!");
            return (ushort)((a[offset] << 8) + a[offset + 1]);
        }

        public static uint UInt32Big(this byte[] a, int offset)
        {
            if (a.Length - offset < 4)
                throw new ArgumentOutOfRangeException("Less than 4 bytes available after offset!");
            return (uint)((a[offset] << 24) + (a[offset + 1] << 16) + (a[offset + 2] << 8) + a[offset + 3]);
        }

        public static ulong UInt64Big(this byte[] a, int offset)
        {
            if (a.Length - offset < 8)
                throw new ArgumentOutOfRangeException("Less than 8 bytes available after offset!");
            return (ulong)((a[offset] << 56) + (a[offset + 1] << 48) + (a[offset + 2] << 40) + (a[offset + 3] << 32) + (a[offset + 4] << 24) + (a[offset + 5] << 16) + (a[offset + 6] << 8) + a[offset + 7]);
        }

        public static byte[] BigEndian(this ushort v) => new[] { (byte)(v >> 8), (byte)(v & 0xFF) };

        public static byte[] BigEndian(this uint v) => new[] { (byte)(v >> 24), (byte)(v >> 16 & 0xFF), (byte)(v >> 8 & 0xFF), (byte)(v & 0xFF) };

        public static byte[] BigEndian(this ulong v) => new[] { (byte)(v >> 56), (byte)(v >> 48 & 0xFF), (byte)(v >> 40 & 0xFF), (byte)(v >> 32 & 0xFF), (byte)(v >> 24 & 0xFF), (byte)(v >> 16 & 0xFF), (byte)(v >> 8 & 0xFF), (byte)(v & 0xFF) };

        public static unsafe byte Unsigned(this sbyte v) => *(byte*)&v;

        public static unsafe ushort Unsigned(this short v) => *(ushort*)&v;

        public static unsafe uint Unsigned(this int v) => *(uint*)&v;

        public static unsafe ulong Unsigned(this long v) => *(ulong*)&v;

        public static unsafe sbyte Signed(this byte v) => *(sbyte*)&v;

        public static unsafe short Signed(this ushort v) => *(short*)&v;

        public static unsafe int Signed(this uint v) => *(int*)&v;

        public static unsafe long Signed(this ulong v) => *(long*)&v;
    }
}