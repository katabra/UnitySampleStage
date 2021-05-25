using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> stageObjs;
    public Image progressBar;
    public Image fade;

    public static GameManager instance;

    int currentStage = 0;
    Color invisible = new Color(0f, 0f, 0f, 0f);

    private void Awake()
    {
        instance = this;
        progressBar.fillAmount = 0f;

        fade.color = Color.black;
    }

    private void Start()
    {
        stageObjs[currentStage].SetActive(true);
        stageObjs[currentStage].GetComponent<IStage>().StageReset();

        fade.DOColor(invisible, 0.2f).OnComplete(
            () =>
            {
                fade.gameObject.SetActive(false);
            }
            );
    }

    public void NextStage()
    {
        progressBar.fillAmount += 0.1f;

        fade.gameObject.SetActive(true);

        
        fade.color = invisible;

        fade.DOColor(Color.black, 0.5f).OnComplete(
            () =>
            {
                stageObjs[currentStage].SetActive(false);
                ++currentStage;
                if (currentStage >= stageObjs.Count) currentStage = 0;
                stageObjs[currentStage].SetActive(true);
                stageObjs[currentStage].GetComponent<IStage>().StageReset();

                fade.DOColor(invisible, 0.5f).OnComplete(
                    () =>
                    {
                        fade.gameObject.SetActive(false);
                    }
                    );
            }
            );
    }
}
