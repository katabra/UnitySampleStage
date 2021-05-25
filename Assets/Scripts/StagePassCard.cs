using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePassCard : MonoBehaviour, IStage
{
    List<PassCardCtrl> passCardList = new List<PassCardCtrl>();

    public List<Transform> cardPositions;
    public List<float> cardScale = new List<float>() { 0.6f, 0.8f, 1f, 0.8f, 0.6f };
    public List<int> cardOrder = new List<int>() { 1, 2, 3, 2, 1 };
    public List<Color> cardBorderColor = new List<Color>() { Color.green, Color.green, Color.cyan, Color.gray, Color.gray };
    public List<Color> cardLineColor = new List<Color>() { Color.gray, Color.gray, Color.black, Color.gray, Color.gray };
    public GameObject cardPrefab;
    public CalcCtrl calcCtrl;

    PassCardCtrl currentCard = null;

    bool canUserInput = false;

    public int maxQuizCnt = 5;
    int answerCnt = 0;

    private void OnEnable()
    {
        StageReset();
    }

    public void StageReset()
    {
        calcCtrl.stageCtrl = this;
        calcCtrl.CalcReset();
        canUserInput = false;
        answerCnt = 0;

        for(int i = 0; i < passCardList.Count; ++i)
        {
            Destroy(passCardList[i].gameObject);
        }
        passCardList.Clear();

        StageReady();
    }

    void StageReady()
    {
        for(int i = 0; i < cardPositions.Count; ++i)
        {
            GameObject card = Instantiate(cardPrefab, transform, true);

            card.transform.localScale = Vector3.one * cardScale[i];
            card.transform.position = cardPositions[i].position;

            PassCardCtrl cardCtrl = card.GetComponent<PassCardCtrl>();
            cardCtrl.sortingGroup.sortingOrder = cardOrder[i];

            cardCtrl.SetColor(cardBorderColor[i], cardLineColor[i]);

            if (i < 3)
                SetCardValue(cardCtrl);

            passCardList.Add(cardCtrl);
        }
        currentCard = passCardList[2];

        canUserInput = true;
    }

    public void PassCards()
    {
        currentCard = null;
        canUserInput = false;

        int last = passCardList.Count - 1;

        for (int i = 0; i < cardPositions.Count - 1; ++i)
        {
            passCardList[i].transform.DOMove(cardPositions[i + 1].position, 0.5f);
            passCardList[i].transform.DOScale(cardScale[i + 1], 0.5f);
            passCardList[i].sortingGroup.sortingOrder = cardOrder[i + 1];
            passCardList[i].SetColor(cardBorderColor[i + 1], cardLineColor[i + 1]);
        }

        passCardList[last].transform.DOMove(cardPositions[last].position + Vector3.right * 5f, 0.5f)
            .OnComplete(() => {
                var lastCard = passCardList[last];
                for(int i = passCardList.Count - 2;i >= 0; --i)
                {
                    passCardList[i + 1] = passCardList[i];
                }
                passCardList[0] = lastCard;
                passCardList[0].transform.position = cardPositions[0].position;
                passCardList[0].SetColor(cardBorderColor[0], cardLineColor[0]);
                SetCardValue(passCardList[0]);

                currentCard = passCardList[2];

                canUserInput = true;
            });
    }

    void SetCardValue(PassCardCtrl ctrl)
    {
        int a = UnityEngine.Random.Range(1, 100);
        int b = UnityEngine.Random.Range(1, 100);
        int high = Mathf.Max(a, b);
        int low = Mathf.Min(a, b);

        ctrl.A = high;
        ctrl.B = low;
        ctrl.Op = UnityEngine.Random.Range(1, 3);
        ctrl.resultText.text = string.Empty;
    }

    public void OnAnswerChanged()
    {
        if(currentCard != null)
            currentCard.resultText.text = calcCtrl.calcText.text;
    }

    public void CheckAnswer()
    {
        if (currentCard == null)
            return;

        if(int.TryParse(calcCtrl.calcText.text, out int answer))
        {
            switch(currentCard.Op)
            {
                case 1:
                    if (currentCard.A + currentCard.B == answer)
                    {
                        OnCurrectAnswer();
                    }
                    else
                    {
                        currentCard.resultText.text = string.Empty;
                    }
                    break;
                case 2:
                    if (currentCard.A - currentCard.B == answer)
                    {
                        OnCurrectAnswer();
                    }
                    else
                    {
                        currentCard.resultText.text = string.Empty;
                    }
                    break;
            }
        }

        calcCtrl.CalcReset();
    }

    void OnCurrectAnswer()
    {
        Debug.Log("right");
        if (++answerCnt == maxQuizCnt)
            NextStage();
        else
            PassCards();
    }

    public bool CanUserInput()
    {
        return canUserInput;
    }

    public void NextStage()
    {
        GameManager.instance.NextStage();
    }
}
