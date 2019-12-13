using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanEmbarkHandler : MonoBehaviour
{
    [SerializeField] private float embarkRadius;
    [SerializeField] private KeyCode embarkKey;
    [SerializeField] private LayerMask detectionMask;
    bool embarkLock = false;

    private void OnEnable()
    {
        embarkLock = false;
    }

    void Update()
    {

    }

    private void EmbarkTitan()
    {

    }
}
