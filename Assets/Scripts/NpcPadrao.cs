using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcPadrao : MonoBehaviour
{
    [SerializeField] Transform[] caminhos;
    [SerializeField] float TempoDeEspera;
    public Animator animFala;
    public Dialogue dialogue;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CorotinaDeMovimento());
        animFala = GameObject.FindGameObjectWithTag("GUI/Dialogue").GetComponent<Animator>();
    }

    IEnumerator CorotinaDeMovimento()
    {
        for (int i = 0; i <= caminhos.Length; i++)
        {
            Debug.Log(i);
            yield return StartCoroutine(MoveNpc(i));
            if(i >= caminhos.Length - 1)
            {
                i = -1;
            }
        }

    }
   

    IEnumerator MoveNpc(int numCaminho)
    {
        GetComponent<NavMeshAgent>().destination = caminhos[numCaminho].position;
        yield return new WaitForSeconds(TempoDeEspera);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetButtonDown("Fire1") && FindObjectOfType<DialogueManager>().isSpeaking == false) 
        {
            
            FindObjectOfType<DialogueManager>().awakedialogue(dialogue);
            animFala.SetTrigger("DialogueShow");
            animFala.ResetTrigger("DialogueHide");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            animFala.SetTrigger("DialogueHide");
            animFala.ResetTrigger("DialogueShow");
            FindObjectOfType<DialogueManager>().isSpeaking = false;
        }
    }
}
