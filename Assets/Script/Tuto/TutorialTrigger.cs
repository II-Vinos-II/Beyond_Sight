using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class TutorialTrigger : MonoBehaviour
{
    public KeyCode[] tutoinput;

    public string tutoKeyboard;
    public string tutoController;
    private GameObject canvasTuto;

    public bool tutoStart;
    private bool tutoEnd;

    private PlayerController2 player;

    public bool movement;

    private void Start()
    {
        player = FindObjectOfType<PlayerController2>();
        canvasTuto = GameObject.Find("TutoText");
        tutoStart = false;
        canvasTuto.GetComponent<TextMeshProUGUI>().text = "";
    }

    private void Update()
    {
        if (tutoStart && !tutoEnd)
        {
            if (player.onKeyboard)
            {
                canvasTuto.GetComponent<TextMeshProUGUI>().text = tutoKeyboard;
            }
            else
            {
                canvasTuto.GetComponent<TextMeshProUGUI>().text = tutoController;
            }

            if (movement)
            {
                if (Mathf.Abs(InputManager._moveHorizontal) != 0f)
                {
                    tutoEnd = true;
                    canvasTuto.GetComponent<Animation>().Play("EndTuto");
                }
            }

            float distance = Vector3.Distance(player.transform.position, transform.position);

            foreach (var tuto in tutoinput)
            {

                if (Input.GetKey(tuto) || distance > 10f)
                {
                    tutoEnd = true;
                    canvasTuto.GetComponent<Animation>().Play("EndTuto");
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !tutoStart)
        {
            tutoStart = true;
            canvasTuto.GetComponent<Animation>().Play("StartTuto");
            //if(IACS.activeControl)
        }
    }

    /*private IEnumerator DisplayLine(string line)
    {
        TextMeshProUGUI dialogueText = canvasTuto.GetComponent<TextMeshProUGUI>();

        line = line.Substring(0, line.Length - 1);

        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;
        foreach (char letter in line.ToCharArray())
        {
            if ((InputManager.GetInstance().GetSubmitPressed() && !isGrigriActivated) || isSkipping)
            {
                dialogueText.maxVisibleCharacters = line.Length;
                if (itemset)
                {
                    item.GetComponent<Animator>().Play("item_set");
                }
                else
                {
                    item.GetComponent<Animator>().Play("item_default");
                }

                if (isSkipping)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                break;
            }
            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                if (letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            else
            {
                PlayDialogueSound(dialogueText.maxVisibleCharacters, dialogueText.text[dialogueText.maxVisibleCharacters]);
                dialogueText.maxVisibleCharacters++;
                if (isGrigriActivated)
                {
                    yield return new WaitForSeconds(typingSpeed * 2);
                }
                else if (!isGrigriActivated)
                {
                    yield return new WaitForSeconds(typingSpeed);
                }

            }
        }

        dialogueText.maxVisibleCharacters = line.Length;
        canContinueToNextLine = true;

        if (puzzleName != "nothing" && isSkipping)
        {
            isSkipping = false;
            EnterPuzzle();
        }
        else
        {
            DisplayChoices();
        }
    }*/
}
