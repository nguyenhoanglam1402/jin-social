using System;
using UnityEngine;

public class LoadFileUtil
{
    public static T LoadFile<T>(string path)
    {
        if (path == null && path == "")
        {
            Debug.LogWarning("Not Found Path");
            return default;
        }

        TextAsset file = Resources.Load(path) as TextAsset;
        if (file)
        {
            if(typeof(T) == typeof(byte[]))
            {
                return (T)Convert.ChangeType(file.bytes.Length > 0 ? file.bytes : new byte[] { }, typeof(byte[]));
            }
            return (T)Convert.ChangeType(file.text ?? "", typeof(string));
        }

        Debug.LogError("No such file or directory");
        return (T)Convert.ChangeType("", typeof(string));
    }
}

