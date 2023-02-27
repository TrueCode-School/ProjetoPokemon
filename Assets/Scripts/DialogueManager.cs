using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Queue<string> sentences;
    public Animator animFala;
    public Image imagemNpc;
    public bool isSpeaking = false;
    // Start is called before the first frame update
    void Start()
    {
        animFala = GameObject.FindGameObjectWithTag("GUI/Dialogue").GetComponent<Animator>();
        sentences = new Queue<string>();
        dialogueText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && isSpeaking == true)
        {
            StartCoroutine(DisplayNextSentence());
        }
    }

    public void awakedialogue(Dialogue dialogueF)
    {
        dialogueText.text = "";
        StartCoroutine(StartDialogue(dialogueF));

    }

    IEnumerator StartDialogue(Dialogue dialogueC)
    {
        imagemNpc.sprite = dialogueC.imagem;
        nameText.text = dialogueC.name;
        sentences.Clear();
        foreach(string sentence in dialogueC.sentences)
        {
            sentences.Enqueue(sentence);
        }
        yield return new WaitForSeconds(1f);
        dialogueText.fontSize = dialogueC.fontS;
        StartCoroutine(DisplayNextSentence());
    }

    IEnumerator DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            StartCoroutine(EndDialogue());
            yield return null;
        }
        string sentence = sentences.Dequeue();
        //dialogueText.text = sentence;
        FindObjectOfType<TextWritter>().addWriter(dialogueText, sentence, 0.1f);
        isSpeaking = true;

    }

    IEnumerator EndDialogue()
    {
        animFala.SetTrigger("DialogueHide");
        animFala.ResetTrigger("DialogueShow");
        isSpeaking = false;
        yield return null;
    }

}
