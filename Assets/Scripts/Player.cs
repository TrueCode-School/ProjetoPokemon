using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    #region Variáveis
    [SerializeField]CharacterController ccPlayer;
    [SerializeField]float velocidade;
    [SerializeField]Animator animPlayer;
    [SerializeField] Vector3 input;
    [SerializeField] bool isMoving;
    [SerializeField] float[] TimeForEncounter = new float[2];
    [SerializeField] float timer;

    public event Action OnEncountered;

    #endregion
    // Start is called before the first frame update
    
    void Start()
    {  
        ccPlayer = GetComponent<CharacterController>();
        animPlayer = GetComponent<Animator>();
        timer = UnityEngine.Random.Range(TimeForEncounter[0], TimeForEncounter[1]);
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        movimento();
        animacao();
    }
    #region Funções
        #region Movimento
    void movimento()
    {
        ccPlayer.SimpleMove(Physics.gravity);
        //Vector3 movimento = new Vector3(Input.GetAxisRaw("Horizontal") * velocidade,0,(Input.GetAxisRaw("Vertical") * velocidade)*2);
        if(Input.GetButton("Vertical"))
        {
            input.z = Input.GetAxisRaw("Vertical");
            input.x = 0;
        }
        else if(Input.GetButton("Horizontal"))
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.z = 0;
        }
        else
        {
            input.x = 0;
            input.z = 0;
        }
        Vector3 movimento = new Vector3(input.x * velocidade, input.y, (input.z * velocidade) * 2);
        //Debug.Log(Input.GetAxis("Horizontal"));
        //Debug.Log(Input.GetAxisRaw("Horizontal"));
        ccPlayer.Move(movimento * Time.deltaTime);
    }
    #endregion
        #region animação
    void animacao()
    {
        if(Input.GetButton("Vertical")||Input.GetButton("Horizontal"))
        {
            animPlayer.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
            animPlayer.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
        }

        //Se Movendo ou Parado
        if(Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0)
        {
            animPlayer.SetBool("IsMoving", false);
            isMoving = false;
        }
        else
        {
            animPlayer.SetBool("IsMoving", true);
            isMoving = true;
        }
    }
    #endregion
        #region OnMoveOver
    void OnMoveOver()
    {
        var colliders = Physics.OverlapSphere(transform.position, 0.5f, GameLayers.i.TriggerableLayers);

    }


        #endregion

    #endregion
    #region Encounter
    private void OnTriggerStay(Collider other)
    {
        /* LongGrass - 
            Charmander - 1%;  */
        #region LongGrass
        if (other.gameObject.tag == "Terrains/LongGrass" && isMoving == true)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                switch(UnityEngine.Random.Range(1,11))
                {
                    case 11:
                    case 10:
                    case 9:
                    case 8:
                    case 7:
                    case 6:
                        Debug.Log("Você encontrou um Ratata");
                        break;
                    case 5:
                        break;
                    case 4:
                        break;
                    case 3:
                        break;
                    case 2: 
                        Debug.Log("Bulbasour Encontrado");
                        OnEncountered();
                        timer = UnityEngine.Random.Range(TimeForEncounter[0], TimeForEncounter[1]);
                        break;
                    case 1:
                        int pokeRandom = 0;
                        pokeRandom = UnityEngine.Random.Range(1, 3);
                        if(pokeRandom == 1)
                            Debug.Log("Você Achou um Charmander");
                        if (pokeRandom == 2)
                            Debug.Log("Você Achou um Squirtle");
                        if (pokeRandom == 3)
                            Debug.Log("Você Achou um Zubat Gold");

                        timer = UnityEngine.Random.Range(TimeForEncounter[0], TimeForEncounter[1]);
                        break;

                }
            }
        }

        
    }
        #endregion


        
    #endregion
}
