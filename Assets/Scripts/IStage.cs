using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStage 
{
    public bool CanUserInput();
    public void StageReset();

    public void OnAnswerChanged();

    public void CheckAnswer();

    public void NextStage();

}
