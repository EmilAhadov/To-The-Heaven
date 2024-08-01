using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDown : MonoBehaviour
{
    public string oneWayPlatformLayerName = "Building";
    public string playerLayerName = "Player";

    [SerializeField] private float _duration;
    private bool _coroutineGateKeeper = true;
    private void Update()
    {
        Fall();
    }

    public void Fall()
    {
        if (GetComponent<InputTesting>().fall)
        {
            StartCoroutine(TemporariyIgnorLayerMask());
        }
    }

    IEnumerator TemporariyIgnorLayerMask()
    {
        //_coroutineGateKeeper = false;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(playerLayerName), LayerMask.NameToLayer(oneWayPlatformLayerName), true);
        yield return new WaitForSeconds(_duration);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(playerLayerName), LayerMask.NameToLayer(oneWayPlatformLayerName), false);
        //_coroutineGateKeeper = true;
    }
}