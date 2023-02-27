using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HpBar hpBar;
    [SerializeField] Text HpText;
    [SerializeField] int PreviousHP;
    

    Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;

        nameText.text = _pokemon.Base.Name;
        levelText.text = "Lvl " + _pokemon.Level;
        hpBar.SetHP((float)_pokemon.HP / _pokemon.MaxHp);
        HpText.text = _pokemon.HP.ToString();
       
        
    }

    public IEnumerator UpdateHP()
    {
        StartCoroutine(HpCounterpart2());
        yield return hpBar.SetHPSmooth((float)_pokemon.HP / _pokemon.MaxHp);
        
        //HpText.text = _pokemon.HP.ToString();
        
    }

    public void HpCounterpart1()
    {
        PreviousHP = _pokemon.HP;
    }

    public IEnumerator HpCounterpart2()
    {
        int CurrentHP = _pokemon.HP;
        while(PreviousHP > CurrentHP)
        {
            PreviousHP -= 1;
            HpText.text = PreviousHP.ToString();
            yield return new WaitForSeconds(0.1f);
        }
        PreviousHP = _pokemon.HP;
    }

    

}
