using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PassCardCtrl : MonoBehaviour
{
    public TMPro.TMP_Text aText;
    public TMPro.TMP_Text bText;
    public TMPro.TMP_Text opText;
    public TMPro.TMP_Text resultText;
    public SortingGroup sortingGroup;
    public SpriteRenderer lineRender;
    public SpriteRenderer borderRender;

    public int A { get => _a; set { _a = value; aText.text = _a.ToString(); } }
    int _a;

    public int B { get => _b; set { _b = value; bText.text = _b.ToString(); } }
    int _b;

    public int Op
    {
        get => _op;
        set
        {
            _op = value;
            switch (_op)
            {
                case 1:
                    opText.text = "+";
                    break;
                case 2:
                    opText.text = "-";
                    break;
                default:
                    opText.text = string.Empty;
                    break;
            }
        }
    }
    public int _op;

    private void Awake()
    {
        CardReset();
    }

    public void CardReset()
    {
        _a = 0;
        _b = 0;
        _op = 0;

        aText.text = string.Empty;
        bText.text = string.Empty;
        opText.text = string.Empty;
        resultText.text = string.Empty;
    }

    public void SetColor(Color border, Color line)
    {
        lineRender.color = line;
        aText.color = line;
        bText.color = line;
        opText.color = line;
        resultText.color = line;
        borderRender.color = border;

    }
}
