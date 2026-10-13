using UnityEngine;

namespace FemmeFatale.Save
{
    public interface ISaveable
    {
        string CaptureState();
        
        void RestoreState(string json);
    }
    
    public abstract class SaveableBehaviour<T> : MonoBehaviour, ISaveable
    {
        protected abstract T Capture();
        protected abstract void Restore(T state);

        public string CaptureState() => JsonUtility.ToJson(Capture());
        public void RestoreState(string json) => Restore(JsonUtility.FromJson<T>(json));
    }
}
