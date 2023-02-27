using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IPlayerTriggerable
{
    public void OnPlayerTriggered(Player player)
    {
        Debug.Log("Entrou no portal!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Entrou no portal!");
        }
    }
    
}
