using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.RegisterChest(this);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>() != null)
        {
            GameManager.Instance.ChestFound(this);
            Destroy(this.gameObject);
        }
    }
}
