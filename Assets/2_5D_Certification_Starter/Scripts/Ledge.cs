using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    [SerializeField]
    private Vector3 _handPos, _standPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ledge_Grabber")
        {
            Player playerController = other.transform.parent.GetComponent<Player>();

            if (playerController != null)
            {
                playerController.transform.parent = this.transform;
                playerController.GrabLedge(_handPos, this);
            }
            else 
            {
                Debug.LogError("Can't find Player script");
            }
        }
    }

    public Vector3 GetStandPos()
    {
        return _standPos;
    }

}
