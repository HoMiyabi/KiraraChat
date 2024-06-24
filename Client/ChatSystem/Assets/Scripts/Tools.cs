using UnityEngine;

public static class Tools
{
    public static void DestroyAllChild(this Transform transform)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Object.Destroy(transform.GetChild(i).gameObject);
        }
    }

    public static void WarnToast(string message)
    {
        Debug.LogWarning(message);
        UIToast.Make(message).Show();
    }

    public static void LogToast(string message)
    {
        Debug.Log(message);
        UIToast.Make(message).Show();
    }
}