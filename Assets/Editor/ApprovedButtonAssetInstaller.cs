#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

namespace SurvivalGame.EditorTools
{
    /// <summary>
    /// Reconstructs the user-approved HUD button PNGs byte-for-byte from source chunks
    /// stored in the repository, verifies SHA-256, and imports them as Unity sprites.
    /// This intentionally never redraws or modifies the approved artwork.
    /// </summary>
    public static class ApprovedButtonAssetInstaller
    {
        public const string OutputFolder = "Assets/UI/Approved/Buttons";
        public const string SourceFolder = "Assets/UI/Approved/ExactSource";

        private static readonly Dictionary<string, string> ExpectedSha256 =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "action",   "111306d9d25818848fbfed4001fd7b2d38d9d22237740dfe4289e70e2e31d937" },
                { "aim",      "2be0f4c1cfbcdd97219b8afa75351d3e5b4d90371aa743e05f4222e4be5ba84f" },
                { "axe",      "b6e643a13d4b3141a9c91242274d5de7fa64d3656316f6d46c8911d67b6b32c0" },
                { "pickaxe",  "6a90b5c3589ba314b78b7905de47ab1262be28085365a17e1adb2af893c9d8f3" },
                { "backpack", "b38cda463222c059364ac9b2370cb3937aea373300f54089518dcec06f6dbc62" },
                { "craft",    "a929ec5193240b55bd23dad38bebccbd7cb5d9e1de5896e73572a2d511ec74e5" },
                { "build",    "b4f3ca6f3f442702a3e9b0ac4ead61e13bb8398a270e3d8210afc084df820a94" },
                { "map",      "1a00525e91b7e9677aa3801fcbd64ee13111e63c393d50824623dbd8269e373a" },
                { "menu",     "3e14df49071c9758bdc311577a049f837ea7154aaef1640ca94bff9f9f3bd619" },
                { "settings", "aabddb4445d3f67ad0c25a28137c1a992925acfbf53dee509ea88bbf189d9c4a" }
            };

        public static void EnsureInstalled()
        {
            EnsureFolder("Assets", "UI");
            EnsureFolder("Assets/UI", "Approved");
            EnsureFolder("Assets/UI/Approved", "Buttons");

            foreach (var pair in ExpectedSha256)
                EnsureOne(pair.Key, pair.Value);

            AssetDatabase.SaveAssets();
        }

        public static Sprite Load(string name)
        {
            EnsureInstalled();
            return AssetDatabase.LoadAssetAtPath<Sprite>($"{OutputFolder}/{name}.png");
        }

        private static void EnsureOne(string name, string expectedSha)
        {
            string assetPath = $"{OutputFolder}/{name}.png";
            string absolutePath = Path.Combine(Application.dataPath, $"UI/Approved/Buttons/{name}.png");

            if (!File.Exists(absolutePath) ||
                !string.Equals(HashFile(absolutePath), expectedSha, StringComparison.OrdinalIgnoreCase))
            {
                byte[] exactBytes = ReadExactSource(name);

                string actual = HashBytes(exactBytes);
                if (!string.Equals(actual, expectedSha, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidDataException(
                        $"Approved UI asset '{name}' failed SHA-256 validation. Expected {expectedSha}, got {actual}. " +
                        "The approved artwork will not be replaced by an approximation.");

                File.WriteAllBytes(absolutePath, exactBytes);
                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceSynchronousImport);
            }

            ConfigureImporter(assetPath);
        }

        private static byte[] ReadExactSource(string name)
        {
            string sourceDir = Path.Combine(Application.dataPath, $"UI/Approved/ExactSource/{name}");
            if (!Directory.Exists(sourceDir))
                throw new DirectoryNotFoundException($"Missing approved UI source directory: {sourceDir}");

            string[] chunks = Directory.GetFiles(sourceDir, "*.b64")
                .OrderBy(Path.GetFileName, StringComparer.Ordinal)
                .ToArray();

            if (chunks.Length == 0)
                throw new FileNotFoundException($"No source chunks found for approved UI asset '{name}'.");

            var base64 = new System.Text.StringBuilder();
            foreach (string chunk in chunks)
                base64.Append(File.ReadAllText(chunk).Trim());

            return Convert.FromBase64String(base64.ToString());
        }

        private static void ConfigureImporter(string assetPath)
        {
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (!importer) return;

            bool changed = false;

            if (importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                changed = true;
            }

            if (importer.spriteImportMode != SpriteImportMode.Single)
            {
                importer.spriteImportMode = SpriteImportMode.Single;
                changed = true;
            }

            if (!importer.alphaIsTransparency)
            {
                importer.alphaIsTransparency = true;
                changed = true;
            }

            if (importer.mipmapEnabled)
            {
                importer.mipmapEnabled = false;
                changed = true;
            }

            if (importer.filterMode != FilterMode.Bilinear)
            {
                importer.filterMode = FilterMode.Bilinear;
                changed = true;
            }

            if (importer.textureCompression != TextureImporterCompression.CompressedHQ)
            {
                importer.textureCompression = TextureImporterCompression.CompressedHQ;
                changed = true;
            }

            if (importer.maxTextureSize != 512)
            {
                importer.maxTextureSize = 512;
                changed = true;
            }

            if (changed)
                importer.SaveAndReimport();
        }

        private static string HashFile(string path)
        {
            return HashBytes(File.ReadAllBytes(path));
        }

        private static string HashBytes(byte[] bytes)
        {
            using var sha = SHA256.Create();
            return BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", "").ToLowerInvariant();
        }

        private static void EnsureFolder(string parent, string child)
        {
            string path = $"{parent}/{child}";
            if (!AssetDatabase.IsValidFolder(path))
                AssetDatabase.CreateFolder(parent, child);
        }
    }
}
#endif
