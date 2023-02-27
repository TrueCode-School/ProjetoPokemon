using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [Header("Battle Dialog Attributes")]
    [SerializeField] float letterPerSeconds;

    [SerializeField] Text dialogText;
    [SerializeField] Text ppText;
    [SerializeField] Text typeText;

    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;// Armazena o objeto vazio que guarda os ataques
    [SerializeField] GameObject moveDetails; // Detalhes dos Ataque

    [SerializeField] Color HighligthedColor;
    [SerializeField] Color ActualColor;

    [SerializeField] List<Text> actionTexts;
     
    [SerializeField] List<Text> moveTexts;

    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach(var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(letterPerSeconds);
        }
        
    }

    
    
    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }

    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }



    public void UpdateActionSelection(int selectedAction)
    {
        for(int i=0; i < actionTexts.Count; i++)
        {
            if(i == selectedAction)
            {
                actionTexts[i].color = HighligthedColor;
            }
            else
            {
                actionTexts[i].color = ActualColor;
            }
        }
    }

    public void UpdateMoveSelection(int selectedMove, Move move)
    {
        for(int i = 0; i < moveTexts.Count; i++)
        {
            if(i == selectedMove)
            {
                moveTexts[i].color = HighligthedColor;
            }
            else
            {
                moveTexts[i].color = ActualColor;
            }

            ppText.text = $"PP {move.PP}/{move.Base.Pp}";
            typeText.text = move.Base.Type.ToString();

        }  
    }

    public void SetMoveNames(List<Move> moves)
    {
        for(int i = 0; i < moveTexts.Count; i++)
        {
            if(i < moves.Count)
            {
                moveTexts[i].text = moves[i].Base.Name;
            }
            else
            {
                moveTexts[i].text = "-";
            }
        }
    }

}
