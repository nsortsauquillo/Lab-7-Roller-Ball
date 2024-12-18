using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Award : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("mlagent"))
        {
            Invoke("MoveInitialPosition", 4);
        }
    }
    private void MoveInitialPosition()
    {
        bool positionFound = false;
        int attempts = 100;
        Vector3 positionPotential = Vector3.zero;
        while (!positionFound && attempts >= 0)
        {
            attempts--;
            positionPotential = new Vector3(transform.parent.position.x + UnityEngine.Random.Range(-3f, 3f), 0.555f, transform.parent.position.z + UnityEngine.Random.Range(-3f, 3f));

            Collider[] colliders = Physics.OverlapSphere(positionPotential, 0.05f);
            if (colliders.Length == 0)
            {
                transform.position = positionPotential;
                positionFound = true;
            }
        }
    }
}