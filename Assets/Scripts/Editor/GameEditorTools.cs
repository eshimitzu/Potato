using System.IO;
using UnityEditor;
using UnityEngine;

namespace Potato.Editor
{
    public static class GameEditorTools
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

        [MenuItem("Potato/Reset Progress")]
        private static void ResetProgress()
        {
            if (!EditorUtility.DisplayDialog(
                "Reset Progress",
                $"Delete save file?\n{SavePath}",
                "Delete", "Cancel"))
                return;

            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
                Debug.Log("[Potato] Save file deleted.");
            }
            else
            {
                Debug.Log("[Potato] No save file found.");
            }
        }
    }
}
