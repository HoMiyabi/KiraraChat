using System;
using System.Text;
using UnityEngine;

//public class Protocol
//{
//    //public static byte[] Pack<TKind>(Body.Kind kind, TKind body)
//    //{
//    //    string bodyString = JsonUtility.ToJson(body);

//    //    byte[] bodyBytes = Encoding.UTF8.GetBytes(bodyString);
//    //    byte[] kindBytes = BitConverter.GetBytes((int)kind);
//    //    byte[] lengthBytes = BitConverter.GetBytes(4 + bodyBytes.Length);
//    //    byte[] result = new byte[8 + bodyBytes.Length];
//    //    Buffer.BlockCopy(lengthBytes, 0, result, 0, 4);
//    //    Buffer.BlockCopy(kindBytes, 0, result, 4, 4);
//    //    Buffer.BlockCopy(bodyBytes, 0, result, 8, bodyBytes.Length);
//    //    return result;
//    //}

//    //public static Body.Kind GetKind(byte[] kindBodyBytes)
//    //{
//    //    Body.Kind kind = (Body.Kind)BitConverter.ToInt32(kindBodyBytes);
//    //    return kind;
//    //}

//    //public static TBody GetBody<TBody>(byte[] kindBodyBytes)
//    //{
//    //    string data = Encoding.UTF8.GetString(kindBodyBytes, 4, kindBodyBytes.Length - 4);
//    //    TBody body = JsonUtility.FromJson<TBody>(data);
//    //    return body;
//    //}
//}