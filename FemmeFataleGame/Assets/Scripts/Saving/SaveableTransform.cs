using UnityEngine;
using System;

namespace FemmeFatale.Save
{
    public class SaveableTransform : SaveableBehaviour<SaveableTransform.State>
    {
        [Serializable]
        public struct State
        {
            public Vector3 position;
            public Quaternion rotation;
            public bool active;
        }

        protected override State Capture() => new State
        {
            position = transform.position,
            rotation = transform.rotation,
            active = gameObject.activeSelf
        };

        protected override void Restore(State s)
        {
            var cc = GetComponent<PlayerController>();
            if (cc != null) cc.enabled = false;

            transform.SetPositionAndRotation(s.position, s.rotation);

            if (cc != null) cc.enabled = true;

            // For monsters, call agent.Warp(s.position) here instead.
            gameObject.SetActive(s.active);
        }
    }
}