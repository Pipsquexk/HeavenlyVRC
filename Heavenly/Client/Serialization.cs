using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

internal static class Serialization
{

    public static Il2CppSystem.Object ToIl2CppObject(this object ob)
    {
        return new Il2CppSystem.Object(Marshal.AllocHGlobal(Marshal.SizeOf(ob)));
    }

    //public static object ToObject(this Il2CppSystem.Object obj)
    //{
    //    GCHandle h = GCHandle.Alloc(obj, GCHandleType.Normal);
    //    var result = (object)h;
    //    h.Free();
    //    return result;
    //}

    public static byte[] ToByteArray(Il2CppSystem.Object obj)
    {
        byte[] result;
        if (obj == null)
        {
            result = null;
        }
        else
        {
            Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Il2CppSystem.IO.MemoryStream memoryStream = new Il2CppSystem.IO.MemoryStream();
            binaryFormatter.Serialize(memoryStream, obj);
            result = memoryStream.ToArray();
        }
        return result;
    }

    public static byte[] ToByteArray(object obj)
    {
        bool flag = obj == null;
        byte[] result;
        if (flag)
        {
            result = null;
        }
        else
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            binaryFormatter.Serialize(memoryStream, obj);
            result = memoryStream.ToArray();
        }
        return result;
    }

    public static T ILFromByteArray<T>(byte[] data)
    {
        bool flag = data == null;
        T result;
        if (flag)
        {
            result = default(T);
        }
        else
        {
            Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Il2CppSystem.IO.MemoryStream memoryStream = new Il2CppSystem.IO.MemoryStream(data);
            object obj = binaryFormatter.Deserialize(memoryStream);
            result = (T)((object)obj);
        }
        return result;
    }

    public static T FromByteArray<T>(byte[] data)
    {
        T result;
        if (data == null)
        {
            result = default(T);
        }
        else
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(data))
            {
                object obj = binaryFormatter.Deserialize(memoryStream);
                result = (T)((object)obj);
            }
        }
        return result;
    }

    public static T FromIL2CPPToManaged<T>(Il2CppSystem.Object obj)
    {
        return Serialization.FromByteArray<T>(Serialization.ToByteArray(obj));
    }

    public static Dictionary<K, V> HashtableToDictionary<K, V>(Hashtable table)
    {
        return table
          .Cast<DictionaryEntry>()
          .ToDictionary(kvp => (K)kvp.Key, kvp => (V)kvp.Value);
    }

    public static Il2CppSystem.Object FromManagedToIL2CPP(object obj)
    {
        return Serialization.ILFromByteArray<Il2CppSystem.Object>(Serialization.ToByteArray(obj));
    }
}
