﻿using System.Text.Json;

namespace CafeExtensions.Extensions;
public static class ObjectSerializationExtension
{
    public static byte[] SerializeToByteArray(this object obj)
    {
        return JsonSerializer.SerializeToUtf8Bytes(obj);
    }

    public static T? Deserialize<T>(byte[] data) where T : class
    {
        using (var stream = new MemoryStream(data))
            return JsonSerializer.Deserialize(stream, typeof(T)) as T;
    }

    public static async Task<T?> DeserializeAsync<T>(byte[] data) where T : class
    {
        using (var stream = new MemoryStream(data))
        {
            var obj = await JsonSerializer.DeserializeAsync<T>(stream);
            return obj;
        }
    }
}
