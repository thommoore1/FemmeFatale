using System;
using UnityEngine;

namespace FemmeFatale.Save
{
    [RequireComponent(typeof(MonsterAffection))]
    public class SaveableMonster : SaveableEntity
    {
        private const string AffectionKey = "MonsterAffection";

        [Serializable]
        private class MonsterData
        {
            public string affectionId;
            public float value;
            public int patrolIndex;
        }

        private MonsterAffection _affection;
        private MonsterAffection Affection =>
            _affection != null ? _affection : (_affection = GetComponent<MonsterAffection>());

        public override EntityRecord Capture()
        {
            var record = base.Capture();

            var monsterAI = GetComponent<MonsterAI>();

            var data = new MonsterData
            {
                affectionId = Affection.AffectionId,
                value = Affection.Affection,
                patrolIndex = monsterAI != null ? monsterAI.PatrolIndex : 0
            };

            record.components.Add(new ComponentRecord
            {
                key = AffectionKey,
                json = JsonUtility.ToJson(data)
            });
            return record;
        }

        public override void Restore(EntityRecord record)
        {
            base.Restore(record);

            foreach (var comp in record.components)
            {
                if (comp.key != AffectionKey) continue;

                var data = JsonUtility.FromJson<MonsterData>(comp.json);
                if (data == null || string.IsNullOrEmpty(data.affectionId)) return;

                AffectionManager.Instance.Set(data.affectionId, data.value);

                var monsterAI = GetComponent<MonsterAI>();
                if (monsterAI != null)
                {
                    monsterAI.PatrolIndex = data.patrolIndex;
                }

                return;
            }
        }
    }
}