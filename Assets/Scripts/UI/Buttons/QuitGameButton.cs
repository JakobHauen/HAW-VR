using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameButton : UIButton
{
    public override void OnClick()
    {
        base.OnClick();
        Application.Quit();
    }
}
