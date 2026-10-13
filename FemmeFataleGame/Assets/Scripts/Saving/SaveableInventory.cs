using System;
using System.Collections.Generic;
using FemmeFatale;
using UnityEngine;

namespace FemmeFatale.Save
{
    [RequireComponent(typeof(InventoryController))]
    public class SaveableInventory : SaveableBehaviour<SaveableInventory.State>
    {
        [Serializable]
        public class State
        {
            public List<string> slotItemIds = new List<string>();
            public int selectedIndex = -1;
        }

        private InventoryController _inventory;
        private InventoryController Inventory
        {
            get
            {
                if (_inventory == null) _inventory = GetComponent<InventoryController>();
                return _inventory;
            }
        }

        protected override State Capture() => new State
        {
            slotItemIds = Inventory.CaptureSlotItemIds(),
            selectedIndex = Inventory.GetSelectedIndex()
        };

        protected override void Restore(State state)
        {
            Inventory.RestoreSlotItemIds(state.slotItemIds, state.selectedIndex);
        }
    }
}
