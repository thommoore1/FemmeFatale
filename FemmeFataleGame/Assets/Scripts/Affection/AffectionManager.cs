using System;
using System.Collections.Generic;
using UnityEngine;

public class AffectionManager : MonoBehaviour
{
    public const float Min = -100f;
    public const float Max = 100f;

    public static AffectionManager Instance { get; private set; }

    /// <summary>(id, oldValue, newValue)</summary>
    public event Action<string, float, float> OnAffectionChanged;

    private readonly Dictionary<string, float> _values = new Dictionary<string, float>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        Debug.Log("Affection");
        if (Instance != null) return;
        var go = new GameObject("AffectionManager");
        go.AddComponent<AffectionManager>(); 
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public float Get(string id, float defaultValue = 0f)
    {
        return _values.TryGetValue(id, out var v) ? v : defaultValue;
    }
    
    public float GetNormalized(string id, float defaultValue = 0f)
    {
        return Get(id, defaultValue) / Max;
    }

    public bool Has(string id) => _values.ContainsKey(id);
    
    public void Set(string id, float value)
    {
        value = Mathf.Clamp(value, Min, Max);
        float old = Get(id);
        _values[id] = value;
        if (!Mathf.Approximately(old, value))
            OnAffectionChanged?.Invoke(id, old, value);
    }
    
    public float Add(string id, float amount, float defaultValue = 0f)
    {
        float newValue = Mathf.Clamp(Get(id, defaultValue) + amount, Min, Max);
        Set(id, newValue);
        return newValue;
    }
    public void InitializeIfMissing(string id, float startingValue)
    {
        if (!_values.ContainsKey(id)) _values[id] = Mathf.Clamp(startingValue, Min, Max);
    }

    // ---------- Save / Load ----------

    [Serializable]
    private class SaveData
    {
        public List<string> ids = new List<string>();
        public List<float> values = new List<float>();
    }

    /// <summary>Returns a JSON string you can drop into your save file.</summary>
    public string ToJson()
    {
        var data = new SaveData();
        foreach (var kv in _values)
        {
            data.ids.Add(kv.Key);
            data.values.Add(kv.Value);
        }
        return JsonUtility.ToJson(data);
    }

    public void FromJson(string json)
    {
        _values.Clear();
        var data = JsonUtility.FromJson<SaveData>(json);
        if (data == null) return;
        for (int i = 0; i < data.ids.Count && i < data.values.Count; i++)
            _values[data.ids[i]] = Mathf.Clamp(data.values[i], Min, Max);
    }
}
