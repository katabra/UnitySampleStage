using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSlotWheel : MonoBehaviour, IStage
{
    public enum SLOTSTATE
    {
        READY,
        USERINPUT,
        RESULT,
        NEXT,
    }

    public List<WheelCtrl> wheelList;
    public TMPro.TMP_Text resultText;
    public CalcCtrl calcCtrl;

    int buttonLayer = 0;

    bool canUserInput = false;

    private void Awake()
    {
        buttonLayer = 1 << LayerMask.NameToLayer("Button");
    }

    private void OnEnable()
    {
        StageReset();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(touchPosition, Vector2.zero, 0f, buttonLayer);

            if (hit)
            {
                Debug.Log(hit.collider.name);

                WheelStart();
                WheelStop();
            }
        }
    }

    public void CheckAnswer()
    {
        canUserInput = false;
        if(int.TryParse(resultText.text, out int answer))
        {
            if (wheelList[0].GetResult() + wheelList[2].GetResult() == answer)
            {
                Debug.Log("right");

                calcCtrl.CalcReset();
                NextStage();
            }
            else
            {
                Debug.Log("wrong");
                canUserInput = true;
            }
        }
    }

    public void OnAnswerChanged()
    {
        resultText.text = calcCtrl.calcText.text;
    }

    public void StageReset()
    {
        calcCtrl.stageCtrl = this;
        calcCtrl.CalcReset();
        resultText.text = string.Empty;
        canUserInput = false;

        for(int i = 0; i < 3; ++i)
        {
            wheelList[i].StageReset(i == 1);
        }
    }

    public void WheelStart()
    {
        foreach(var wheel in wheelList)
        {
            wheel.WheelStart();
        }
    }

    public void WheelStop()
    {
        StartCoroutine(CoWheelStop());
    }

    IEnumerator CoWheelStop()
    {
        foreach (var wheel in wheelList)
        {
            yield return new WaitForSeconds(1f);
            wheel.WheelStop();
        }
        canUserInput = true;
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
