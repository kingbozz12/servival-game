#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using SurvivalGame.Player;
using SurvivalGame.UI;

namespace SurvivalGame.EditorTools
{
    public static class ProductionHudBuilder
    {
        private const string UiFolder = "Assets/Generated/UI";

        private static Sprite roundedSprite;
        private static Sprite circleSprite;
        private static Sprite ringSprite;

        private static readonly Color Panel = new Color(0.025f, 0.035f, 0.040f, 0.88f);
        private static readonly Color PanelSoft = new Color(0.035f, 0.050f, 0.055f, 0.78f);
        private static readonly Color Border = new Color(0.36f, 0.43f, 0.44f, 0.52f);
        private static readonly Color Cyan = new Color(0.10f, 0.74f, 0.86f, 1f);
        private static readonly Color CyanDim = new Color(0.08f, 0.36f, 0.42f, 1f);
        private static readonly Color Red = new Color(0.90f, 0.16f, 0.17f, 1f);
        private static readonly Color Armor = new Color(0.12f, 0.60f, 0.86f, 1f);
        private static readonly Color Warm = new Color(0.93f, 0.62f, 0.12f, 1f);
        private static readonly Color TextPrimary = new Color(0.96f, 0.98f, 0.98f, 1f);
        private static readonly Color TextMuted = new Color(0.68f, 0.74f, 0.75f, 1f);

        public static void Build(GameObject player)
        {
            EnsureUiArt();

            var previous = GameObject.Find("Gameplay HUD");
            if (previous)
                Object.DestroyImmediate(previous);

            var canvas = CreateCanvas();
            var safe = CreateRect("Safe Area", canvas.transform, Vector2.zero, Vector2.one);
            safe.gameObject.AddComponent<SafeAreaFitter>();

            BuildPlayerPanel(safe, player);
            BuildQuestPanel(safe);
            BuildTopMenu(safe);
            BuildMinimap(safe, player);
            BuildJoystick(safe, player);
            BuildActionCluster(safe);
            BuildBottomUtilityButtons(safe);
            BuildExpBar(safe, player);
        }

        private static Canvas CreateCanvas()
        {
            var go = new GameObject("Gameplay HUD");
            var canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 50;

            var scaler = go.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            go.AddComponent<GraphicRaycaster>();

            if (!Object.FindFirstObjectByType<EventSystem>())
            {
                var es = new GameObject("EventSystem");
                es.AddComponent<EventSystem>();
                es.AddComponent<InputSystemUIInputModule>();
            }

            return canvas;
        }

        private static void BuildPlayerPanel(RectTransform root, GameObject player)
        {
            var panel = CreateFramedPanel(root, "Player Status",
                new Vector2(0.016f, 0.835f), new Vector2(0.315f, 0.985f), Panel);

            var portraitFrame = CreateCircle(panel, "Portrait Frame",
                new Vector2(0.018f, 0.10f), new Vector2(0.185f, 0.94f),
                new Color(0.73f, 0.79f, 0.80f, 0.85f));
            var portrait = CreateCircle(portraitFrame, "Portrait",
                new Vector2(0.055f, 0.055f), new Vector2(0.945f, 0.945f),
                new Color(0.10f, 0.13f, 0.14f, 1f));
            CreateText(portrait, "Portrait Mark", "P",
                new Vector2(0f, 0f), new Vector2(1f, 1f), 40, TextAnchor.MiddleCenter, TextMuted);

            var nickname = CreateText(panel, "Nickname", "Player5776",
                new Vector2(0.205f, 0.70f), new Vector2(0.60f, 0.96f), 27, TextAnchor.MiddleLeft, TextPrimary);

            var levelBadge = CreatePanel(panel, "Level Badge",
                new Vector2(0.73f, 0.70f), new Vector2(0.82f, 0.96f),
                new Color(0.08f, 0.10f, 0.11f, 0.98f), true);
            AddOutline(levelBadge, Warm, 2f);
            var level = CreateText(levelBadge, "Level", "1",
                Vector2.zero, Vector2.one, 22, TextAnchor.MiddleCenter, TextPrimary);

            CreateStatGlyph(panel, "Health Icon", StatGlyph.Heart,
                new Vector2(0.205f, 0.48f), new Vector2(0.245f, 0.67f), Red);
            var healthBar = CreateBar(panel, "Health",
                new Vector2(0.255f, 0.50f), new Vector2(0.675f, 0.64f), Red);
            var healthText = CreateText(panel, "Health Value", "100 / 100",
                new Vector2(0.69f, 0.45f), new Vector2(0.96f, 0.67f), 18, TextAnchor.MiddleLeft, TextPrimary);

            CreateStatGlyph(panel, "Armor Icon", StatGlyph.Shield,
                new Vector2(0.205f, 0.27f), new Vector2(0.245f, 0.45f), Armor);
            var armorBar = CreateBar(panel, "Armor",
                new Vector2(0.255f, 0.29f), new Vector2(0.675f, 0.42f), Armor);
            var armorText = CreateText(panel, "Armor Value", "0 / 100",
                new Vector2(0.69f, 0.24f), new Vector2(0.96f, 0.46f), 18, TextAnchor.MiddleLeft, TextPrimary);

            var foodChip = CreatePanel(panel, "Food Chip",
                new Vector2(0.205f, 0.035f), new Vector2(0.43f, 0.23f), PanelSoft, true);
            CreateStatGlyph(foodChip, "Food Icon", StatGlyph.Food,
                new Vector2(0.08f, 0.18f), new Vector2(0.30f, 0.82f), Warm);
            var foodText = CreateText(foodChip, "Food Value", "100",
                new Vector2(0.35f, 0f), new Vector2(0.96f, 1f), 19, TextAnchor.MiddleLeft, TextPrimary);

            var waterChip = CreatePanel(panel, "Water Chip",
                new Vector2(0.45f, 0.035f), new Vector2(0.675f, 0.23f), PanelSoft, true);
            CreateStatGlyph(waterChip, "Water Icon", StatGlyph.Drop,
                new Vector2(0.08f, 0.18f), new Vector2(0.30f, 0.82f), Cyan);
            var waterText = CreateText(waterChip, "Water Value", "100",
                new Vector2(0.35f, 0f), new Vector2(0.96f, 1f), 19, TextAnchor.MiddleLeft, TextPrimary);

            var presenter = panel.gameObject.AddComponent<PlayerHudPresenter>();
            BindObject(presenter, "vitals", player.GetComponent<PlayerVitals>());
            BindObject(presenter, "progression", player.GetComponent<PlayerProgression>());
            BindObject(presenter, "nicknameText", nickname);
            BindObject(presenter, "levelText", level);
            BindObject(presenter, "healthBar", healthBar);
            BindObject(presenter, "healthText", healthText);
            BindObject(presenter, "armorBar", armorBar);
            BindObject(presenter, "armorText", armorText);
            BindObject(presenter, "foodText", foodText);
            BindObject(presenter, "waterText", waterText);
        }

        private static void BuildQuestPanel(RectTransform root)
        {
            var panel = CreateFramedPanel(root, "Quest Tracker",
                new Vector2(0.016f, 0.565f), new Vector2(0.245f, 0.820f), Panel);

            var tabs = CreatePanel(panel, "Quest Tabs",
                new Vector2(0.018f, 0.80f), new Vector2(0.982f, 0.975f),
                new Color(0.028f, 0.040f, 0.044f, 0.95f), true);

            var active = CreatePanel(tabs, "Active Tab",
                new Vector2(0.0f, 0.0f), new Vector2(0.33f, 1f),
                new Color(0.055f, 0.34f, 0.41f, 0.95f), true);
            CreateText(active, "Label", "ЗАДАНИЯ", Vector2.zero, Vector2.one, 16,
                TextAnchor.MiddleCenter, TextPrimary);
            CreateText(tabs, "Tab 2", "ФРАКЦИИ",
                new Vector2(0.34f, 0f), new Vector2(0.66f, 1f), 15, TextAnchor.MiddleCenter, TextMuted);
            CreateText(tabs, "Tab 3", "СОБЫТИЯ",
                new Vector2(0.67f, 0f), new Vector2(1f, 1f), 15, TextAnchor.MiddleCenter, TextMuted);

            CreateText(panel, "Quest Title", "ПЕРВЫЙ ЛАГЕРЬ",
                new Vector2(0.075f, 0.66f), new Vector2(0.94f, 0.79f), 22,
                TextAnchor.MiddleLeft, Warm);

            CreateQuestRow(panel, 0.53f, "Собрать: Дерево", "0/10");
            CreateQuestRow(panel, 0.39f, "Собрать: Камень", "0/8");
            CreateQuestRow(panel, 0.25f, "Скрафтить: Костёр", "0/1");
            CreateQuestRow(panel, 0.11f, "Построить: Ящик", "0/1");
        }

        private static void CreateQuestRow(RectTransform panel, float y, string label, string value)
        {
            var check = CreatePanel(panel, "Check",
                new Vector2(0.08f, y), new Vector2(0.13f, y + 0.075f),
                new Color(0.04f, 0.055f, 0.06f, 1f), true);
            AddOutline(check, Border, 1f);

            CreateText(panel, label, label,
                new Vector2(0.16f, y - 0.005f), new Vector2(0.76f, y + 0.085f),
                17, TextAnchor.MiddleLeft, TextPrimary);
            CreateText(panel, value, value,
                new Vector2(0.77f, y - 0.005f), new Vector2(0.94f, y + 0.085f),
                17, TextAnchor.MiddleRight, TextMuted);
        }

        private static void BuildTopMenu(RectTransform root)
        {
            string[] labels = { "МАГАЗИН", "СТРОЙКА", "СОБЫТИЯ", "ПЕРСОНАЖ", "МЕНЮ" };
            TopIcon[] icons = { TopIcon.Shop, TopIcon.Build, TopIcon.Event, TopIcon.Character, TopIcon.Menu };

            float width = 0.059f;
            float gap = 0.007f;
            float start = 0.665f;

            for (int i = 0; i < labels.Length; i++)
            {
                float x0 = start + i * (width + gap);
                float x1 = x0 + width;

                var buttonRoot = CreatePanel(root, labels[i],
                    new Vector2(x0, 0.892f), new Vector2(x1, 0.985f),
                    new Color(0.025f, 0.037f, 0.042f, 0.91f), true);
                AddOutline(buttonRoot, Border, 1f);

                var button = buttonRoot.gameObject.AddComponent<Button>();
                buttonRoot.GetComponent<Image>().raycastTarget = true;
                button.targetGraphic = buttonRoot.GetComponent<Image>();
                buttonRoot.gameObject.AddComponent<HudTopButton>();

                DrawTopIcon(buttonRoot, icons[i]);
                CreateText(buttonRoot, "Label", labels[i],
                    new Vector2(0.02f, 0.03f), new Vector2(0.98f, 0.31f),
                    12, TextAnchor.MiddleCenter, TextPrimary);
            }
        }

        private static void DrawTopIcon(RectTransform root, TopIcon icon)
        {
            var holder = CreateRect("Icon", root, new Vector2(0.22f, 0.34f), new Vector2(0.78f, 0.91f));

            switch (icon)
            {
                case TopIcon.Shop:
                    CreateCircle(holder, "Coin 1", new Vector2(0.20f, 0.18f), new Vector2(0.52f, 0.50f), Warm);
                    CreateCircle(holder, "Coin 2", new Vector2(0.43f, 0.35f), new Vector2(0.75f, 0.67f), Warm);
                    CreateCircle(holder, "Coin 3", new Vector2(0.24f, 0.53f), new Vector2(0.56f, 0.85f), Warm);
                    break;
                case TopIcon.Craft:
                    CreateIconBar(holder, new Vector2(0.28f, 0.15f), new Vector2(0.40f, 0.86f), 42f);
                    CreateIconBar(holder, new Vector2(0.60f, 0.15f), new Vector2(0.72f, 0.86f), -42f);
                    break;
                case TopIcon.Build:
                    CreateIconBar(holder, new Vector2(0.24f, 0.43f), new Vector2(0.76f, 0.80f), 0f);
                    CreateIconBar(holder, new Vector2(0.23f, 0.46f), new Vector2(0.53f, 0.58f), 42f);
                    CreateIconBar(holder, new Vector2(0.47f, 0.46f), new Vector2(0.77f, 0.58f), -42f);
                    CreatePanel(holder, "Door", new Vector2(0.44f, 0.43f), new Vector2(0.57f, 0.68f), Panel, false);
                    break;
                case TopIcon.Event:
                    var calendar = CreatePanel(holder, "Calendar", new Vector2(0.22f, 0.20f), new Vector2(0.78f, 0.79f), TextPrimary, true);
                    CreatePanel(calendar, "Cutout", new Vector2(0.10f, 0.30f), new Vector2(0.90f, 0.78f), Panel, true);
                    CreatePanel(holder, "Tab L", new Vector2(0.31f, 0.72f), new Vector2(0.39f, 0.94f), TextPrimary, true);
                    CreatePanel(holder, "Tab R", new Vector2(0.61f, 0.72f), new Vector2(0.69f, 0.94f), TextPrimary, true);
                    break;
                case TopIcon.Character:
                    CreateCircle(holder, "Head", new Vector2(0.35f, 0.53f), new Vector2(0.65f, 0.84f), TextPrimary);
                    CreatePanel(holder, "Body", new Vector2(0.27f, 0.16f), new Vector2(0.73f, 0.52f), TextPrimary, true);
                    break;
                case TopIcon.Menu:
                    CreateIconBar(holder, new Vector2(0.20f, 0.66f), new Vector2(0.80f, 0.76f), 0f);
                    CreateIconBar(holder, new Vector2(0.20f, 0.46f), new Vector2(0.80f, 0.56f), 0f);
                    CreateIconBar(holder, new Vector2(0.20f, 0.26f), new Vector2(0.80f, 0.36f), 0f);
                    break;
            }
        }

        private static void BuildMinimap(RectTransform root, GameObject player)
        {
            var frame = CreateCircle(root, "Minimap Frame",
                new Vector2(0.824f, 0.625f), new Vector2(0.982f, 0.895f),
                new Color(0.60f, 0.68f, 0.69f, 0.88f));

            var maskRoot = CreateCircle(frame, "Minimap Mask",
                new Vector2(0.035f, 0.035f), new Vector2(0.965f, 0.965f), Color.white);
            var mask = maskRoot.gameObject.AddComponent<Mask>();
            mask.showMaskGraphic = false;

            var rawGo = new GameObject("Map Feed", typeof(RectTransform), typeof(RawImage));
            rawGo.transform.SetParent(maskRoot, false);
            var rawRt = rawGo.GetComponent<RectTransform>();
            rawRt.anchorMin = Vector2.zero;
            rawRt.anchorMax = Vector2.one;
            rawRt.offsetMin = Vector2.zero;
            rawRt.offsetMax = Vector2.zero;

            var renderTexture = GetOrCreateMinimapTexture();
            rawGo.GetComponent<RawImage>().texture = renderTexture;

            var cameraGo = new GameObject("Minimap Camera");
            var camera = cameraGo.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 16f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.035f, 0.085f, 0.055f, 1f);
            camera.targetTexture = renderTexture;
            camera.depth = -10f;
            cameraGo.AddComponent<MinimapFollow>().Bind(player.transform);

            var ring = new GameObject("Minimap Ring", typeof(RectTransform), typeof(Image));
            ring.transform.SetParent(frame, false);
            SetAnchors(ring.GetComponent<RectTransform>(), Vector2.zero, Vector2.one);
            ring.GetComponent<Image>().sprite = ringSprite;
            ring.GetComponent<Image>().color = new Color(0.08f, 0.10f, 0.11f, 0.95f);
            ring.GetComponent<Image>().raycastTarget = false;

            CreateText(frame, "N", "N", new Vector2(0.43f, 0.87f), new Vector2(0.57f, 0.99f), 18, TextAnchor.MiddleCenter, TextPrimary);
            CreateText(frame, "S", "S", new Vector2(0.43f, 0.01f), new Vector2(0.57f, 0.13f), 18, TextAnchor.MiddleCenter, TextPrimary);
            CreateText(frame, "W", "W", new Vector2(0.01f, 0.43f), new Vector2(0.13f, 0.57f), 18, TextAnchor.MiddleCenter, TextPrimary);
            CreateText(frame, "E", "E", new Vector2(0.87f, 0.43f), new Vector2(0.99f, 0.57f), 18, TextAnchor.MiddleCenter, TextPrimary);

            var arrow = CreatePanel(frame, "Player Arrow",
                new Vector2(0.465f, 0.44f), new Vector2(0.535f, 0.60f), TextPrimary, true);
            arrow.localRotation = Quaternion.Euler(0f, 0f, 45f);

            var home = CreateCircle(frame, "Home Marker",
                new Vector2(0.31f, 0.59f), new Vector2(0.38f, 0.66f), Color.white);
            var danger = CreateCircle(frame, "Danger Marker",
                new Vector2(0.25f, 0.31f), new Vector2(0.32f, 0.38f), Red);
            var chest = CreateCircle(frame, "Loot Marker",
                new Vector2(0.69f, 0.55f), new Vector2(0.76f, 0.62f), Warm);

            var timeChip = CreatePanel(root, "World Time",
                new Vector2(0.895f, 0.585f), new Vector2(0.982f, 0.625f), Panel, true);
            CreateStatGlyph(timeChip, "Sun", StatGlyph.Sun,
                new Vector2(0.06f, 0.18f), new Vector2(0.24f, 0.82f), Warm);
            var time = CreateText(timeChip, "Time", "18:24",
                new Vector2(0.28f, 0f), new Vector2(0.95f, 1f), 17, TextAnchor.MiddleCenter, TextPrimary);
            var clock = timeChip.gameObject.AddComponent<HudClock>();
            BindObject(clock, "timeText", time);
        }

        private static void BuildJoystick(RectTransform root, GameObject player)
        {
            var outer = CreateCircle(root, "Movement Joystick",
                new Vector2(0.035f, 0.055f), new Vector2(0.177f, 0.305f),
                new Color(0.02f, 0.03f, 0.035f, 0.55f));
            AddOutline(outer, new Color(0.75f, 0.79f, 0.80f, 0.65f), 2f);

            var inner = CreateCircle(outer, "Handle",
                new Vector2(0.28f, 0.28f), new Vector2(0.72f, 0.72f),
                new Color(0.65f, 0.69f, 0.70f, 0.82f));

            CreateDirectionTick(outer, "N", new Vector2(0.47f, 0.85f), new Vector2(0.53f, 0.94f), 0f);
            CreateDirectionTick(outer, "S", new Vector2(0.47f, 0.06f), new Vector2(0.53f, 0.15f), 180f);
            CreateDirectionTick(outer, "W", new Vector2(0.06f, 0.47f), new Vector2(0.15f, 0.53f), 90f);
            CreateDirectionTick(outer, "E", new Vector2(0.85f, 0.47f), new Vector2(0.94f, 0.53f), -90f);

            outer.GetComponent<Image>().raycastTarget = true;
            var joystick = outer.gameObject.AddComponent<VirtualJoystick>();
            joystick.Bind(player.GetComponent<MobileRunController>());
            BindObject(joystick, "background", outer);
            BindObject(joystick, "handle", inner);
        }

        private static void BuildActionCluster(RectTransform root)
        {
            var interact = CreateActionButton(root, "Interact",
                new Vector2(0.785f, 0.175f), new Vector2(0.855f, 0.300f), "ВЗЯТЬ", false);
            DrawHandGlyph(interact);

            var aim = CreateActionButton(root, "Aim",
                new Vector2(0.865f, 0.275f), new Vector2(0.930f, 0.390f), "", false);
            DrawCrosshair(aim);

            var main = CreateActionButton(root, "Context Action",
                new Vector2(0.842f, 0.045f), new Vector2(0.947f, 0.235f), "ДЕЙСТВИЕ", true);
            DrawToolGlyph(main);
        }


        private static void BuildBottomUtilityButtons(RectTransform root)
        {
            var craft = CreateUtilityButton(root, "Craft Quick Button",
                new Vector2(0.936f, 0.285f), new Vector2(0.988f, 0.378f), "КРАФТ");
            DrawCraftGlyph(craft);

            var backpack = CreateUtilityButton(root, "Backpack Quick Button",
                new Vector2(0.936f, 0.180f), new Vector2(0.988f, 0.273f), "РЮКЗАК");
            DrawBackpackGlyph(backpack);
        }

        private static RectTransform CreateUtilityButton(RectTransform root, string name, Vector2 min, Vector2 max, string label)
        {
            var frame = CreateCircle(root, name + " Frame", min, max,
                new Color(0.54f, 0.61f, 0.62f, 0.72f));
            var buttonRoot = CreateCircle(frame, name,
                new Vector2(0.045f, 0.045f), new Vector2(0.955f, 0.955f),
                new Color(0.025f, 0.037f, 0.042f, 0.96f));

            var image = buttonRoot.GetComponent<Image>();
            image.raycastTarget = true;

            var button = buttonRoot.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            buttonRoot.gameObject.AddComponent<HudTopButton>();

            CreateText(buttonRoot, "Label", label,
                new Vector2(0.05f, 0.02f), new Vector2(0.95f, 0.22f),
                10, TextAnchor.MiddleCenter, TextMuted);

            return buttonRoot;
        }

        private static void DrawBackpackGlyph(RectTransform parent)
        {
            var bag = CreatePanel(parent, "Bag",
                new Vector2(0.29f, 0.29f), new Vector2(0.71f, 0.73f), TextPrimary, true);
            CreatePanel(parent, "Top Strap",
                new Vector2(0.40f, 0.67f), new Vector2(0.60f, 0.82f), TextPrimary, true);
            CreatePanel(parent, "Pocket",
                new Vector2(0.36f, 0.34f), new Vector2(0.64f, 0.48f),
                new Color(0.10f, 0.13f, 0.14f, 1f), true);
            CreateIconBar(parent, new Vector2(0.21f, 0.37f), new Vector2(0.30f, 0.68f), -8f);
            CreateIconBar(parent, new Vector2(0.70f, 0.37f), new Vector2(0.79f, 0.68f), 8f);
        }

        private static void DrawCraftGlyph(RectTransform parent)
        {
            CreateIconBar(parent, new Vector2(0.30f, 0.28f), new Vector2(0.39f, 0.73f), 38f);
            CreateIconBar(parent, new Vector2(0.61f, 0.28f), new Vector2(0.70f, 0.73f), -38f);
            CreateCircle(parent, "Joint",
                new Vector2(0.43f, 0.43f), new Vector2(0.57f, 0.57f), Warm);
        }

        private static RectTransform CreateActionButton(RectTransform root, string name, Vector2 min, Vector2 max, string label, bool primary)
        {
            var frame = CreateCircle(root, name + " Frame", min, max,
                primary ? new Color(0.72f, 0.79f, 0.80f, 0.88f) : new Color(0.55f, 0.61f, 0.62f, 0.70f));
            var buttonRoot = CreateCircle(frame, name,
                new Vector2(0.035f, 0.035f), new Vector2(0.965f, 0.965f),
                new Color(0.025f, 0.035f, 0.040f, primary ? 0.97f : 0.91f));

            var button = buttonRoot.gameObject.AddComponent<Button>();
            buttonRoot.GetComponent<Image>().raycastTarget = true;
            button.targetGraphic = buttonRoot.GetComponent<Image>();
            buttonRoot.gameObject.AddComponent<HudTopButton>();

            if (!string.IsNullOrEmpty(label))
                CreateText(buttonRoot, "Label", label,
                    new Vector2(0.08f, 0.02f), new Vector2(0.92f, 0.25f),
                    primary ? 12 : 11, TextAnchor.MiddleCenter, TextMuted);

            return buttonRoot;
        }

        private static void BuildExpBar(RectTransform root, GameObject player)
        {
            var levelText = CreateText(root, "Bottom Level", "Ур. 1",
                new Vector2(0.312f, 0.008f), new Vector2(0.365f, 0.047f),
                15, TextAnchor.MiddleRight, TextPrimary);

            var exp = CreateBar(root, "Experience",
                new Vector2(0.375f, 0.018f), new Vector2(0.625f, 0.030f), Cyan);
            AddOutline(exp.GetComponent<RectTransform>(), new Color(0.48f, 0.55f, 0.56f, 0.45f), 1f);

            var expText = CreateText(root, "EXP Value", "0 / 100",
                new Vector2(0.635f, 0.006f), new Vector2(0.705f, 0.047f),
                15, TextAnchor.MiddleLeft, TextPrimary);

            var presenter = Object.FindFirstObjectByType<PlayerHudPresenter>();
            if (presenter)
            {
                BindObject(presenter, "expBar", exp);
                BindObject(presenter, "bottomLevelText", levelText);
                BindObject(presenter, "expText", expText);
            }
        }

        private static Slider CreateBar(RectTransform parent, string name, Vector2 min, Vector2 max, Color fillColor)
        {
            var root = CreateRect(name, parent, min, max);
            var bg = CreatePanel(root, "Background", Vector2.zero, Vector2.one,
                new Color(0.015f, 0.020f, 0.022f, 0.95f), true);
            var fill = CreatePanel(root, "Fill", new Vector2(0f, 0f), new Vector2(1f, 1f), fillColor, true);

            var slider = root.gameObject.AddComponent<Slider>();
            slider.fillRect = fill;
            slider.targetGraphic = fill.GetComponent<Image>();
            slider.interactable = false;
            slider.minValue = 0f;
            slider.maxValue = 100f;
            slider.value = 100f;
            return slider;
        }

        private static RectTransform CreateFramedPanel(RectTransform parent, string name, Vector2 min, Vector2 max, Color color)
        {
            var shadow = CreatePanel(parent, name + " Shadow",
                min + new Vector2(0.003f, -0.004f), max + new Vector2(0.003f, -0.004f),
                new Color(0f, 0f, 0f, 0.35f), true);

            var panel = CreatePanel(parent, name, min, max, color, true);
            AddOutline(panel, Border, 1f);
            return panel;
        }

        private static RectTransform CreatePanel(RectTransform parent, string name, Vector2 min, Vector2 max, Color color, bool rounded)
        {
            var rt = CreateRect(name, parent, min, max);
            var image = rt.gameObject.AddComponent<Image>();
            image.color = color;
            image.raycastTarget = false;
            if (rounded)
            {
                image.sprite = roundedSprite;
                image.type = Image.Type.Sliced;
            }
            return rt;
        }

        private static RectTransform CreateCircle(Transform parent, string name, Vector2 min, Vector2 max, Color color)
        {
            var rt = CreateRect(name, parent, min, max);
            var image = rt.gameObject.AddComponent<Image>();
            image.sprite = circleSprite;
            image.color = color;
            image.raycastTarget = false;
            return rt;
        }

        private static Text CreateText(Transform parent, string name, string value, Vector2 min, Vector2 max,
            int size, TextAnchor alignment, Color color)
        {
            var rt = CreateRect(name, parent, min, max);
            var text = rt.gameObject.AddComponent<Text>();
            text.text = value;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = size;
            text.alignment = alignment;
            text.color = color;
            text.raycastTarget = false;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            return text;
        }

        private static RectTransform CreateRect(string name, Transform parent, Vector2 min, Vector2 max)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            SetAnchors(rt, min, max);
            return rt;
        }

        private static void AddOutline(RectTransform rt, Color color, float distance)
        {
            var outline = rt.gameObject.AddComponent<Outline>();
            outline.effectColor = color;
            outline.effectDistance = new Vector2(distance, -distance);
            outline.useGraphicAlpha = true;
        }

        private static void CreateDirectionTick(RectTransform parent, string name, Vector2 min, Vector2 max, float angle)
        {
            var tick = CreatePanel(parent, name, min, max, new Color(0.76f, 0.80f, 0.81f, 0.72f), true);
            tick.localRotation = Quaternion.Euler(0f, 0f, angle + 45f);
        }

        private static void CreateIconBar(RectTransform parent, Vector2 min, Vector2 max, float rotation)
        {
            var bar = CreatePanel(parent, "Bar", min, max, TextPrimary, true);
            bar.localRotation = Quaternion.Euler(0f, 0f, rotation);
        }

        private static void DrawCrosshair(RectTransform parent)
        {
            CreateIconBar(parent, new Vector2(0.20f, 0.48f), new Vector2(0.80f, 0.52f), 0f);
            CreateIconBar(parent, new Vector2(0.48f, 0.20f), new Vector2(0.52f, 0.80f), 0f);
            CreateCircle(parent, "Center", new Vector2(0.45f, 0.45f), new Vector2(0.55f, 0.55f), TextPrimary);
        }

        private static void DrawHandGlyph(RectTransform parent)
        {
            var palm = CreatePanel(parent, "Palm", new Vector2(0.35f, 0.35f), new Vector2(0.62f, 0.62f), TextPrimary, true);
            CreateIconBar(parent, new Vector2(0.29f, 0.52f), new Vector2(0.36f, 0.77f), -12f);
            CreateIconBar(parent, new Vector2(0.38f, 0.58f), new Vector2(0.45f, 0.82f), -4f);
            CreateIconBar(parent, new Vector2(0.47f, 0.59f), new Vector2(0.54f, 0.82f), 4f);
            CreateIconBar(parent, new Vector2(0.56f, 0.55f), new Vector2(0.63f, 0.77f), 12f);
        }

        private static void DrawToolGlyph(RectTransform parent)
        {
            var handle = CreatePanel(parent, "Handle", new Vector2(0.46f, 0.30f), new Vector2(0.54f, 0.73f), TextPrimary, true);
            handle.localRotation = Quaternion.Euler(0f, 0f, -38f);
            var head = CreatePanel(parent, "Head", new Vector2(0.33f, 0.61f), new Vector2(0.67f, 0.72f), TextPrimary, true);
            head.localRotation = Quaternion.Euler(0f, 0f, -12f);
        }

        private static void CreateStatGlyph(RectTransform parent, string name, StatGlyph glyph, Vector2 min, Vector2 max, Color color)
        {
            var holder = CreateRect(name, parent, min, max);

            switch (glyph)
            {
                case StatGlyph.Heart:
                    CreateCircle(holder, "L", new Vector2(0.10f, 0.36f), new Vector2(0.58f, 0.82f), color);
                    CreateCircle(holder, "R", new Vector2(0.42f, 0.36f), new Vector2(0.90f, 0.82f), color);
                    var diamond = CreatePanel(holder, "D", new Vector2(0.28f, 0.15f), new Vector2(0.72f, 0.59f), color, false);
                    diamond.localRotation = Quaternion.Euler(0f, 0f, 45f);
                    break;
                case StatGlyph.Shield:
                    var shield = CreatePanel(holder, "Shield", new Vector2(0.18f, 0.12f), new Vector2(0.82f, 0.88f), color, true);
                    shield.localScale = new Vector3(0.85f, 1f, 1f);
                    break;
                case StatGlyph.Food:
                    CreateCircle(holder, "Food", new Vector2(0.12f, 0.18f), new Vector2(0.66f, 0.78f), color);
                    var bone = CreatePanel(holder, "Bone", new Vector2(0.55f, 0.18f), new Vector2(0.70f, 0.62f), color, true);
                    bone.localRotation = Quaternion.Euler(0f, 0f, -35f);
                    break;
                case StatGlyph.Drop:
                    CreateCircle(holder, "Drop", new Vector2(0.20f, 0.12f), new Vector2(0.80f, 0.70f), color);
                    var top = CreatePanel(holder, "Top", new Vector2(0.37f, 0.55f), new Vector2(0.63f, 0.88f), color, false);
                    top.localRotation = Quaternion.Euler(0f, 0f, 45f);
                    break;
                case StatGlyph.Sun:
                    CreateCircle(holder, "Sun", new Vector2(0.22f, 0.22f), new Vector2(0.78f, 0.78f), color);
                    break;
            }
        }

        private static RenderTexture GetOrCreateMinimapTexture()
        {
            const string path = UiFolder + "/GameplayMinimap.renderTexture";
            var rt = AssetDatabase.LoadAssetAtPath<RenderTexture>(path);
            if (rt) return rt;

            rt = new RenderTexture(512, 512, 16, RenderTextureFormat.ARGB32)
            {
                name = "GameplayMinimap",
                filterMode = FilterMode.Bilinear
            };
            AssetDatabase.CreateAsset(rt, path);
            AssetDatabase.SaveAssets();
            return rt;
        }

        private static void EnsureUiArt()
        {
            string diskFolder = Path.Combine(Application.dataPath, "Generated/UI");
            Directory.CreateDirectory(diskFolder);

            roundedSprite = EnsureSprite("rounded_panel", 64, 64, true, false);
            circleSprite = EnsureSprite("circle", 128, 128, false, false);
            ringSprite = EnsureSprite("ring", 128, 128, false, true);
        }

        private static Sprite EnsureSprite(string name, int width, int height, bool rounded, bool ring)
        {
            string assetPath = UiFolder + "/" + name + ".png";
            var loaded = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            if (loaded) return loaded;

            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.name = name;

            float cx = (width - 1) * 0.5f;
            float cy = (height - 1) * 0.5f;
            float radius = Mathf.Min(width, height) * 0.5f - 1f;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float alpha = 0f;

                    if (ring)
                    {
                        float d = Vector2.Distance(new Vector2(x, y), new Vector2(cx, cy));
                        alpha = d <= radius && d >= radius - 5f ? 1f : 0f;
                    }
                    else if (!rounded)
                    {
                        float d = Vector2.Distance(new Vector2(x, y), new Vector2(cx, cy));
                        alpha = d <= radius ? 1f : 0f;
                    }
                    else
                    {
                        float r = 13f;
                        float px = Mathf.Clamp(x, r, width - 1 - r);
                        float py = Mathf.Clamp(y, r, height - 1 - r);
                        float d = Vector2.Distance(new Vector2(x, y), new Vector2(px, py));
                        alpha = d <= r ? 1f : 0f;
                    }

                    texture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
                }
            }

            texture.Apply();
            File.WriteAllBytes(Path.Combine(Application.dataPath, "Generated/UI/" + name + ".png"), texture.EncodeToPNG());
            Object.DestroyImmediate(texture);

            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceSynchronousImport);
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.alphaIsTransparency = true;
            importer.mipmapEnabled = false;
            importer.filterMode = FilterMode.Bilinear;
            if (rounded)
                importer.spriteBorder = new Vector4(13f, 13f, 13f, 13f);
            importer.SaveAndReimport();

            return AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
        }

        private static void SetAnchors(RectTransform rt, Vector2 min, Vector2 max)
        {
            rt.anchorMin = min;
            rt.anchorMax = max;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        private static void BindObject(Object target, string field, Object value)
        {
            var serialized = new SerializedObject(target);
            var property = serialized.FindProperty(field);
            if (property == null) return;

            property.objectReferenceValue = value;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private enum TopIcon
        {
            Shop,
            Craft,
            Build,
            Event,
            Character,
            Menu
        }

        private enum StatGlyph
        {
            Heart,
            Shield,
            Food,
            Drop,
            Sun
        }
    }
}
#endif
