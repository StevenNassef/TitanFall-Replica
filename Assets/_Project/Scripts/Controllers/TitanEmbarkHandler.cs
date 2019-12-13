using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
public class TitanEmbarkHandler : MonoBehaviour
{
    [SerializeField] private float embarkRadius;
    [SerializeField] private KeyCode embarkKey;
    [SerializeField] private Vector3 playerDisembarkPositon = new Vector3(0, 4, 3);
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private GameObject titanComponents;
    [SerializeField] private GameObject titanGFX;
    [MustBeAssigned] [SerializeField] private GameObject player;
    private TitanStatsHandler titanStatsHandler;
    private TitanFirstPersonController titanFirstPersonController;
    bool embarkLock = false;
    bool embarked = false;

    private void Awake()
    {
    }
    private void OnEnable()
    {
        embarkLock = false;
        titanStatsHandler = GetComponent<TitanStatsHandler>();
        titanFirstPersonController = GetComponent<TitanFirstPersonController>();
        titanStatsHandler.enabled = false;
        titanFirstPersonController.enabled = false;
        titanComponents.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(embarkKey))
        {
            if (!embarkLock)
            {
                if (embarked)
                {
                    embarkLock = true;
                    DisembarkTitan();
                }
                else
                {
                    RaycastHit hitInfo;
                    if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hitInfo, embarkRadius, detectionMask))
                    {
                        if (hitInfo.collider.gameObject.CompareTag("Player"))
                        {
                            embarkLock = true;
                            EmbarkTitan();
                        }
                    }
                }
            }
        }
        else
        {
            embarkLock = false;
        }
    }

    private void EmbarkTitan()
    {
        titanGFX.SetActive(false);
        player.SetActive(false);
        player.transform.SetParent(transform);
        player.transform.localPosition = Vector3.zero;
        titanStatsHandler.enabled = true;
        titanFirstPersonController.enabled = true;
        titanComponents.SetActive(true);
        embarked = true;
    }

    private void DisembarkTitan()
    {
        player.transform.localPosition = playerDisembarkPositon;
        player.SetActive(true);
        titanStatsHandler.enabled = false;
        titanFirstPersonController.enabled = false;
        titanComponents.SetActive(false);
        titanGFX.SetActive(true);
        embarked = false;
    }
}
