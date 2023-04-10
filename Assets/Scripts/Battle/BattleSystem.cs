using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy, PartyScreen}

public class BattleSystem : MonoBehaviour
{
    #region Variables
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] PartyScreen partyScreen;

    public event Action<bool> OnBattleOver;

    BattleState state;
    int currentAction;
    int currentMove;
    int currentMember;

    PokemonParty playerParty;
    Pokemon wildPokemon;

    #endregion

    // Start is called before the first frame update
    public void StartBattle(PokemonParty playerParty, Pokemon wildPokemon)
    {
        this.playerParty = playerParty;
        this.wildPokemon = wildPokemon;
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.Setup(playerParty.GetHealthyPokemon());
        enemyUnit.Setup(wildPokemon);
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);

        partyScreen.Init();

        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);

        yield return dialogBox.TypeDialog($"A Wild Pokemon {enemyUnit.Pokemon.Base.Name} appeared!!!");
        yield return new WaitForSeconds(1f);
        yield return dialogBox.TypeDialog("Choose an Action");
        yield return new WaitForSeconds(0.5f);
        PlayerAction();

        
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        dialogBox.EnableActionSelector(true);
    }

    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    void PartyScreen()
    {
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Pokemons);
        partyScreen.gameObject.SetActive(true);

    }

    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;
        enemyHud.HpCounterpart1();
        var move = playerUnit.Pokemon.Moves[currentMove];
        move.PP--;
        yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} used {move.Base.Name}");
        yield return new WaitForSeconds(1f);

        playerUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(0.1f);
        enemyUnit.PlayHitAnimation();

        var damageDetails = enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
        yield return enemyHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        yield return new WaitForSeconds(1f);
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} fainted.");
            enemyUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(1.5f);
            OnBattleOver(true);
        }
        else
        {
            StartCoroutine(EnemyMove());
        }

    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;
        playerHud.HpCounterpart1();
        var move = enemyUnit.Pokemon.GetRandomMove();
        move.PP--;
        yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} used {move.Base.Name}");
        yield return new WaitForSeconds(1f);

        enemyUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(0.1f);
        playerUnit.PlayHitAnimation();

        var damageDetails = playerUnit.Pokemon.TakeDamage(move, enemyUnit.Pokemon);
        yield return playerHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        yield return new WaitForSeconds(1f);
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} fainted.");
            playerUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(1.5f);
            var nextPokemon = playerParty.GetHealthyPokemon();
            if(nextPokemon != null)
            {
                playerUnit.Setup(nextPokemon);
                playerHud.SetData(nextPokemon);

                dialogBox.SetMoveNames(nextPokemon.Moves);

                yield return dialogBox.TypeDialog($"Go {nextPokemon.Base.name}!");
                yield return new WaitForSeconds(1f);
                yield return dialogBox.TypeDialog("Choose an Action");
                yield return new WaitForSeconds(1f);
                PlayerAction();

            }
            else
            {
                OnBattleOver(true);
            }
            

        }
        else
        {
            PlayerAction();
            dialogBox.EnableDialogText(false);
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if(damageDetails.Critical > 1f)
        {
            yield return dialogBox.TypeDialog("A critical hit!");
        }
        if(damageDetails.TypeEffectiveness > 1f)
        {
            yield return dialogBox.TypeDialog("It's super effective!");
        }else if(damageDetails.TypeEffectiveness < 1f)
        {
            yield return dialogBox.TypeDialog("It's not super effective!");
        }

    }

    public void HandleUpdate()
    {
        if(state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        if(state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
        if(state == BattleState.PartyScreen)
        {
            HandlePartySelection();
        }
    }

    void HandleActionSelection()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(currentAction < 3)
            {
                ++currentAction;
            }
        }

        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(currentAction > 0)
            {
                --currentAction;
            }
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentAction < 2)
            {
                currentAction += 2;
            }
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentAction > 1)
            {
                currentAction -= 2;
            }
        }
        dialogBox.UpdateActionSelection(currentAction);

        if(Input.GetKeyUp(KeyCode.Return))
        {
            if(currentAction == 0)
            {
                PlayerMove();
            }
            if (currentAction == 1)
            {
                PartyScreen();
            }
        }

    }

    void HandleMoveSelection()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(currentMove < playerUnit.Pokemon.Moves.Count - 1)
            {
                ++currentMove;
            }
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(currentMove > 0)
            {
                --currentMove;
            }
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(currentMove < playerUnit.Pokemon.Moves.Count - 2 )
            {
                currentMove += 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMove > 1)
            {
                currentMove -= 2;
            }
        }
        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        if(Input.GetKeyDown(KeyCode.Return))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }
    }

    void HandlePartySelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMember >= 0 && currentMember < playerParty.Pokemons.Count - 1)
            {
                currentMember++;
            }

        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMember > 0)
            {
                currentMember--;
            }

        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(currentMember < 3)
            {
                currentMember += 3;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(currentMember > 2)
            {
                currentMember -= 3;
            }
        }
        partyScreen.updateMemberSelection(currentMember);
        
    }
}
