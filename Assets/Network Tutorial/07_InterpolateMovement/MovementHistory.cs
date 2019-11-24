using ECSish;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MovementHistory : MonoBehaviourComponentData
{
    public int maxRecords;
    public List<Data> movementHistory = new List<Data>();

    [Serializable]
    public class Data
    {
        public float time;
        public Vector3 position;
        public Quaternion rotation = Quaternion.identity;
        public Vector3 scale = Vector3.one;
        public Vector3 velocity;
        public Vector3 angularVelocity;
    }
}