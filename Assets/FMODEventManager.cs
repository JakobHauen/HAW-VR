using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEventManager : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string ButtonDownEvent = "";

    private void Start()
    {
        InputManager.Instance.RightController.OnTriggerDown += Sound_ButtonDown;
    }


    void OnDisable()
    {
        InputManager.Instance.RightController.OnTriggerDown -= Sound_ButtonDown;
    }


    void Sound_ButtonDown()
    {
        FMODUnity.RuntimeManager.PlayOneShot(ButtonDownEvent);
    }
}