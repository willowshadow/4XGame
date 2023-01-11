using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Environment
{
    public class Asteroid : MonoBehaviour
    {
        public void Start()
        {
            var random = Random.insideUnitSphere;
            transform.DORotate(random, .11f, RotateMode.FastBeyond360).SetLoops(-1,LoopType.Incremental);
        }
    }
}