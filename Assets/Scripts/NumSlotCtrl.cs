using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumSlotCtrl : MonoBehaviour
{
    public TMPro.TMP_Text numText;

    public int Num
    {
        get => num;
        set
        {
            num = value;
            numText.text = num.ToString();
        }
    }
    int num;
}
