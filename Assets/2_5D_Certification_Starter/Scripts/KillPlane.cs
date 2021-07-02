using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Respawn();
            }

            CharacterController cc = other.GetComponent<CharacterController>();

            if (cc != null)
            {
                cc.enabled = false;
                StartCoroutine(EnableControllerRoutine(cc));
            }
        }
    }

    IEnumerator EnableControllerRoutine(CharacterController controller)
    {
        yield return new WaitForSeconds(0.5f);
        controller.enabled = true;
    }
}
