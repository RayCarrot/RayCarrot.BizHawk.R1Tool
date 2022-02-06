using BizHawk.Client.Common;
using System;
using System.Linq;

namespace RayCarrot.BizHawk.R1Tool;

public class BizHawkBinarySerializer : BinarySerializer
{
    public BizHawkBinarySerializer(IMemoryApi mem)
    {
        Mem = mem;
        CurrentAddressValue = 0x00;
    }

    public override T Serialize<T>(T value, Action<T> onPreSerializing = null, string name = null)
    {
        if (IsWriting)
            Write(value);
        else
            value = (T)Read<T>();

        return value;
    }

    protected object Read<T>()
    {
        // Get the type
        var type = typeof(T);

        TypeCode typeCode = Type.GetTypeCode(type);

        switch (typeCode)
        {
            case TypeCode.Boolean:
                var bo = (byte)Mem.ReadByte(CurrentAddressValue);
                CurrentAddressValue += sizeof(byte);
                return bo == 1;

            case TypeCode.SByte:
                var sb = (sbyte)Mem.ReadS8(CurrentAddressValue);
                CurrentAddressValue += sizeof(sbyte);
                return sb;

            case TypeCode.Byte:
                var b = (byte)Mem.ReadByte(CurrentAddressValue);
                CurrentAddressValue += sizeof(byte);
                return b ;

            case TypeCode.Int16:
                var s = (short)Mem.ReadS16(CurrentAddressValue);
                CurrentAddressValue += sizeof(short);
                return s;

            case TypeCode.UInt16:
                var us = (ushort)Mem.ReadU16(CurrentAddressValue);
                CurrentAddressValue += sizeof(ushort);
                return us;

            case TypeCode.Int32:
                var i = (int)Mem.ReadS32(CurrentAddressValue);
                CurrentAddressValue += sizeof(int);
                return i;

            case TypeCode.UInt32:
                var ui = (uint)Mem.ReadU32(CurrentAddressValue);
                CurrentAddressValue += sizeof(uint);
                return ui;

            case TypeCode.Int64: 
            case TypeCode.UInt64:
            case TypeCode.Single:
            case TypeCode.Double:
            case TypeCode.String:
            case TypeCode.Decimal:
            case TypeCode.Char:
            case TypeCode.DateTime:
            case TypeCode.Empty:
            case TypeCode.DBNull:
            default:
                throw new NotSupportedException("The specified generic type can not be read from the reader");
        }
    }

    protected void Write(object value)
    {
        if (value is byte[] ba)
        {
            Mem.WriteByteRange(CurrentAddressValue, ba.ToList());
            CurrentAddressValue += ba.Length;
        }
        else if (value is Array a)
        {
            foreach (var item in a)
                Write(item);
        }
        else if (value.GetType().IsEnum)
        {
            var t = Enum.GetUnderlyingType(value.GetType());
            Write(Convert.ChangeType(value, t));
        }
        else if (value is bool bo)
        {
            Mem.WriteByte(CurrentAddressValue, (byte)(bo ? 1 : 0));
            CurrentAddressValue += sizeof(byte);
        }
        else if (value is sbyte sb)
        {
            Mem.WriteS8(CurrentAddressValue, sb);
            CurrentAddressValue += sizeof(sbyte);
        }
        else if (value is byte by)
        {
            Mem.WriteByte(CurrentAddressValue, by);
            CurrentAddressValue += sizeof(byte);
        }
        else if (value is short sh)
        {
            Mem.WriteS16(CurrentAddressValue, sh);
            CurrentAddressValue += sizeof(short);
        }
        else if (value is ushort ush)
        {
            Mem.WriteU16(CurrentAddressValue, ush);
            CurrentAddressValue += sizeof(ushort);
        }
        else if (value is int i32)
        {
            Mem.WriteS32(CurrentAddressValue, i32);
            CurrentAddressValue += sizeof(int);
        }
        else if (value is uint ui32)
        {
            Mem.WriteU32(CurrentAddressValue, ui32);
            CurrentAddressValue += sizeof(uint);
        }
        //else if (value is long lo)
        //{
        //    CurrentAddressValue += sizeof(long);
        //}
        //else if (value is ulong ulo)
        //{
        //    CurrentAddressValue += sizeof(ulong);
        //}
        //else if (value is float fl)
        //{
        //    CurrentAddressValue += sizeof(float);
        //}
        //else if (value is double dou)
        //{
        //    CurrentAddressValue += sizeof(double);
        //}
        //else if (value is string s)
        //{

        //}
        else
        {
            throw new NotSupportedException($"The specified type {value.GetType().Name} is not supported.");
        }
    }

    public override Pointer CurrentAddress
    {
        get => CurrentAddressValue;
        set => CurrentAddressValue = value.Address;
    }

    public IMemoryApi Mem { get; }
    public long CurrentAddressValue { get; set; } // Since we're not working with a stream we need to manually keep track of the current address
    public bool IsWriting { get; set; }
}