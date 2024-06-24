using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class StaticData : MonoSingleton<StaticData>
{
    public SOSpriteAssets soEmojiAssets;
    public SOSpriteAssets soProfilePhotoAssets;

    public Dictionary<string, SpriteAsset> profilePhotoName_Asset = new();

    public Dictionary<string, SpriteAsset> emojiName_Asset = new();

    protected override void Awake()
    {
        base.Awake();
        foreach (SpriteAsset spriteAsset in soProfilePhotoAssets.spriteAssets)
        {
            profilePhotoName_Asset.Add(spriteAsset.internalName, spriteAsset);
        }

        foreach (SpriteAsset emojiAsset in soEmojiAssets.spriteAssets)
        {
            emojiName_Asset.Add(emojiAsset.internalName, emojiAsset);
        }
    }
}