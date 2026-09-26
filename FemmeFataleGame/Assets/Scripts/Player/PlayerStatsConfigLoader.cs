using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace FemmeFatale
{
    public class PlayerStatsConfigLoader : MonoBehaviour
    {
        [Tooltip("Folder, relative to Assets/, the config file lives in.")]
        [SerializeField] private string folderName = "Config";

        [Tooltip("File name inside that folder.")]
        [SerializeField] private string fileName = "PlayerStatsConfig.json";

        [Tooltip("How often (seconds) to check whether the file changed on disk.")]
        [SerializeField] private float pollIntervalSeconds = 0.5f;
        
        public event Action<PlayerStatsConfig> ConfigChanged;
        
        public PlayerStatsConfig Config { get; private set; } = new PlayerStatsConfig();

        public string FilePath { get; private set; }

        private DateTime _lastWriteTimeUtc;

        private void Awake()
        {
            FilePath = Path.Combine(Application.dataPath, folderName, fileName);
            EnsureFileExists();
            LoadFromDisk(logReload: false);
        }

        private void OnEnable()
        {
            StartCoroutine(PollForChanges());
        }

        private IEnumerator PollForChanges()
        {
            var wait = new WaitForSeconds(pollIntervalSeconds);
            while (true)
            {
                yield return wait;
                CheckAndReloadIfChanged();
            }
        }

        private void EnsureFileExists()
        {
            if (File.Exists(FilePath))
            {
                return;
            }

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
                string json = JsonUtility.ToJson(Config, prettyPrint: true);
                File.WriteAllText(FilePath, json);
                Debug.Log($"[PlayerMovementConfigLoader] No config file found - wrote defaults to {FilePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[PlayerMovementConfigLoader] Failed to write default config: {e}");
            }
        }

        private void CheckAndReloadIfChanged()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    return;
                }

                DateTime writeTime = File.GetLastWriteTimeUtc(FilePath);
                if (writeTime != _lastWriteTimeUtc)
                {
                    LoadFromDisk(logReload: true);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[PlayerMovementConfigLoader] Could not check config file: {e}");
            }
        }

        private void LoadFromDisk(bool logReload)
        {
            try
            {
                string json = File.ReadAllText(FilePath);
                PlayerStatsConfig loaded = JsonUtility.FromJson<PlayerStatsConfig>(json);

                if (loaded == null)
                {
                    Debug.LogWarning("[PlayerMovementConfigLoader] Config file parsed to null - keeping previous values.");
                    return;
                }

                Config = loaded;
                _lastWriteTimeUtc = File.GetLastWriteTimeUtc(FilePath);

                if (logReload)
                {
                    Debug.Log("[PlayerMovementConfigLoader] Reloaded player movement config from disk.");
                }

                ConfigChanged?.Invoke(Config);
            }
            catch (Exception e)
            {
                Debug.LogError($"[PlayerMovementConfigLoader] Failed to read/parse config at {FilePath}: {e}");
            }
        }

        public void ReloadNow() => LoadFromDisk(logReload: true);
    }
}