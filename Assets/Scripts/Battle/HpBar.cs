using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] GameObject health;
    [SerializeField] GameObject healtAmount;
    // Start is called before the first frame update
    void Start()
    {
        //health.transform.localScale = new Vector3(0.5f, 1f);
    }

    // Hp que o pokemon irá ter
    public void SetHP(float hpNormalized)
    {
        health.transform.localScale = new Vector3(hpNormalized, 1f, 1f);
    }

    public IEnumerator SetHPSmooth(float newHp)
    {
        float curHP = health.transform.localScale.x;
        float changeAmt = curHP - newHp;

        health.transform.localScale = new Vector3(newHp, 1f);

        while(curHP - newHp > Mathf.Epsilon)
        {
            curHP -= changeAmt * Time.deltaTime * 0.75f;
            healtAmount.transform.localScale = new Vector3(curHP, 1f);
            yield return null;
        }
        healtAmount.transform.localScale = new Vector3(newHp, 1f);
    }

  
}
