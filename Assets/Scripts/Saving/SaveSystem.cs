using System.IO;
using UnityEngine;

namespace Potato.Saving
{
    public class SaveSystem : MonoBehaviour
    {
        public static SaveSystem Instance { get; private set; }

        private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Save(SaveData data)
        {
            data.lastSaveTimestamp = GetUtcSeconds();
            File.WriteAllText(SavePath, JsonUtility.ToJson(data, true));
        }

        public SaveData Load()
        {
            if (!File.Exists(SavePath)) return new SaveData();
            string json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<SaveData>(json) ?? new SaveData();
        }

        private static double GetUtcSeconds() =>
            (System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalSeconds;
    }
}
