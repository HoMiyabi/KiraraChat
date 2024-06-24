using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class SpriteAsset
{
    public string internalName;
    public string externalName;
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "SOSpriteAssets", menuName = "SO Sprite Assets")]
public class SOSpriteAssets : ScriptableObject
{
    public List<SpriteAsset> spriteAssets;
}