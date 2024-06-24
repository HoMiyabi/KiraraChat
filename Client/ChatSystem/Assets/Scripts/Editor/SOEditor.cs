using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class SOEditor : EditorWindow
{
    [MenuItem("扩展/SO编辑器")]
    static void ShowWindow()
    {
        GetWindow<SOEditor>("SO编辑器");
    }

    public SOSpriteAssets so;

    private ReorderableList list;

    public void OnGUI()
    {
        so = EditorGUILayout.ObjectField("SO文件", so, typeof(SOSpriteAssets), false) as SOSpriteAssets;

        if (GUILayout.Button("更新"))
        {
            if (so != null)
            {
                list = new ReorderableList(so.spriteAssets, so.spriteAssets.GetType(),
                    true, true, true, true);

                Type type = typeof(SpriteAsset);
                StringBuilder builder = new();
                foreach (FieldInfo info in type.GetFields())
                {
                    builder.Append(" " + info.Name);
                }
                string header = builder.ToString();

                list.drawHeaderCallback = (rect) =>
                {
                    GUI.Label(rect, header);
                };

                list.drawElementCallback = (rect, index, active, focused) =>
                {
                    SpriteAsset spriteAsset = so.spriteAssets[index];
                    rect.width = 100;
                    spriteAsset.internalName = EditorGUI.TextField(rect, spriteAsset.internalName);
                    rect.x += 100;
                    spriteAsset.externalName = EditorGUI.TextField(rect, spriteAsset.externalName);
                    rect.x += 100;
                    spriteAsset.sprite =
                        EditorGUI.ObjectField(rect, spriteAsset.sprite, typeof(Sprite), false) as Sprite;
                };
            }
        }


        if (so != null && list != null)
        {
            list.DoLayoutList();
            // Debug.Log(so.spriteAssets.Count);
        }
    }
}