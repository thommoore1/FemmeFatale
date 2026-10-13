using System;
using System.IO;
using UnityEngine;

namespace FemmeFatale.Save
{
    public static class SaveFileIO
    {
        private const string Extension = ".sav";

        private static string Directory_ => Path.Combine(Application.persistentDataPath, "Saves");
        public static string GetPath(int slot) => Path.Combine(Directory_, $"slot{slot}{Extension}");

        public static bool Exists(int slot) => File.Exists(GetPath(slot));

        public static bool Write(int slot, SaveFile file)
        {
            try
            {
                Directory.CreateDirectory(Directory_);
                string path = GetPath(slot);
                string tmp = path + ".tmp";
                
                File.WriteAllText(tmp, JsonUtility.ToJson(file, true));

                if (File.Exists(path)) File.Copy(path, path + ".bak", true);
                File.Copy(tmp, path, true);
                File.Delete(tmp);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Save] Failed to write slot {slot}: {e}");
                return false;
            }
        }

        public static bool TryRead(int slot, out SaveFile file)
        {
            string path = GetPath(slot);
            return TryReadPath(path, out file) || TryReadPath(path + ".bak", out file);
        }

        private static bool TryReadPath(string path, out SaveFile file)
        {
            file = null;
            if (!File.Exists(path)) return false;
            try
            {
                file = JsonUtility.FromJson<SaveFile>(File.ReadAllText(path));
                return file != null;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Save] Failed to read '{path}': {e}");
                return false;
            }
        }

        public static void Delete(int slot)
        {
            string path = GetPath(slot);
            foreach (var p in new[] { path, path + ".bak", path + ".tmp" })
                if (File.Exists(p)) File.Delete(p);
        }
    }
}
