using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

// public static class RPC
// {
//     public static async UniTask<T> CallAsync<T>(string methodName, object parameter, CancellationToken cancellationToken = default)
//     {
//         HttpResponseMessage response = await NetManager.Instance.httpClient.PostAsync
//             (
//                 Settings.Instance.MainIPPort + "/" + methodName,
//                 new StringContent(
//                     JsonUtility.ToJson(parameter),
//                     Encoding.UTF8,
//                     "application/json"
//                     ),
//                 cancellationToken
//             );
//         string responseBody = await response.Content.ReadAsStringAsync();
//         T result = JsonUtility.FromJson<T>(responseBody);
//         return result;
//     }
// }