using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShipController : MonoBehaviour
{

    public float bobHeight = .25f;
    public float bobSpeed = .25f;

    private float originalYpos = 0;

    void Start()
    {
        originalYpos = this.transform.localPosition.y;
    }

    void Update()
    {
        ShipBobbing();
    }

    void ShipBobbing()
    {
        this.transform.localPosition = new Vector3(this.transform.localPosition.x , Mathf.Sin(Time.time * bobSpeed) * bobHeight + originalYpos, this.transform.localPosition.z);
    }
}
