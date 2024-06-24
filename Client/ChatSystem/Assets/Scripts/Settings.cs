using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoSingleton<Settings>
{
    public string serverIP;
    public int mainPort;
    public string MainIPPort { get => $"http://{serverIP}:{mainPort}"; }

    public List<Vector2Int> availableResolution;
    public int resolutionIndex = 0;
}