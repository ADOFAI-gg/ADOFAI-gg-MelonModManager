
using System;
using System.Collections.Generic;
using System.IO;
using MelonLoader;
using SimpleJSON;
using UnityEngine;
using MelonModManager.Core;

namespace MelonModManager {
    public class MAIN : MelonPlugin {
        /*

        private bool _isGUIOpen = false,
            _isSettingOpen = false;

        private Vector2 _scrollPosition = Vector2.zero,
            _scrollPosition2 = Vector2.zero;

        private List<MelonMod> SupportMods = new List<MelonMod>();
        private Dictionary<string, int> modToggle = new Dictionary<string, int>();
        private Dictionary<ModStatus, string> Status2Color = new Dictionary<ModStatus, string>
        {
            { ModStatus.Enabled, WindowColor.OnlineText },
            { ModStatus.Disabled, WindowColor.OfflineText },
            { ModStatus.Warning, WindowColor.WarningText },
            { ModStatus.Error, WindowColor.ErrorText }
        };

        private GUIStyle TranslucentBackground,
            ModInfoArea,
            ModListArea,
            _areaStyle3,
            InvisibleBackgroundText,
            ModToggleButton,
            OnSettingGUIButton,
            SettingTitleText,
            RoundBackgroundText;

        private Texture2D
            ModInfoTexture,
            ModListTexture,
            TranslucentTexture,
            InvisibleTexture,
            ButtonTexture,
            RoundTexture;

        private MelonMod SelectedMod;
        private Rect _mWindowRect = new Rect(0, 0, 0, 0);


        public override void OnApplicationStart() {

        }

        public override void OnApplicationLateStart() {

            //OnToggle 있는 모드들만 선택
            foreach (var mod in MelonHandler.Mods) {
                var OnToggle = mod.Info.SystemType.GetMethod("OnToggle");

                if (OnToggle != null) {
                    SupportMods.Add(mod);

                    try {
                        //OnToggle 있는 모드들 실행
                        if (!modToggle.ContainsKey(mod.Info.Name)) modToggle[mod.Info.Name] = ModStatus.Enabled;
                        if (modToggle[mod.Info.Name] == ModStatus.Enabled) OnToggle.Invoke(mod, new object[] { true });
                    }
                    catch (Exception e) {
                        MelonLogger.Error(e);
                        modToggle[mod.Info.Name] = ModStatus.Error;
                    }

                }
            }
        }

        public override void OnUpdate() {

            if (Input.GetKeyDown(KeyCode.F10) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))) {
                _isGUIOpen = !_isGUIOpen;
                SelectedMod = null;
            }

            if (Input.GetKeyDown(KeyCode.Escape) && _isGUIOpen) {
                _isGUIOpen = false;
                SelectedMod = null;
            }
        }

        public override void OnGUI() {

            if (!_isGUIOpen) return;

            //스타일 설정
            if (TranslucentBackground == null) {
                ModInfoTexture = CanvasDrawer.MakeRoundRectangle(4, 80, 80, 1, 15, WindowColor.ModInfoLayout);
                ModListTexture = CanvasDrawer.MakeRoundRectangle(4, 80, 80, 1, 15, WindowColor.ModListLayout);
                TranslucentTexture = CanvasDrawer.MakeTexture(1, 1, WindowColor.Translucent);
                InvisibleTexture = CanvasDrawer.MakeTexture(1, 1, WindowColor.Invisible);
                ButtonTexture = CanvasDrawer.MakeRoundRectangle(4, 80, 80, 1, 10, WindowColor.ButtonColor);
                RoundTexture = CanvasDrawer.MakeRoundRectangle(4, 80, 80, 1, 20, WindowColor.RoundLayout);

                TranslucentBackground = new GUIStyle(GUI.skin.box);
                TranslucentBackground.normal.textColor = Color.white;
                TranslucentBackground.fontSize = 40;
                TranslucentBackground.font = RDString.GetFontDataForLanguage(RDString.language).font;
                TranslucentBackground.normal.background = TranslucentTexture;

                ModInfoArea = new GUIStyle();
                ModInfoArea.normal.background = ModInfoTexture;
                ModInfoArea.border = new RectOffset(15, 15, 15, 15);

                ModListArea = new GUIStyle();
                ModListArea.normal.background = ModListTexture;
                ModListArea.border = new RectOffset(15, 15, 15, 15);

                InvisibleBackgroundText = new GUIStyle();
                InvisibleBackgroundText.normal.textColor = Color.white;
                InvisibleBackgroundText.normal.background = InvisibleTexture;
                InvisibleBackgroundText.fontSize = 30;
                InvisibleBackgroundText.font = RDString.GetFontDataForLanguage(RDString.language).font;
                InvisibleBackgroundText.alignment = TextAnchor.UpperCenter;
                InvisibleBackgroundText.wordWrap = true;

                ModToggleButton = new GUIStyle(GUI.skin.button);
                ModToggleButton.normal.textColor = Color.black;
                ModToggleButton.normal.background = ButtonTexture;
                ModToggleButton.active.background = ButtonTexture;
                ModToggleButton.focused.background = ButtonTexture;
                ModToggleButton.hover.background = ButtonTexture;
                ModToggleButton.font = RDString.GetFontDataForLanguage(RDString.language).font;
                ModToggleButton.alignment = TextAnchor.MiddleCenter;
                ModToggleButton.margin.left = 20;
                ModToggleButton.margin.right = 20;
                ModToggleButton.padding.left = 10;
                ModToggleButton.padding.right = 10;
                ModToggleButton.fontSize = 30;
                ModToggleButton.border = new RectOffset(10, 10, 10, 10);

                OnSettingGUIButton = new GUIStyle(GUI.skin.label);
                OnSettingGUIButton.alignment = TextAnchor.UpperCenter;
                OnSettingGUIButton.margin.top = -13;

                SettingTitleText = new GUIStyle();
                SettingTitleText.normal.textColor = Color.white;
                SettingTitleText.wordWrap = true;
                SettingTitleText.fontSize = 30;
                SettingTitleText.font = RDString.GetFontDataForLanguage(RDString.language).font;
                SettingTitleText.alignment = TextAnchor.UpperLeft;
                SettingTitleText.padding.left = 10;
                SettingTitleText.padding.right = 10;
                SettingTitleText.padding.bottom = 5;
                SettingTitleText.margin.top = -20;
                SettingTitleText.margin.bottom = 20;

                RoundBackgroundText = new GUIStyle(SettingTitleText);
                RoundBackgroundText.normal.background = RoundTexture;
                RoundBackgroundText.margin.top = 0;
                RoundBackgroundText.margin.right = 20;
                RoundBackgroundText.border = new RectOffset(20, 20, 20, 20);
            }

            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "MelonLoader Manager", TranslucentBackground); //전체 반투명
            GUI.Box(_mWindowRect, "", ModInfoArea); // 창

            float f = _mWindowRect.width / 7;
            float t = _mWindowRect.width - f;

            GUI.Box(new Rect(_mWindowRect.x, _mWindowRect.y, _mWindowRect.width, 30), "", ModListArea); //창에서 가장 왼쪽부분을 모드 리스트
            GUI.Box(new Rect(_mWindowRect.x, _mWindowRect.y, f, _mWindowRect.height), "", ModListArea); //창에서 가장 윗부분 꾸미기



            //모드 리스트 부분에 모드 불러와서 텍스트 추가
            GUILayout.BeginArea(new Rect(_mWindowRect.x, _mWindowRect.y, f, _mWindowRect.height), "", ModListArea);
            _scrollPosition = GUILayout.BeginScrollView(
                _scrollPosition, GUILayout.Width(f), GUILayout.Height(_mWindowRect.height - 60));
            GUILayout.Space(10);
            GUILayout.Label("Mod List", InvisibleBackgroundText);

            foreach (var mod in SupportMods) {

                //모드 선택
                if (GUILayout.Button($"<color={(SelectedMod == mod ? WindowColor.SelectText : WindowColor.NotSelectText)}>{mod.Info.Name}</color>",
                    InvisibleBackgroundText, GUILayout.Height(50))) {
                    SelectedMod = mod;
                    if (!modToggle.ContainsKey(SelectedMod.Info.Name)) modToggle[SelectedMod.Info.Name] = ModStatus.Enabled;
                }

            }
            GUILayout.EndScrollView();

            //모드 토글 버튼
            if (SelectedMod != null && (modToggle[SelectedMod.Info.Name] == ModStatus.Enabled || modToggle[SelectedMod.Info.Name] == ModStatus.Disabled)) {

                if (!modToggle.ContainsKey(SelectedMod.Info.Name)) modToggle[SelectedMod.Info.Name] = ModStatus.Enabled;

                if (GUILayout.Button($"{(modToggle[SelectedMod.Info.Name] == ModStatus.Enabled ? "Disable" : "Enable")}", ModToggleButton, GUILayout.Height(40))) {
                    modToggle[SelectedMod.Info.Name] = modToggle[SelectedMod.Info.Name] == ModStatus.Enabled
                        ? ModStatus.Disabled
                        : ModStatus.Enabled;
                    SelectedMod.Info.SystemType.GetMethod("OnToggle").Invoke(null, new object[] { modToggle[SelectedMod.Info.Name] == ModStatus.Enabled });
                }

            }
            GUILayout.EndArea();

            //모드가 선택되면 모드 설명 띄움
            if (SelectedMod == null) return;
            string info = "";

            GUILayout.BeginArea(new Rect(_mWindowRect.x + f + 40, _mWindowRect.y + 20, t - 80, _mWindowRect.height - 40), "");
            _scrollPosition2 = GUILayout.BeginScrollView(
                _scrollPosition2, GUILayout.Width(t - 80), GUILayout.Height(_mWindowRect.height - 40));

            GUILayout.Label($"<size=40><color={Status2Color[modToggle[SelectedMod.Info.Name]]}>●</color></size> <size=50>{SelectedMod.Info.Name}</size>", InvisibleBackgroundText);

            info += $"Author - {SelectedMod.Info.Author}\n";
            info += $"Version - v{SelectedMod.Info.Version}\n";

            var description = SelectedMod.Info.SystemType.GetField("Description");
            var OnSettingGUI = SelectedMod.Info.SystemType.GetMethod("OnSettingGUI");

            if (SelectedMod.Info.DownloadLink != null) info += $"Link - {SelectedMod.Info.DownloadLink}\n";
            if (description != null) info += $"Description - {description.GetValue(SelectedMod)}\n";

            GUILayout.Label(info.Trim(), RoundBackgroundText);

            //OnSettingGUI
            if (OnSettingGUI != null) {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button($"{(_isSettingOpen ? "<size=40>◢</size>" : "<size=30>▶</size>")}", OnSettingGUIButton))
                    _isSettingOpen = !_isSettingOpen;

                GUILayout.Label("<size=40>Setting</size>", SettingTitleText);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                if (_isSettingOpen) {
                    GUILayout.BeginVertical("", RoundBackgroundText);
                    OnSettingGUI.Invoke(SelectedMod, null);
                    GUILayout.EndVertical();
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();



        }

        public override void OnApplicationQuit() {

        }*/
    }
}
