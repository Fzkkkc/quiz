using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class GameScript : MonoBehaviour
{

    public QuestionList[] questions;
    public Text[] answersText;
    public Text qText;
    public GameObject headPanel;
    public Button[] answerBttns = new Button[3];
    public Sprite[] TFIcons = new Sprite[2];
    public Image TFIcon;
    public Text TFText;

    bool defaultColor = false, trueColor = false, falseColor = false;

    List<object> qList;
    QuestionList currentQ;
    int randQ;

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (defaultColor) 
        {
            headPanel.GetComponent<Image>().color = Color.Lerp(headPanel.GetComponent<Image>().color, new Color(231 / 255.0f, 78 / 255.0f, 62 / 255.0f), 8 * Time.deltaTime);
        }
        if (trueColor)
        {
            headPanel.GetComponent<Image>().color = Color.Lerp(headPanel.GetComponent<Image>().color, new Color(104 / 255.0f, 184 / 255.0f, 89 / 255.0f), 8 * Time.deltaTime);
        }
        if (falseColor)
        {
            headPanel.GetComponent<Image>().color = Color.Lerp(headPanel.GetComponent<Image>().color, new Color(192 / 255.0f, 57 / 255.0f, 43 / 255.0f), 8 * Time.deltaTime);
        }
    }

    public void OnClickPlay()
    {
        AudioManager.instance.Play("tap");
        qList = new List<object>(questions);
        questionGenerate();
        if (!headPanel.GetComponent<Animator>().enabled)
        {
            headPanel.GetComponent<Animator>().enabled = true;
        }
        else
        {
            headPanel.GetComponent<Animator>().SetTrigger("In");
        }
    }

    public void questionGenerate()
    {
        if (qList.Count > 0)
        {
            randQ = Random.Range(0, qList.Count);
            currentQ = qList[randQ] as QuestionList;
            qText.text = currentQ.question;
            qText.gameObject.GetComponent<Animator>().SetTrigger("In");
            List<string> answers = new List<string>(currentQ.answers);
            for (int i = 0; i < currentQ.answers.Length; i++)
            {
                int rand = Random.Range(0, answers.Count);
                answersText[i].text = answers[rand];
                answers.RemoveAt(rand);
            }
            StartCoroutine(animBttns());
        }
        else
        {
            headPanel.GetComponent<Animator>().SetTrigger("Out");
        }
    }
    IEnumerator animBttns()
    {
        //yield return new WaitForSeconds(1);
        for (int i = 0; i < answerBttns.Length; i++)
        {
            answerBttns[i].interactable = false;
        }
        int a = 0;
        while (a < answerBttns.Length)
        {
            if (!answerBttns[a].gameObject.activeSelf)
            {
                answerBttns[a].gameObject.SetActive(true);
            }
            else
            {
                answerBttns[a].gameObject.GetComponent<Animator>().SetTrigger("In");
            }
            a++;
           //yield return new WaitForSeconds(1);
        }
        for (int i = 0; i < answerBttns.Length; i++)
        {
            answerBttns[i].interactable = true;
        }
        yield break;
    }
    IEnumerator trueOrFalse(bool check)
    {
        defaultColor = false;
        for (int i = 0; i < answerBttns.Length; i++)
        {
            answerBttns[i].interactable = false;
        }

        yield return new WaitForSeconds(0.3f);

        

        for (int i = 0; i < answerBttns.Length; i++)
        {
            answerBttns[i].gameObject.GetComponent<Animator>().SetTrigger("Out");
            AudioManager.instance.Play("tap");
        }

        qText.gameObject.GetComponent<Animator>().SetTrigger("Out");
        yield return new WaitForSeconds(0.5f);
        if (!TFIcon.gameObject.activeSelf)
        {
            TFIcon.gameObject.SetActive(true);
        }
        else
        {
            TFIcon.gameObject.GetComponent<Animator>().SetTrigger("In");
        }

        if (check)
        {           
            trueColor = true;
            TFIcon.sprite = TFIcons[0];
            TFText.text = "Правильно!";
            yield return new WaitForSeconds(0.8f);
            TFIcon.gameObject.GetComponent<Animator>().SetTrigger("Out");
            qList.RemoveAt(randQ);
            questionGenerate();
            trueColor = false;
            defaultColor = true;
            yield break;
        }
        else
        {
            falseColor = true;
            TFIcon.sprite = TFIcons[1];
            TFText.text = "Неправильно";
            yield return new WaitForSeconds(0.8f);
            TFIcon.gameObject.GetComponent<Animator>().SetTrigger("Out");
            headPanel.GetComponent<Animator>().SetTrigger("Out");
            falseColor = false;
            defaultColor = true;
            yield break;
        }
    }
    public void answersBttns(int index)
    {
        if (answersText[index].text.ToString() == currentQ.answers[0])
        {
            StartCoroutine(trueOrFalse(true));
        }
        else
        {
            StartCoroutine(trueOrFalse(false));
        }
    }
}

[System.Serializable]
public class QuestionList
{
    public string question;
    public string[] answers = new string[3];
}

