using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Battle};

public class GameController : MonoBehaviour
{
    [SerializeField] Player playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    GameState state;

    void Start()
    {
        state = GameState.FreeRoam;
        playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
    }


    void Update()
    {
        if(state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if(state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
    }

    void StartBattle()
    {
        state = GameState.Battle;
        playerController.gameObject.SetActive(false);
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<PokemonParty>();
        var wildPokemon = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildPokemon();

        battleSystem.StartBattle(playerParty,wildPokemon);



    }

    void EndBattle(bool won)
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        playerController.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(true);
    }
}
