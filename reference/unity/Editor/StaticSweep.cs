using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Friendslop.Reference.Editor
{
    /// <summary>
    /// The QA static sweep (framework/08): scans every scene in Build Settings
    /// plus every prefab under Assets/_Project for missing scripts and null
    /// serialized object references. Run before every gate:
    ///   menu Friendslop -> Static Sweep, or headless:
    ///   Unity -batchmode -executeMethod Friendslop.Reference.Editor.StaticSweep.Run
    /// Exit code / log line "SWEEP FAILED" is the machine-readable result.
    /// </summary>
    public static class StaticSweep
    {
        [MenuItem("Friendslop/Static Sweep")]
        public static void Run()
        {
            var report = new StringBuilder();
            int issues = 0;

            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled) continue;
                var opened = EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Single);
                foreach (var root in opened.GetRootGameObjects())
                    issues += ScanHierarchy(root, scene.path, report);
            }

            foreach (var guid in AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/_Project" }))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab != null) issues += ScanHierarchy(prefab, path, report);
            }

            if (issues > 0) Debug.LogError($"SWEEP FAILED — {issues} issue(s):\n{report}");
            else Debug.Log("SWEEP PASSED — no missing scripts or references.");
            if (Application.isBatchMode) EditorApplication.Exit(issues > 0 ? 1 : 0);
        }

        private static int ScanHierarchy(GameObject root, string context, StringBuilder report)
        {
            int issues = 0;
            foreach (var t in root.GetComponentsInChildren<Transform>(true))
            {
                foreach (var c in t.GetComponents<Component>())
                {
                    if (c == null) // missing script
                    {
                        report.AppendLine($"[missing script] {context} :: {Path(t)}");
                        issues++;
                        continue;
                    }
                    if (c is not MonoBehaviour) continue;

                    var so = new SerializedObject(c);
                    var prop = so.GetIterator();
                    while (prop.NextVisible(true))
                    {
                        if (prop.propertyType == SerializedPropertyType.ObjectReference
                            && prop.objectReferenceValue == null
                            && prop.objectReferenceInstanceIDValue != 0) // broken ref, not intentionally-empty
                        {
                            report.AppendLine($"[broken ref] {context} :: {Path(t)}.{c.GetType().Name}.{prop.name}");
                            issues++;
                        }
                    }
                }
            }
            return issues;
        }

        private static string Path(Transform t) =>
            t.parent == null ? t.name : Path(t.parent) + "/" + t.name;
    }
}
