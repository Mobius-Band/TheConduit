﻿using System;
using UnityEngine;

namespace Combat
{
    /// <summary>
    /// This class is used to store the data of a single attack.
    /// </summary>
    [Serializable]
    public class Attack
    {
        public int damage;
        public Vector3 hitboxPosition;
        public Vector3 hitboxSize;
        public float stepAmount;
        public float stepDuration;
        public int knockbackAmount;

        //public Animation animation;
    }
}