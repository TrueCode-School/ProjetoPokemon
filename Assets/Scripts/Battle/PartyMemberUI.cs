using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] Text hpText;
    [SerializeField] HpBar hpBar;
    [SerializeField] Image selector;

    [SerializeField] Color highlightColor;
    [SerializeField] Color normalColor;

    Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;
        nameText.text = pokemon.Base.Name;
        levelText.text = $"Lvl {pokemon.Level}";
        hpText.text = pokemon.HP.ToString();
        hpBar.SetHP((float)pokemon.HP / pokemon.MaxHp);
        

    }

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            selector.color = highlightColor;
        }
        else
        {
            selector.color = normalColor;
        }
    }


}
