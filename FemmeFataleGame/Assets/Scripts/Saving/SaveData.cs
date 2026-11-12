using System;
using System.Collections.Generic;

namespace FemmeFatale.Save
{
    [Serializable]
    public class ComponentRecord
    {
        public string key;
        public string json;
    }
    
    [Serializable]
    public class EntityRecord
    {
        public string id;
        public string prefabId;
        public List<ComponentRecord> components = new List<ComponentRecord>();
    }
    
    [Serializable]
    public class SaveFile
    {
        public const int CurrentVersion = 1;
        public int version = CurrentVersion;
        public string sceneName;
        public string savedAtUtc;
        public List<EntityRecord> entities = new List<EntityRecord>();

        public InventorySaveData inventory;
        
        public string affectionJson;
    }
    
    [Serializable]
    public class InventorySaveData
    {
        public List<string> itemIds = new List<string>();
        public int selectedIndex;
    }
}
