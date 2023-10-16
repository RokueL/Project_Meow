using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class Stats
    {
        public float HP = 100f;
        public float Speed = 2f;
        public float Damage = 15f;
    }

    public class CoreType
    {
        public enum coreType
        {
            A,
            B,
            C
        }
    }
}
