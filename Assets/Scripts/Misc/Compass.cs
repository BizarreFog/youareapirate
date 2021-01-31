using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public Transform currentTarget;

    void Update()
    {
        if(GameManager.Instance.currentFavored != null)
        {
            currentTarget = GameManager.Instance.currentFavored.transform;
        }

        if(currentTarget != null)
        {
            Vector3 dir = this.transform.position - currentTarget.position;
            Vector3 needleDir = Vector3.ProjectOnPlane(dir, this.transform.up);
            this.transform.LookAt(currentTarget.position);
        }
    }
}
