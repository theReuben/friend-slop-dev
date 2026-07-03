using System.IO;
using UnityEditor;
using UnityEngine;

namespace Friendslop.Reference.Editor
{
    /// <summary>
    /// One-click folder tree from framework/03-unity-conventions.md, so every
    /// game project starts identically. Drop into _Project/Code/Editor/, run
    /// menu Friendslop > Create Project Structure. Idempotent.
    /// </summary>
    public static class CreateProjectStructure
    {
        private static readonly string[] Folders =
        {
            "Assets/_Project/Art/Models",
            "Assets/_Project/Art/Materials",
            "Assets/_Project/Art/Textures",
            "Assets/_Project/Art/Animations",
            "Assets/_Project/Audio/SFX",
            "Assets/_Project/Audio/Music",
            "Assets/_Project/Audio/Ambience",
            "Assets/_Project/Code/Runtime",
            "Assets/_Project/Code/Editor",
            "Assets/_Project/Code/Tests",
            "Assets/_Project/Levels",
            "Assets/_Project/Prefabs",
            "Assets/_Project/Settings",
            "Assets/_Project/UI",
            "Assets/ThirdParty",
            "Tools",
        };

        [MenuItem("Friendslop/Create Project Structure")]
        public static void Run()
        {
            foreach (var folder in Folders)
            {
                if (Directory.Exists(folder)) continue;
                Directory.CreateDirectory(folder);
                // .gitkeep so empty folders survive the first commit
                File.WriteAllText(Path.Combine(folder, ".gitkeep"), "");
            }
            AssetDatabase.Refresh();
            Debug.Log("Friendslop project structure created (framework/03 layout).");
        }
    }
}
