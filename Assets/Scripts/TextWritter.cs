using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWritter : MonoBehaviour
{
    Text uiText;
    string textToWrite;
    int characterIndex;
    float timePerCharacter;
    float timer;

    public void addWriter(Text uiText, string textToWrite, float timePerCharacter)
    {
        this.uiText = uiText;
        this.textToWrite = textToWrite;
        this.timePerCharacter = timePerCharacter;
        characterIndex = 0;
    }

    void Update()
    {
        if(uiText != null)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                //mostrar o proximo caracter
                timer += timePerCharacter;
                characterIndex++;
                uiText.text = textToWrite.Substring(0, characterIndex);
            }
        }
    }
}
