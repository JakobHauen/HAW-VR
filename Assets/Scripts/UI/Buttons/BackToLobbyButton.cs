using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToLobbyButton : UIButton
{
    public override void OnClick()
    {
        base.OnClick();
        SceneManager.LoadScene(0);
    }
}
