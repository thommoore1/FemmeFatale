using UnityEngine;

namespace FemmeFatale
{
    public class InventoryItemScale : MonoBehaviour
    {
        public Vector3 OriginalScale { get; private set; } = Vector3.one;

        public void SetOriginalScale(Vector3 scale)
        {
            OriginalScale = scale;
        }
    }
}