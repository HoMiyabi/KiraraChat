using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIMsgInfo : UIPanel
{
    public HorizontalLayoutGroup totalHorizontalLayoutGroup;
    public VerticalLayoutGroup nameAndContentVerticalLayoutGroup;

    public Image messageBg;
    public Text usernameText;
    public Text messageText;
    public Image profilePhotoImg;

    public enum ESenderPlace { Left, Right }

    public ESenderPlace _senderPlace;

    public int messagePadding = 100;

    public Transform textContent;
    public Transform emojiContent;

    public Image messageEmoji;
    public ESenderPlace SenderPlace
    {
        get => _senderPlace;
        set
        {
            _senderPlace = value;

            totalHorizontalLayoutGroup.reverseArrangement = _senderPlace == ESenderPlace.Right;

            if (_senderPlace == ESenderPlace.Left)
            {
                messageBg.color = leftMessageBgColor;
                messageText.color = leftMessageTextColor;

                totalHorizontalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
                nameAndContentVerticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;

                totalHorizontalLayoutGroup.padding.left = 0;
                totalHorizontalLayoutGroup.padding.right = messagePadding;
            }
            else
            {
                messageBg.color = rightMessageBgColor;
                messageText.color = rightMessageTextColor;

                totalHorizontalLayoutGroup.childAlignment = TextAnchor.UpperRight;
                nameAndContentVerticalLayoutGroup.childAlignment = TextAnchor.UpperRight;

                totalHorizontalLayoutGroup.padding.left = messagePadding;
                totalHorizontalLayoutGroup.padding.right = 0;
            }
        }
    }

    [Header("Left")]
    public Color leftMessageBgColor = Color.white;
    public Color leftMessageTextColor = Color.white;

    [Header("Right")]
    public Color rightMessageBgColor = Color.white;
    public Color rightMessageTextColor = Color.white;

    public UIMsgInfo Init(ESenderPlace senderPlace, ClientUserInfo userInfo, string text)
    {
        textContent.gameObject.SetActive(true);
        emojiContent.gameObject.SetActive(false);

        SenderPlace = senderPlace;

        usernameText.text = userInfo.username;
        messageText.text = text;

        profilePhotoImg.sprite = StaticData.Instance.profilePhotoName_Asset[userInfo.profilePhotoName].sprite;

        return this;
    }

    public static UIMsgInfo Make(UIPanel uiRoot, Transform uiParent, ESenderPlace senderPlace, ClientUserInfo userInfo, string text)
    {
        UIMsgInfo uiMsgInfo = UIManager.OpenPanel<UIMsgInfo>("MsgInfo", uiRoot, uiParent);
        uiMsgInfo.Init(senderPlace, userInfo, text);
        return uiMsgInfo;
    }

    public UIMsgInfo Init(ESenderPlace senderPlace, ClientUserInfo userInfo, Sprite emoji)
    {
        textContent.gameObject.SetActive(false);
        emojiContent.gameObject.SetActive(true);

        SenderPlace = senderPlace;

        usernameText.text = userInfo.username;
        messageEmoji.sprite = emoji;

        profilePhotoImg.sprite = StaticData.Instance.profilePhotoName_Asset[userInfo.profilePhotoName].sprite;

        return this;
    }

    public static UIMsgInfo Make(UIPanel uiRoot, Transform uiParent, ESenderPlace senderPlace, ClientUserInfo userInfo, Sprite emoji)
    {
        UIMsgInfo uiMsgInfo = UIManager.OpenPanel<UIMsgInfo>("MsgInfo", uiRoot, uiParent);
        uiMsgInfo.Init(senderPlace, userInfo, emoji);
        return uiMsgInfo;
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        SenderPlace = _senderPlace;
    }
    #endif
}
