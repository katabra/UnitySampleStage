using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcCtrl : MonoBehaviour
{
    public TMPro.TMP_Text calcText;
    public IStage stageCtrl;

    int buttonLayer;

    private void Awake()
    {
        buttonLayer = 1 << LayerMask.NameToLayer("CalcButton");
    }

    public void CalcReset()
    {
        calcText.text = string.Empty;
    }

    private void Update()
    {
        if (!stageCtrl.CanUserInput())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            var touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(touchPosition, Vector2.zero, 0f, buttonLayer);
            if (hit)
            {
                Debug.Log(hit.transform.name);

                int buttonNum = hit.transform.GetComponent<ButtonCtrl>().buttonValue;

                if(buttonNum == -1)
                {
                    stageCtrl.CheckAnswer();
                }
                else if(buttonNum == -2)
                {
                    if(calcText.text.Length > 1)
                    {
                        calcText.text = calcText.text.Remove(calcText.text.Length - 1);
                        if (stageCtrl != null)
                            stageCtrl.OnAnswerChanged();
                    }
                    else if(calcText.text.Length == 1)
                    {
                        calcText.text = "0";
                        if (stageCtrl != null)
                            stageCtrl.OnAnswerChanged();
                    }
                }
                else if(calcText.text == "0")
                {
                    if (buttonNum == 0) { }
                    else if (0 < buttonNum && buttonNum < 10)
                    {
                        calcText.text = buttonNum.ToString();
                        if (stageCtrl != null)
                            stageCtrl.OnAnswerChanged();
                    }
                }
                else
                {
                    calcText.text += buttonNum.ToString();
                    if(stageCtrl != null)
                        stageCtrl.OnAnswerChanged();
                }
            }
        }
    }
}
