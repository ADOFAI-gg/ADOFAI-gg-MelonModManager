using System;
using System.Collections.Generic;
using DG.Tweening;
using HarmonyLib;
using MelonLoader;
using MelonModManager.Util;
using UnityEngine;
using UnityEngine.UI;

namespace MelonModManager.Core.View
{
    public class ModTab : MonoBehaviour
    {
        public Text Name;
        public Image Fill;
        public Image Border;
        public GameObject Value;
        public Button LeftButton;
        public Button RightButton;
        public Button Tab;
        public Text ValueName;
        public RectTransform ButtonContainer;
        private bool _notML = false;
        
        
        private void Awake()
        {
            Name = transform.Find("SettingName").GetComponent<Text>();
            Border = transform.Find("Border").GetComponent<Image>();
            Fill = transform.Find("Border").Find("Fill").GetComponent<Image>();
            Value = transform.Find("Value").gameObject;
            LeftButton = Value.transform.Find("LeftArrow").GetComponent<Button>();
            RightButton = Value.transform.Find("RightArrow").GetComponent<Button>();
            Tab = transform.GetComponent<Button>();
            ValueName = Value.GetComponent<Text>();
            ButtonContainer = gameObject.GetComponent<RectTransform>();

            Tab.onClick.AddListener(() =>
            {
                var modWindow = ModWindow.Instance;
                if (modWindow.SelectMod != null) 
                    modWindow.SelectMod.SetFocus(false);
                SetFocus(true);
                modWindow.SelectMod = this;
                modWindow.ModInfo.gameObject.SetActive(true);
                var mod = MelonHandler.Mods.Find(m => m.Info.Name.Equals(modWindow.SelectMod.Name.text.Replace("<color=#ff0000>!! </color>","")));
                modWindow.ModInfo.Title.text = mod.Info.Name;

                modWindow.ModInfo.Details.text =
                    $"(v{mod.Info.Version.Replace("v", "")}) {mod.Info.Author}";
                /*modWindow.ModInfo.Description.text =
                    !_notML
                        ? $"{(mod as MelonLoaderMod)?.Description}"
                        : $"{mod.Info.Name} is not compatible with MelonModManager.";*/
                modWindow.ModInfo.Description.text =
                    "lkjsdlkjs\ndglokjsdg\nljksdgjksdgl\njksdglkjsdglkj\nsdglkjsdglkj\nsdglkjsdgl\nkjsdglkj\nsdglkjsdg\nlkjsdglkjsdgl\nkjsdgl\nkjsdg\nlkj;dfbkljdfgkj;dfg\npojsdgpojsdgs\nsopidfgpijodg\nsdf";

            });
            
            LeftButton.onClick.AddListener(() =>
            {
                MelonModManager.ModToggle[name] = (MelonModManager.ModToggle[name] == ModStatus.Enabled
                    ? ModStatus.Disabled
                    : ModStatus.Enabled);
                ValueName.text = MelonModManager.ModToggle[name] == ModStatus.Enabled ? "On" : "Off";
                var mod = MelonHandler.Mods.Find(m => m.Info.Name.Equals(ModWindow.Instance.SelectMod.Name.text.Replace("<color=#ff0000>!! </color>","")));
                (mod as MelonLoaderMod)?.OnToggle(MelonModManager.ModToggle[name] == ModStatus.Enabled);
                PlayArrowAnimation(false);
            });
            
            RightButton.onClick.AddListener(() =>
            {
                MelonModManager.ModToggle[name] = (MelonModManager.ModToggle[name] == ModStatus.Enabled
                    ? ModStatus.Disabled
                    : ModStatus.Enabled);
                ValueName.text = MelonModManager.ModToggle[name] == ModStatus.Enabled ? "On" : "Off";
                var mod = MelonHandler.Mods.Find(m => m.Info.Name.Equals(ModWindow.Instance.SelectMod.Name.text.Replace("<color=#ff0000>!! </color>","")));
                (mod as MelonLoaderMod)?.OnToggle(MelonModManager.ModToggle[name] == ModStatus.Enabled);
                PlayArrowAnimation(true);
            });
            
            SetFocus(false);
        }

        public void NotMelonLoaderMod()
        {
            RightButton.gameObject.SetActive(false);
            LeftButton.gameObject.SetActive(false);
            Value.gameObject.SetActive(false);
            _notML = true;
        }
        public void SetFocus(bool focus)
        {

            Name.color =
                (focus ? ModWindowStyles.Colors.selectedLabelColor : ModWindowStyles.Colors.unselectedLabelColor);
            ValueName.color = Name.color;
            Border.color = (focus
                ? ModWindowStyles.Colors.selectedLabelColor
                : ModWindowStyles.Colors.unselectedBorderColor);
            Fill.color =
                (focus ? ModWindowStyles.Colors.selectedFillColor : ModWindowStyles.Colors.unselectedFillColor);
            RightButton.gameObject.SetActive(focus);
            LeftButton.gameObject.SetActive(focus);
            
            if (focus)
            {
                ModWindow.Instance.Audio.Play();
                ButtonContainer.DOKill(true);
                ButtonContainer.DOScale(1.02f, 0.1f).SetEase(Ease.InOutQuad).SetUpdate(true).OnComplete(delegate
                {
                    ButtonContainer.DOScale(1f, 0.1f).SetEase(Ease.InOutQuad).SetUpdate(true);
                });
            }
        }
        
        public void PlayArrowAnimation(bool isRight)
        {
            ModWindow.Instance.Audio.Play();
            RectTransform componentInChildren = (isRight ? RightButton : LeftButton).GetComponentInChildren<RectTransform>();
            componentInChildren.DOComplete(false);
            componentInChildren.DOAnchorPosX(componentInChildren.anchoredPosition.x + 5f * 0.25f * (float)(isRight ? 1 : -1), 0.17f, false).SetEase(Ease.InOutQuad).SetUpdate(true).SetLoops(2, LoopType.Yoyo);
        }
    }
}