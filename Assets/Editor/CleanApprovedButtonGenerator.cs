#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SurvivalGame.EditorTools
{
    /// <summary>
    /// Generates every approved HUD button independently.
    /// No sprite-sheet cropping is used.
    /// </summary>
    public static class CleanApprovedButtonGenerator
    {
        public const string Folder = "Assets/UI/Approved/Buttons";
        private const int Size = 512;

        private static readonly Color White = new Color(0.94f, 0.96f, 0.97f, 1f);
        private static readonly Color Dark = new Color(0.10f, 0.13f, 0.15f, 1f);
        private static readonly Color Cyan = new Color(0.16f, 0.82f, 0.90f, 1f);
        private static readonly Color Wood = new Color(0.62f, 0.40f, 0.22f, 1f);

        public static void GenerateAll(bool force = true)
        {
            EnsureFolder("Assets", "UI");
            EnsureFolder("Assets/UI", "Approved");
            EnsureFolder("Assets/UI/Approved", "Buttons");

            Write("action", DrawAction(), force);
            Write("aim", DrawAim(), force);
            Write("axe", DrawAxe(), force);
            Write("backpack", DrawBackpack(), force);
            Write("craft", DrawCraft(), force);
            Write("build", DrawBuild(), force);
            Write("menu", DrawMenu(), force);
            Write("map", DrawMap(), force);
            Write("chrome", CreateBase(), force);

            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

            string[] names =
            {
                "action", "aim", "axe", "backpack", "craft",
                "build", "menu", "map", "chrome"
            };

            foreach (string name in names)
                ConfigureImporter($"{Folder}/{name}.png");
        }

        private static Texture2D DrawAction()
        {
            var r = CreateBase();
            RoundedRect(r, 205, 215, 318, 340, 25, White);

            Finger(r, 192, 176);
            Finger(r, 221, 159);
            Finger(r, 250, 164);
            Finger(r, 279, 178);

            RoundedRect(r, 165, 250, 230, 283, 14, White);
            return r;
        }

        private static void Finger(Texture2D r, int x, int top)
        {
            RoundedRect(r, x, top, x + 26, 250, 12, White);
        }

        private static Texture2D DrawAim()
        {
            var r = CreateBase();

            Ring(r, 256, 256, 91, 11, White);
            Ring(r, 256, 256, 24, 8, White);

            Line(r, 256, 123, 256, 195, 9, White);
            Line(r, 256, 317, 256, 389, 9, White);
            Line(r, 123, 256, 195, 256, 9, White);
            Line(r, 317, 256, 389, 256, 9, White);

            Circle(r, 256, 256, 5, Cyan);
            return r;
        }

        private static Texture2D DrawAxe()
        {
            var r = CreateBase();

            Line(r, 205, 372, 295, 173, 22, new Color(0f, 0f, 0f, 0.42f));
            Line(r, 208, 367, 295, 176, 15, Wood);

            Polygon(r, new[]
            {
                new Vector2(260,145),
                new Vector2(350,154),
                new Vector2(382,197),
                new Vector2(320,252),
                new Vector2(272,236)
            }, White);

            Line(r, 278, 154, 345, 162, 5, new Color(1f,1f,1f,0.65f));
            return r;
        }

        private static Texture2D DrawBackpack()
        {
            var r = CreateBase();

            Arc(r, 256, 220, 76, 200f, 340f, 17, White);
            RoundedRect(r, 165, 183, 348, 365, 35, White);
            RoundedRect(r, 141, 215, 173, 338, 12, new Color(0.80f,0.83f,0.84f,1f));
            RoundedRect(r, 340, 215, 372, 338, 12, new Color(0.80f,0.83f,0.84f,1f));
            RoundedRect(r, 198, 250, 316, 330, 15, Dark);
            RoundedRect(r, 210, 265, 304, 313, 11, new Color(0.32f,0.38f,0.41f,1f));
            RoundedRect(r, 242, 183, 270, 213, 6, Dark);

            return r;
        }

        private static Texture2D DrawCraft()
        {
            var r = CreateBase();

            Line(r, 170, 345, 335, 180, 22, White);
            Circle(r, 170, 347, 23, White);
            RoundedRect(r, 315, 155, 367, 184, 9, White);

            Line(r, 172, 182, 340, 350, 20, White);
            RoundedRect(r, 146, 156, 216, 184, 9, White);
            Circle(r, 256, 256, 14, Cyan);

            return r;
        }

        private static Texture2D DrawBuild()
        {
            var r = CreateBase();

            Polygon(r, new[]
            {
                new Vector2(142,258),
                new Vector2(256,150),
                new Vector2(371,258),
                new Vector2(345,258),
                new Vector2(345,365),
                new Vector2(168,365),
                new Vector2(168,258)
            }, White);

            FillRect(r, 231, 283, 282, 365, Dark);
            FillRect(r, 190, 268, 223, 301, Dark);
            FillRect(r, 290, 268, 323, 301, Dark);

            return r;
        }

        private static Texture2D DrawMenu()
        {
            var r = CreateBase();

            RoundedRect(r, 165, 193, 347, 217, 12, White);
            RoundedRect(r, 165, 244, 347, 268, 12, White);
            RoundedRect(r, 165, 295, 347, 319, 12, White);

            return r;
        }

        private static Texture2D DrawMap()
        {
            var r = CreateBase();

            Polygon(r, new[]
            {
                new Vector2(160,175),
                new Vector2(227,155),
                new Vector2(285,175),
                new Vector2(352,152),
                new Vector2(352,352),
                new Vector2(285,372),
                new Vector2(227,352),
                new Vector2(160,374)
            }, White);

            Line(r, 227,155,227,352,9,Dark);
            Line(r, 285,175,285,372,9,Dark);

            Circle(r, 313, 217, 23, Cyan);
            Polygon(r, new[]
            {
                new Vector2(313,243),
                new Vector2(298,220),
                new Vector2(328,220)
            }, Cyan);
            Circle(r, 313, 217, 8, Dark);

            return r;
        }

        private static Texture2D CreateBase()
        {
            var t = NewTexture();

            // Soft black drop shadow.
            Circle(t, 260, 268, 198, new Color(0f,0f,0f,0.28f));

            // Metal rim.
            Circle(t, 256, 256, 198, new Color(0.19f,0.22f,0.24f,0.92f));
            Ring(t, 256, 256, 196, 8, new Color(0.67f,0.72f,0.74f,0.96f));

            // Semi-transparent dark face.
            Circle(t, 256, 256, 186, new Color(0.08f,0.11f,0.13f,0.57f));
            Ring(t, 256, 256, 184, 5, new Color(0.08f,0.72f,0.82f,0.82f));
            Circle(t, 256, 256, 174, new Color(0.08f,0.11f,0.13f,0.43f));
            Ring(t, 256, 256, 173, 3, new Color(0.44f,0.51f,0.53f,0.62f));

            // Restrained highlights.
            Arc(t, 256, 256, 190, 205f, 330f, 4, new Color(1f,1f,1f,0.38f));
            Arc(t, 256, 256, 179, 20f, 155f, 3, new Color(0.08f,0.78f,0.88f,0.35f));

            t.Apply();
            return t;
        }

        private static Texture2D NewTexture()
        {
            var t = new Texture2D(Size, Size, TextureFormat.RGBA32, false);
            t.name = "Approved HUD Button";
            var clear = new Color[Size * Size];
            t.SetPixels(clear);
            return t;
        }

        private static void Write(string name, Texture2D texture, bool force)
        {
            string assetPath = $"{Folder}/{name}.png";
            string diskPath = Path.Combine(Application.dataPath,
                $"UI/Approved/Buttons/{name}.png");

            if (force || !File.Exists(diskPath))
                File.WriteAllBytes(diskPath, texture.EncodeToPNG());

            Object.DestroyImmediate(texture);
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceSynchronousImport);
        }

        private static void ConfigureImporter(string path)
        {
            if (AssetImporter.GetAtPath(path) is not TextureImporter importer)
                return;

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.alphaIsTransparency = true;
            importer.mipmapEnabled = false;
            importer.filterMode = FilterMode.Bilinear;
            importer.textureCompression = TextureImporterCompression.CompressedHQ;
            importer.maxTextureSize = 512;
            importer.SaveAndReimport();
        }

        private static void EnsureFolder(string parent, string child)
        {
            string path = $"{parent}/{child}";
            if (!AssetDatabase.IsValidFolder(path))
                AssetDatabase.CreateFolder(parent, child);
        }

        private static void FillRect(Texture2D t, int x0, int y0, int x1, int y1, Color c)
        {
            for (int y = y0; y <= y1; y++)
                for (int x = x0; x <= x1; x++)
                    Blend(t, x, y, c);
        }

        private static void Circle(Texture2D t, int cx, int cy, int radius, Color c)
        {
            int rr = radius * radius;
            for (int y = cy - radius; y <= cy + radius; y++)
            {
                int dy = y - cy;
                for (int x = cx - radius; x <= cx + radius; x++)
                {
                    int dx = x - cx;
                    if (dx * dx + dy * dy <= rr)
                        Blend(t, x, y, c);
                }
            }
        }

        private static void Ring(Texture2D t, int cx, int cy, int radius, int thickness, Color c)
        {
            int outer = radius * radius;
            int innerRadius = Mathf.Max(0, radius - thickness);
            int inner = innerRadius * innerRadius;

            for (int y = cy - radius; y <= cy + radius; y++)
            {
                int dy = y - cy;
                for (int x = cx - radius; x <= cx + radius; x++)
                {
                    int dx = x - cx;
                    int d = dx * dx + dy * dy;
                    if (d <= outer && d >= inner)
                        Blend(t, x, y, c);
                }
            }
        }

        private static void RoundedRect(Texture2D t, int x0, int y0, int x1, int y1, int radius, Color c)
        {
            int left = x0 + radius;
            int right = x1 - radius;
            int bottom = y0 + radius;
            int top = y1 - radius;

            FillRect(t, left, y0, right, y1, c);
            FillRect(t, x0, bottom, x1, top, c);

            Circle(t, left, bottom, radius, c);
            Circle(t, right, bottom, radius, c);
            Circle(t, left, top, radius, c);
            Circle(t, right, top, radius, c);
        }

        private static void Line(Texture2D t, int x0, int y0, int x1, int y1, int width, Color c)
        {
            Vector2 a = new Vector2(x0, y0);
            Vector2 b = new Vector2(x1, y1);
            Vector2 ab = b - a;
            float lenSq = Mathf.Max(0.001f, ab.sqrMagnitude);
            float half = width * 0.5f;

            int minX = Mathf.FloorToInt(Mathf.Min(x0, x1) - half);
            int maxX = Mathf.CeilToInt(Mathf.Max(x0, x1) + half);
            int minY = Mathf.FloorToInt(Mathf.Min(y0, y1) - half);
            int maxY = Mathf.CeilToInt(Mathf.Max(y0, y1) + half);

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    Vector2 p = new Vector2(x, y);
                    float u = Mathf.Clamp01(Vector2.Dot(p - a, ab) / lenSq);
                    Vector2 nearest = a + ab * u;
                    if (Vector2.Distance(p, nearest) <= half)
                        Blend(t, x, y, c);
                }
            }
        }

        private static void Arc(Texture2D t, int cx, int cy, int radius, float start, float end, int width, Color c)
        {
            for (float angle = start; angle <= end; angle += 0.35f)
            {
                float rad = angle * Mathf.Deg2Rad;
                int x = Mathf.RoundToInt(cx + Mathf.Cos(rad) * radius);
                int y = Mathf.RoundToInt(cy + Mathf.Sin(rad) * radius);
                Circle(t, x, y, Mathf.Max(1, width / 2), c);
            }
        }

        private static void Polygon(Texture2D t, Vector2[] points, Color c)
        {
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
            {
                for (int x = Mathf.FloorToInt(minX); x <= Mathf.CeilToInt(maxX); x++)
                {
                    if (InsidePolygon(new Vector2(x + 0.5f, y + 0.5f), points))
                        Blend(t, x, y, c);
                }
            }
        }

        private static bool InsidePolygon(Vector2 p, Vector2[] polygon)
        {
            bool inside = false;

            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                Vector2 a = polygon[i];
                Vector2 b = polygon[j];

                bool intersects =
                    ((a.y > p.y) != (b.y > p.y)) &&
                    (p.x < (b.x - a.x) * (p.y - a.y) /
                    ((Mathf.Abs(b.y - a.y) < 0.0001f) ? 0.0001f : (b.y - a.y)) + a.x);

                if (intersects)
                    inside = !inside;
            }

            return inside;
        }

        private static void Blend(Texture2D t, int x, int y, Color src)
        {
            if (x < 0 || y < 0 || x >= Size || y >= Size)
                return;

            Color dst = t.GetPixel(x, y);
            float a = src.a + dst.a * (1f - src.a);

            if (a <= 0.0001f)
            {
                t.SetPixel(x, y, Color.clear);
                return;
            }

            Color result = new Color(
                (src.r * src.a + dst.r * dst.a * (1f - src.a)) / a,
                (src.g * src.a + dst.g * dst.a * (1f - src.a)) / a,
                (src.b * src.a + dst.b * dst.a * (1f - src.a)) / a,
                a);

            t.SetPixel(x, y, result);
        }
    }
}
#endif
