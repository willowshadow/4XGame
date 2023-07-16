using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Environment
{
    public class Asteroid : MonoBehaviour
    {
        public float rotationSpeed;

        private void Awake()
        {
            var startRot = Random.rotation;
            transform.rotation = startRot;
        }

        public void Start()
        {
            var random = Random.insideUnitSphere;
            var rotDif = random - transform.eulerAngles;
            
            transform.DORotate(random, rotDif.magnitude/rotationSpeed, RotateMode.FastBeyond360).SetLoops(-1,LoopType.Incremental);
        }
    }
}