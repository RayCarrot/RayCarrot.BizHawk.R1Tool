using System;

namespace RayCarrot.BizHawk.R1Tool;

public abstract class BinarySerializer
{
    public abstract T Serialize<T>(T value, Action<T>? onPreSerializing = null, string? name = null);

    public virtual T SerializeObject<T>(T value, Action<T>? onPreSerializing = null, string? name = null)
        where T : IBinarySerializable, new()
    {
        // Create a new instance of the object if null
        if (value == null)
            value = new T();

        Depth++;

        // Call pre-serializing action
        onPreSerializing?.Invoke(value);

        // Serialize the object
        value.Serialize(this);

        Depth--;

        // Return the object
        return value;
    }

    public virtual T[] SerializeArray<T>(T[] array, int length, string name = null)
    {
        // Create the array if null or if the size doesn't match
        if (array == null || array.Length != length)
            array = new T[length];

        // Serialize each value
        for (int i = 0; i < length; i++)
            array[i] = Serialize(array[i], name: name == null ? null : $"{name}[{i}]");

        // Return the array
        return array;
    }
    public virtual T[] SerializeObjectArray<T>(T[] array, int length, Action<T>? onPreSerializing = null, string name = null)
        where T : IBinarySerializable, new()
    {
        // Create the array if null or if the size doesn't match
        if (array == null || array.Length != length)
            array = new T[length];

        // Serialize each value
        for (int i = 0; i < length; i++)
            array[i] = SerializeObject(array[i], onPreSerializing: onPreSerializing, name: name == null ? null : $"{name}[{i}]");

        // Return the array
        return array;
    }

    public abstract Pointer CurrentAddress { get; set; }
    protected int Depth { get; set; }

    public virtual T DoAt<T>(Pointer addr, Func<T> func)
    {
        if (addr.Address == 0)
            return default;

        var a = CurrentAddress;

        try
        {
            CurrentAddress = addr;
            return func();
        }
        finally
        {
            CurrentAddress = a;
        }
    }
}