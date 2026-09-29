#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SurvivalGame.EditorTools
{
    public static class ProductionUiArtGenerator
    {
        public const string Folder = "Assets/Generated/UI/Art";

        private static readonly Color White = new Color(0.92f, 0.95f, 0.95f, 1f);
        private static readonly Color Dark = new Color(0.075f, 0.095f, 0.105f, 1f);
        private static readonly Color Cyan = new Color(0.10f, 0.72f, 0.82f, 1f);
        private static readonly Color Red = new Color(0.93f, 0.18f, 0.20f, 1f);
        private static readonly Color Blue = new Color(0.18f, 0.62f, 0.90f, 1f);
        private static readonly Color Orange = new Color(0.95f, 0.61f, 0.16f, 1f);
        private static readonly Color Gold = new Color(0.98f, 0.72f, 0.14f, 1f);

        public static void Ensure()
        {
            EnsureFolder("Assets", "Generated");
            EnsureFolder("Assets/Generated", "UI");
            EnsureFolder("Assets/Generated/UI", "Art");

            GenerateIfMissing("ui_panel", MakePanel(), true);
            GenerateIfMissing("ui_button_circle", MakeCircleButton(), false);
            GenerateIfMissing("ui_button_square", MakeSquareButton(), true);

            GenerateIfMissing("icon_backpack", MakeBackpack(), false);
            GenerateIfMissing("icon_craft", MakeCraft(), false);
            GenerateIfMissing("icon_interact", MakeHand(), false);
            GenerateIfMissing("icon_aim", MakeAim(), false);
            GenerateIfMissing("icon_axe", MakeAxe(), false);

            GenerateIfMissing("icon_heart", MakeHeart(), false);
            GenerateIfMissing("icon_armor", MakeShield(), false);
            GenerateIfMissing("icon_food", MakeFood(), false);
            GenerateIfMissing("icon_water", MakeWater(), false);

            GenerateIfMissing("icon_shop", MakeShop(), false);
            GenerateIfMissing("icon_build", MakeBuild(), false);
            GenerateIfMissing("icon_event", MakeEvent(), false);
            GenerateIfMissing("icon_character", MakeCharacter(), false);
            GenerateIfMissing("icon_menu", MakeMenu(), false);

            AssetDatabase.SaveAssets();
        }

        public static Sprite Load(string name)
        {
            Ensure();
            return AssetDatabase.LoadAssetAtPath<Sprite>($"{Folder}/{name}.png");
        }

        private static void GenerateIfMissing(string name, Texture2D texture, bool sliced)
        {
            string path = $"{Folder}/{name}.png";
            if (File.Exists(path))
            {
                Object.DestroyImmediate(texture);
                if (!AssetDatabase.LoadAssetAtPath<Sprite>(path))
                    Import(path, sliced);
                return;
            }

            File.WriteAllBytes(path, texture.EncodeToPNG());
            Object.DestroyImmediate(texture);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
            Import(path, sliced);
        }

        private static void Import(string path, bool sliced)
        {
            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (!importer) return;

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.alphaIsTransparency = true;
            importer.mipmapEnabled = false;
            importer.filterMode = FilterMode.Bilinear;
            importer.textureCompression = TextureImporterCompression.CompressedHQ;
            importer.maxTextureSize = 512;

            if (sliced)
                importer.spriteBorder = new Vector4(22f, 22f, 22f, 22f);

            importer.SaveAndReimport();
        }

        private static Texture2D MakePanel()
        {
            var c = new Raster(128, 128);
            c.RoundedRect(new Rect(5, 8, 118, 116), 20, new Color(0, 0, 0, 0.40f));
            c.RoundedRect(new Rect(3, 3, 122, 122), 20, new Color(0.075f, 0.095f, 0.105f, 0.98f));
            c.RoundedRing(new Rect(3, 3, 122, 122), 20, 2.2f, new Color(0.50f, 0.57f, 0.58f, 0.70f));
            c.RoundedRing(new Rect(8, 8, 112, 112), 16, 1.2f, new Color(0.08f, 0.57f, 0.64f, 0.25f));
            c.Line(new Vector2(18, 15), new Vector2(110, 15), 1.3f, new Color(1, 1, 1, 0.12f));
            return c.ToTexture();
        }

        private static Texture2D MakeCircleButton()
        {
            var c = new Raster(256, 256);
            var center = new Vector2(128, 128);
            c.Circle(center + new Vector2(0, 7), 112, new Color(0, 0, 0, 0.52f));
            c.Circle(center, 112, new Color(0.69f, 0.73f, 0.74f, 1f));
            c.Circle(center, 105, new Color(0.28f, 0.31f, 0.32f, 1f));
            c.Circle(center, 100, new Color(0.055f, 0.070f, 0.078f, 1f));
            c.Ring(center, 92, 2.2f, new Color(0.09f, 0.63f, 0.71f, 0.43f));
            c.Arc(center, 106, 200, 340, 2.6f, new Color(1, 1, 1, 0.30f));
            c.Circle(new Vector2(92, 82), 22, new Color(1, 1, 1, 0.025f));
            return c.ToTexture();
        }

        private static Texture2D MakeSquareButton()
        {
            var c = new Raster(192, 192);
            c.RoundedRect(new Rect(11, 16, 170, 168), 29, new Color(0, 0, 0, 0.50f));
            c.RoundedRect(new Rect(7, 7, 178, 178), 29, new Color(0.075f, 0.095f, 0.105f, 0.99f));
            c.RoundedRing(new Rect(7, 7, 178, 178), 29, 3f, new Color(0.55f, 0.61f, 0.62f, 0.76f));
            c.RoundedRing(new Rect(14, 14, 164, 164), 23, 1.5f, new Color(0.07f, 0.57f, 0.64f, 0.30f));
            c.Line(new Vector2(27, 20), new Vector2(165, 20), 1.5f, new Color(1, 1, 1, 0.13f));
            return c.ToTexture();
        }

        private static Texture2D MakeBackpack()
        {
            var c = IconCanvas();
            Shadow(c, r =>
            {
                r.Arc(new Vector2(128, 93), 57, 202, 338, 13, White);
                r.RoundedRect(new Rect(55, 76, 146, 145), 30, White);
                r.RoundedRect(new Rect(43, 103, 28, 96), 12, White);
                r.RoundedRect(new Rect(185, 103, 28, 96), 12, White);
            });

            c.Arc(new Vector2(128, 93), 57, 202, 338, 13, White);
            c.RoundedRect(new Rect(55, 76, 146, 145), 30, White);
            c.RoundedRect(new Rect(43, 103, 28, 96), 12, new Color(0.82f, 0.86f, 0.86f, 1f));
            c.RoundedRect(new Rect(185, 103, 28, 96), 12, new Color(0.82f, 0.86f, 0.86f, 1f));
            c.RoundedRect(new Rect(77, 123, 102, 61), 15, Dark);
            c.RoundedRing(new Rect(88, 133, 80, 39), 11, 4, new Color(0.40f, 0.48f, 0.50f, 1f));
            c.RoundedRect(new Rect(118, 80, 20, 26), 4, Dark);
            return c.ToTexture();
        }

        private static Texture2D MakeCraft()
        {
            var c = IconCanvas();
            c.Line(new Vector2(65, 194), new Vector2(181, 78), 25, new Color(0, 0, 0, 0.35f));
            c.Line(new Vector2(76, 72), new Vector2(190, 193), 22, new Color(0, 0, 0, 0.35f));

            c.Line(new Vector2(65, 194), new Vector2(181, 78), 18, White);
            c.Circle(new Vector2(65, 197), 20, White);
            c.Circle(new Vector2(190, 68), 22, White);

            c.Line(new Vector2(76, 72), new Vector2(190, 193), 16, White);
            c.RoundedRect(new Rect(43, 45, 71, 40), 9, White);
            c.Circle(new Vector2(128, 130), 9, Cyan);
            return c.ToTexture();
        }

        private static Texture2D MakeHand()
        {
            var c = IconCanvas();
            var sh = new Color(0, 0, 0, 0.35f);
            DrawHand(c, sh, new Vector2(3, 4));
            DrawHand(c, White, Vector2.zero);
            return c.ToTexture();
        }

        private static void DrawHand(Raster c, Color color, Vector2 offset)
        {
            c.RoundedRect(new Rect(84 + offset.x, 103 + offset.y, 94, 108), 34, color);
            float[] xs = { 77, 104, 132, 159 };
            float[] tops = { 57, 42, 48, 65 };
            for (int i = 0; i < xs.Length; i++)
                c.RoundedRect(new Rect(xs[i] + offset.x, tops[i] + offset.y, 23, 83), 11, color);
            c.RoundedRect(new Rect(52 + offset.x, 116 + offset.y, 59, 31), 14, color);
        }

        private static Texture2D MakeAim()
        {
            var c = IconCanvas();
            c.Ring(new Vector2(128, 128), 74, 8, White);
            c.Ring(new Vector2(128, 128), 26, 6, White);
            c.Line(new Vector2(128, 24), new Vector2(128, 82), 8, White);
            c.Line(new Vector2(128, 174), new Vector2(128, 232), 8, White);
            c.Line(new Vector2(24, 128), new Vector2(82, 128), 8, White);
            c.Line(new Vector2(174, 128), new Vector2(232, 128), 8, White);
            c.Circle(new Vector2(128, 128), 5, Cyan);
            return c.ToTexture();
        }

        private static Texture2D MakeAxe()
        {
            var c = IconCanvas();
            c.Line(new Vector2(92, 214), new Vector2(158, 73), 22, new Color(0, 0, 0, 0.35f));
            c.Line(new Vector2(96, 208), new Vector2(158, 76), 13, new Color(0.62f, 0.39f, 0.19f, 1f));
            c.Polygon(new[]
            {
                new Vector2(132, 48), new Vector2(197, 55), new Vector2(221, 88),
                new Vector2(180, 125), new Vector2(144, 111)
            }, new Color(0.88f, 0.91f, 0.92f, 1f));
            c.Line(new Vector2(148, 56), new Vector2(202, 65), 2.5f, new Color(1, 1, 1, 0.55f));
            return c.ToTexture();
        }

        private static Texture2D MakeHeart()
        {
            var c = IconCanvas();
            c.Circle(new Vector2(91, 96), 49, Red);
            c.Circle(new Vector2(165, 96), 49, Red);
            c.Polygon(new[]
            {
                new Vector2(48, 105), new Vector2(208, 105), new Vector2(128, 224)
            }, Red);
            c.Line(new Vector2(74, 72), new Vector2(103, 61), 4, new Color(1, 1, 1, 0.25f));
            return c.ToTexture();
        }

        private static Texture2D MakeShield()
        {
            var c = IconCanvas();
            c.Polygon(new[]
            {
                new Vector2(128, 28), new Vector2(207, 60), new Vector2(198, 147),
                new Vector2(166, 197), new Vector2(128, 226), new Vector2(90, 197),
                new Vector2(58, 147), new Vector2(49, 60)
            }, Blue);
            c.Polyline(new[]
            {
                new Vector2(128, 51), new Vector2(181, 73), new Vector2(174, 139),
                new Vector2(151, 176), new Vector2(128, 194), new Vector2(105, 176),
                new Vector2(82, 139), new Vector2(75, 73), new Vector2(128, 51)
            }, 4, new Color(0.80f, 0.91f, 0.96f, 0.70f));
            return c.ToTexture();
        }

        private static Texture2D MakeFood()
        {
            var c = IconCanvas();
            c.Circle(new Vector2(111, 111), 57, Orange);
            c.Polygon(new[]
            {
                new Vector2(70, 77), new Vector2(177, 151), new Vector2(153, 183), new Vector2(53, 111)
            }, Orange);
            c.Line(new Vector2(155, 166), new Vector2(207, 211), 19, Orange);
            c.Circle(new Vector2(207, 211), 15, Orange);
            c.Circle(new Vector2(191, 224), 14, Orange);
            return c.ToTexture();
        }

        private static Texture2D MakeWater()
        {
            var c = IconCanvas();
            c.Polygon(new[]
            {
                new Vector2(128, 24), new Vector2(72, 112), new Vector2(55, 149),
                new Vector2(61, 185), new Vector2(87, 215), new Vector2(128, 227),
                new Vector2(169, 215), new Vector2(195, 185), new Vector2(201, 149),
                new Vector2(184, 112)
            }, new Color(0.15f, 0.69f, 0.93f, 1f));
            c.Circle(new Vector2(128, 166), 73, new Color(0.15f, 0.69f, 0.93f, 1f));
            c.Arc(new Vector2(120, 160), 42, 185, 285, 6, new Color(0.86f, 0.97f, 1f, 0.65f));
            return c.ToTexture();
        }

        private static Texture2D MakeShop()
        {
            var c = IconCanvas();
            c.Ellipse(new Rect(57, 137, 64, 29), Gold);
            c.Ellipse(new Rect(100, 103, 64, 29), Gold);
            c.Ellipse(new Rect(140, 143, 64, 29), Gold);
            c.Ellipse(new Rect(104, 168, 64, 29), Gold);
            c.Line(new Vector2(73, 147), new Vector2(107, 147), 2, new Color(1, 0.92f, 0.55f, 0.7f));
            return c.ToTexture();
        }

        private static Texture2D MakeBuild()
        {
            var c = IconCanvas();
            c.Polygon(new[]
            {
                new Vector2(42, 121), new Vector2(128, 48), new Vector2(214, 121),
                new Vector2(195, 121), new Vector2(195, 213), new Vector2(61, 213), new Vector2(61, 121)
            }, White);
            c.Rect(new Rect(111, 151, 35, 62), Dark);
            c.Rect(new Rect(75, 136, 30, 30), Dark);
            c.Rect(new Rect(153, 136, 30, 30), Dark);
            return c.ToTexture();
        }

        private static Texture2D MakeEvent()
        {
            var c = IconCanvas();
            c.RoundedRect(new Rect(48, 55, 160, 160), 20, White);
            c.RoundedRect(new Rect(77, 35, 19, 42), 8, White);
            c.RoundedRect(new Rect(160, 35, 19, 42), 8, White);
            c.Rect(new Rect(62, 99, 132, 99), Dark);

            foreach (float x in new[] { 82f, 128f, 174f })
                foreach (float y in new[] { 122f, 158f })
                    c.Circle(new Vector2(x, y), 7, new Color(0.53f, 0.61f, 0.62f, 1f));
            return c.ToTexture();
        }

        private static Texture2D MakeCharacter()
        {
            var c = IconCanvas();
            c.Circle(new Vector2(128, 80), 40, White);
            c.RoundedRect(new Rect(62, 120, 132, 105), 49, White);
            return c.ToTexture();
        }

        private static Texture2D MakeMenu()
        {
            var c = IconCanvas();
            c.RoundedRect(new Rect(49, 69, 158, 19), 9, White);
            c.RoundedRect(new Rect(49, 119, 158, 19), 9, White);
            c.RoundedRect(new Rect(49, 169, 158, 19), 9, White);
            return c.ToTexture();
        }

        private static Raster IconCanvas() => new Raster(256, 256);

        private static void Shadow(Raster c, System.Action<Raster> draw)
        {
            var temp = new Raster(c.Width, c.Height);
            draw(temp);
            c.BlitAlpha(temp, new Vector2(3, 5), new Color(0, 0, 0, 0.38f));
        }

        private static void EnsureFolder(string parent, string child)
        {
            string path = parent + "/" + child;
            if (!AssetDatabase.IsValidFolder(path))
                AssetDatabase.CreateFolder(parent, child);
        }

        private sealed class Raster
        {
            public readonly int Width;
            public readonly int Height;
            private readonly Color[] pixels;

            public Raster(int width, int height)
            {
                Width = width;
                Height = height;
                pixels = new Color[width * height];
            }

            public Texture2D ToTexture()
            {
                var texture = new Texture2D(Width, Height, TextureFormat.RGBA32, false);
                texture.SetPixels(pixels);
                texture.Apply();
                return texture;
            }

            public void BlitAlpha(Raster source, Vector2 offset, Color tint)
            {
                int ox = Mathf.RoundToInt(offset.x);
                int oy = Mathf.RoundToInt(offset.y);

                for (int y = 0; y < source.Height; y++)
                {
                    for (int x = 0; x < source.Width; x++)
                    {
                        Color src = source.pixels[y * source.Width + x];
                        if (src.a <= 0.001f) continue;
                        Blend(x + ox, y + oy, new Color(tint.r, tint.g, tint.b, src.a * tint.a));
                    }
                }
            }

            public void Rect(Rect rect, Color color) => RoundedRect(rect, 0, color);

            public void RoundedRect(Rect rect, float radius, Color color)
            {
                int minX = Mathf.FloorToInt(rect.xMin - 1);
                int maxX = Mathf.CeilToInt(rect.xMax + 1);
                int minY = Mathf.FloorToInt(rect.yMin - 1);
                int maxY = Mathf.CeilToInt(rect.yMax + 1);

                Vector2 center = rect.center;
                Vector2 half = rect.size * 0.5f;
                float r = Mathf.Max(0, radius);

                for (int y = minY; y <= maxY; y++)
                {
                    for (int x = minX; x <= maxX; x++)
                    {
                        Vector2 p = new Vector2(x + 0.5f, y + 0.5f);
                        Vector2 q = new Vector2(Mathf.Abs(p.x - center.x), Mathf.Abs(p.y - center.y))
                                    - (half - new Vector2(r, r));
                        Vector2 maxQ = new Vector2(Mathf.Max(q.x, 0), Mathf.Max(q.y, 0));
                        float distance = maxQ.magnitude + Mathf.Min(Mathf.Max(q.x, q.y), 0) - r;
                        float coverage = Mathf.Clamp01(0.75f - distance);
                        if (coverage > 0) Blend(x, y, color * new Color(1, 1, 1, coverage));
                    }
                }
            }

            public void RoundedRing(Rect rect, float radius, float thickness, Color color)
            {
                Vector2 center = rect.center;
                Vector2 half = rect.size * 0.5f;
                float outerRadius = Mathf.Max(0, radius);
                float innerRadius = Mathf.Max(0, radius - thickness);
                Vector2 innerHalf = half - new Vector2(thickness, thickness);

                int minX = Mathf.FloorToInt(rect.xMin - 1);
                int maxX = Mathf.CeilToInt(rect.xMax + 1);
                int minY = Mathf.FloorToInt(rect.yMin - 1);
                int maxY = Mathf.CeilToInt(rect.yMax + 1);

                for (int y = minY; y <= maxY; y++)
                {
                    for (int x = minX; x <= maxX; x++)
                    {
                        Vector2 p = new Vector2(x + 0.5f, y + 0.5f);

                        float outer = RoundedRectDistance(p, center, half, outerRadius);
                        float inner = RoundedRectDistance(p, center, innerHalf, innerRadius);

                        float outerCoverage = Mathf.Clamp01(0.75f - outer);
                        float innerCoverage = Mathf.Clamp01(0.75f - inner);
                        float coverage = Mathf.Clamp01(outerCoverage - innerCoverage);

                        if (coverage > 0)
                            Blend(x, y, color * new Color(1, 1, 1, coverage));
                    }
                }
            }

            private static float RoundedRectDistance(Vector2 p, Vector2 center, Vector2 half, float radius)
            {
                Vector2 q = new Vector2(Mathf.Abs(p.x - center.x), Mathf.Abs(p.y - center.y))
                            - (half - new Vector2(radius, radius));
                Vector2 maxQ = new Vector2(Mathf.Max(q.x, 0), Mathf.Max(q.y, 0));
                return maxQ.magnitude + Mathf.Min(Mathf.Max(q.x, q.y), 0) - radius;
            }

            public void Circle(Vector2 center, float radius, Color color)
            {
                int minX = Mathf.FloorToInt(center.x - radius - 1);
                int maxX = Mathf.CeilToInt(center.x + radius + 1);
                int minY = Mathf.FloorToInt(center.y - radius - 1);
                int maxY = Mathf.CeilToInt(center.y + radius + 1);

                for (int y = minY; y <= maxY; y++)
                    for (int x = minX; x <= maxX; x++)
                    {
                        float distance = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), center);
                        float coverage = Mathf.Clamp01(radius + 0.75f - distance);
                        if (coverage > 0) Blend(x, y, color * new Color(1, 1, 1, coverage));
                    }
            }

            public void Ellipse(Rect rect, Color color)
            {
                Vector2 center = rect.center;
                Vector2 radius = rect.size * 0.5f;

                int minX = Mathf.FloorToInt(rect.xMin - 1);
                int maxX = Mathf.CeilToInt(rect.xMax + 1);
                int minY = Mathf.FloorToInt(rect.yMin - 1);
                int maxY = Mathf.CeilToInt(rect.yMax + 1);

                for (int y = minY; y <= maxY; y++)
                    for (int x = minX; x <= maxX; x++)
                    {
                        float nx = (x + 0.5f - center.x) / Mathf.Max(0.01f, radius.x);
                        float ny = (y + 0.5f - center.y) / Mathf.Max(0.01f, radius.y);
                        float d = Mathf.Sqrt(nx * nx + ny * ny);
                        float coverage = Mathf.Clamp01((1.01f - d) * Mathf.Min(radius.x, radius.y));
                        if (coverage > 0) Blend(x, y, color * new Color(1, 1, 1, coverage));
                    }
            }

            public void Ring(Vector2 center, float radius, float thickness, Color color)
            {
                int minX = Mathf.FloorToInt(center.x - radius - thickness);
                int maxX = Mathf.CeilToInt(center.x + radius + thickness);
                int minY = Mathf.FloorToInt(center.y - radius - thickness);
                int maxY = Mathf.CeilToInt(center.y + radius + thickness);

                float half = thickness * 0.5f;
                for (int y = minY; y <= maxY; y++)
                    for (int x = minX; x <= maxX; x++)
                    {
                        float d = Mathf.Abs(Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), center) - radius);
                        float coverage = Mathf.Clamp01(half + 0.75f - d);
                        if (coverage > 0) Blend(x, y, color * new Color(1, 1, 1, coverage));
                    }
            }

            public void Arc(Vector2 center, float radius, float startDegrees, float endDegrees, float thickness, Color color)
            {
                int minX = Mathf.FloorToInt(center.x - radius - thickness);
                int maxX = Mathf.CeilToInt(center.x + radius + thickness);
                int minY = Mathf.FloorToInt(center.y - radius - thickness);
                int maxY = Mathf.CeilToInt(center.y + radius + thickness);

                float half = thickness * 0.5f;
                for (int y = minY; y <= maxY; y++)
                {
                    for (int x = minX; x <= maxX; x++)
                    {
                        Vector2 delta = new Vector2(x + 0.5f, y + 0.5f) - center;
                        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
                        if (angle < 0) angle += 360;

                        bool inArc = startDegrees <= endDegrees
                            ? angle >= startDegrees && angle <= endDegrees
                            : angle >= startDegrees || angle <= endDegrees;

                        if (!inArc) continue;

                        float d = Mathf.Abs(delta.magnitude - radius);
                        float coverage = Mathf.Clamp01(half + 0.75f - d);
                        if (coverage > 0) Blend(x, y, color * new Color(1, 1, 1, coverage));
                    }
                }
            }

            public void Line(Vector2 a, Vector2 b, float thickness, Color color)
            {
                float radius = thickness * 0.5f;
                int minX = Mathf.FloorToInt(Mathf.Min(a.x, b.x) - radius - 1);
                int maxX = Mathf.CeilToInt(Mathf.Max(a.x, b.x) + radius + 1);
                int minY = Mathf.FloorToInt(Mathf.Min(a.y, b.y) - radius - 1);
                int maxY = Mathf.CeilToInt(Mathf.Max(a.y, b.y) + radius + 1);

                Vector2 ab = b - a;
                float lengthSq = Mathf.Max(0.0001f, ab.sqrMagnitude);

                for (int y = minY; y <= maxY; y++)
                    for (int x = minX; x <= maxX; x++)
                    {
                        Vector2 p = new Vector2(x + 0.5f, y + 0.5f);
                        float t = Mathf.Clamp01(Vector2.Dot(p - a, ab) / lengthSq);
                        Vector2 closest = a + ab * t;
                        float d = Vector2.Distance(p, closest);
                        float coverage = Mathf.Clamp01(radius + 0.75f - d);
                        if (coverage > 0) Blend(x, y, color * new Color(1, 1, 1, coverage));
                    }
            }

            public void Polyline(Vector2[] points, float thickness, Color color)
            {
                for (int i = 0; i < points.Length - 1; i++)
                    Line(points[i], points[i + 1], thickness, color);
            }

            public void Polygon(Vector2[] points, Color color)
            {
                if (points == null || points.Length < 3) return;

                float minX = points[0].x, maxX = points[0].x;
                float minY = points[0].y, maxY = points[0].y;
                foreach (var p in points)
                {
                    minX = Mathf.Min(minX, p.x);
                    maxX = Mathf.Max(maxX, p.x);
                    minY = Mathf.Min(minY, p.y);
                    maxY = Mathf.Max(maxY, p.y);
                }

                for (int y = Mathf.FloorToInt(minY); y <= Mathf.CeilToInt(maxY); y++)
                    for (int x = Mathf.FloorToInt(minX); x <= Mathf.CeilToInt(maxX); x++)
                        if (PointInPolygon(new Vector2(x + 0.5f, y + 0.5f), points))
                            Blend(x, y, color);
            }

            private static bool PointInPolygon(Vector2 point, Vector2[] polygon)
            {
                bool inside = false;
                for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
                {
                    Vector2 pi = polygon[i];
                    Vector2 pj = polygon[j];
                    bool intersect = ((pi.y > point.y) != (pj.y > point.y)) &&
                                     (point.x < (pj.x - pi.x) * (point.y - pi.y) /
                                      ((Mathf.Abs(pj.y - pi.y) < 0.00001f) ? 0.00001f : (pj.y - pi.y)) + pi.x);
                    if (intersect) inside = !inside;
                }
                return inside;
            }

            private void Blend(int x, int y, Color source)
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height || source.a <= 0) return;
                int index = y * Width + x;
                pixels[index] = AlphaOver(pixels[index], source);
            }

            private static Color AlphaOver(Color destination, Color source)
            {
                float outA = source.a + destination.a * (1f - source.a);
                if (outA <= 0.0001f) return Color.clear;

                return new Color(
                    (source.r * source.a + destination.r * destination.a * (1f - source.a)) / outA,
                    (source.g * source.a + destination.g * destination.a * (1f - source.a)) / outA,
                    (source.b * source.a + destination.b * destination.a * (1f - source.a)) / outA,
                    outA);
            }
        }
    }
}
#endif
