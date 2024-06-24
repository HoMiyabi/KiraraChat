using UnityEngine;
using UnityEngine.UI;
using Models;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using DG.Tweening;

public class UIChat : UIPanel
{
    public Button emojiBtn;
    public Button sendBtn;
    public InputField messageInput;
    public ScrollRect scrollRect;
    public Text usernameText;

    public ClientUserInfo friendUserInfo;

    public CancellationTokenSource cts = new();

    public override void UIClose()
    {
        base.UIClose();
        cts.Cancel();
        cts.Dispose();
    }

    public async UniTask PollMessage(CancellationToken token)
    {
        while (true)
        {
            if (token.IsCancellationRequested)
            {
                break;
            }
            try
            {
                var (timeout, message) = await NetManager.PollMessage(friendUserInfo.id, token);

                if (timeout) continue;

                int selfID = LocalData.Instance.userInfo.id;

                UIMsgInfo.ESenderPlace senderPlace =
                    (message.senderID == selfID) ? UIMsgInfo.ESenderPlace.Right : UIMsgInfo.ESenderPlace.Left;
                ClientUserInfo senderUserInfo =
                    (message.senderID == selfID) ? LocalData.Instance.userInfo : friendUserInfo;

                switch (message.messageKind)
                {
                    case MessageKind.Text:
                    {
                        UIMsgInfo.Make(uiRoot, scrollRect.content, senderPlace, senderUserInfo, message.content);

                        scrollRect.DOKill();
                        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
                        scrollRect.DOVerticalNormalizedPos(0, 0.25f)
                            .SetEase(Ease.Linear);
                    } break;

                    case MessageKind.Emoji:
                    {
                        UIMsgInfo.Make(uiRoot, scrollRect.content, senderPlace, senderUserInfo,
                            StaticData.Instance.emojiName_Asset[message.content].sprite);

                        scrollRect.DOKill();
                        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
                        scrollRect.DOVerticalNormalizedPos(0, 0.25f)
                            .SetEase(Ease.Linear);
                    } break;

                    default:
                    {
                        Debug.LogWarning("未知的消息类型");
                    } break;
                }
            }
            catch (ApplicationException e)
            {
                Tools.LogToast(e.Message);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }

    public async UniTask UI_SendEmoji(string emojiName)
    {
        sendBtn.interactable = false;

        int friendID = friendUserInfo.id;
        try
        {
            await NetManager.SendEmoji(friendID, emojiName);

            scrollRect.verticalNormalizedPosition = 0;
            UIMsgInfo.Make(uiRoot, scrollRect.content, UIMsgInfo.ESenderPlace.Right, LocalData.Instance.userInfo,
                StaticData.Instance.emojiName_Asset[emojiName].sprite);

            scrollRect.DOKill();
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
            scrollRect.DOVerticalNormalizedPos(0, 0.25f).SetEase(Ease.Linear);
        }
        catch (ApplicationException e)
        {
            Tools.WarnToast(e.Message);
        }
        messageInput.ActivateInputField(); // 拿到焦点，连续输入

        sendBtn.interactable = true;
    }

    public async UniTask UI_SendText()
    {
        if (messageInput.text == "")
        {
            UIToast.Make("请输入内容").Show();
            return;
        }
        sendBtn.interactable = false;
        try
        {
            await NetManager.SendText(friendUserInfo.id, messageInput.text);

            // 成功后上屏
            scrollRect.verticalNormalizedPosition = 0;
            UIMsgInfo.Make(uiRoot, scrollRect.content, UIMsgInfo.ESenderPlace.Right, LocalData.Instance.userInfo, messageInput.text);

            messageInput.text = "";

            scrollRect.DOKill();
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);

            scrollRect.DOVerticalNormalizedPos(0, 0.3f).SetEase(Ease.Linear);
        }
        catch (ApplicationException e)
        {
            Tools.WarnToast(e.Message);
        }

        sendBtn.interactable = true;
        messageInput.ActivateInputField(); // 拿到焦点，连续输入
    }

    private async UniTask InitConversation()
    {
        scrollRect.content.DestroyAllChild();

        int friendID = friendUserInfo.id;
        int selfID = LocalData.Instance.userInfo.id;

        try
        {
            var messages = await NetManager.GetConversation(friendID);

            Debug.Log($"共有{messages.Count}条记录");
            foreach (Message message in messages)
            {
                //Debug.Log($"sender: {message.senderID}, receiver: {message.receiverID}, dateTime: {message.dateTime}, content: {message.content}");

                UIMsgInfo.ESenderPlace senderPlace =
                    (message.senderID == selfID) ? UIMsgInfo.ESenderPlace.Right : UIMsgInfo.ESenderPlace.Left;
                ClientUserInfo senderUserInfo =
                    (message.senderID == selfID) ? LocalData.Instance.userInfo : friendUserInfo;

                if (message.messageKind == MessageKind.Text)
                {
                    UIMsgInfo.Make(uiRoot, scrollRect.content, senderPlace, senderUserInfo, message.content);
                }
                else if (message.messageKind == MessageKind.Emoji)
                {
                    UIMsgInfo.Make(uiRoot, scrollRect.content, senderPlace, senderUserInfo,
                        StaticData.Instance.emojiName_Asset[message.content].sprite);
                }
                else
                {
                    Debug.LogWarning("错误的消息类型");
                }
            }

            // 移动到底部

            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
            scrollRect.verticalNormalizedPosition = 0;
        }
        catch (ApplicationException e)
        {
            Tools.WarnToast(e.Message);
        }
    }

    public async void Init(ClientUserInfo userInfo)
    {
        messageInput.ActivateInputField();

        messageInput.onSubmit.AddListener(async _ =>
        {
            await UI_SendText();
        });

        friendUserInfo = userInfo;

        usernameText.text = userInfo.username;

        emojiBtn.onClick.AddListener(() =>
        {
            var selectEmoji = UISelectEmoji.Make(emojiBtn.transform.position);
            selectEmoji.finishSelectCallback = async (emojiName) =>
            {
                await UI_SendEmoji(emojiName);
            };
        });

        sendBtn.onClick.AddListener(async () =>
        {
            await UI_SendText();
        });

        await InitConversation();

        _ = UniTask.Create(PollMessage, cts.Token);
    }

    public static UIChat Make(ClientUserInfo userInfo)
    {
        UIChat panel = UIManager.OpenPanel<UIChat>("Chat");
        panel.Init(userInfo);
        return panel;
    }
}