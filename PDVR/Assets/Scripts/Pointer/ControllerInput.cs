using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class ControllerInput : BaseInput
{
    [Header("Actions")]
    public SteamVR_Action_Boolean squeeze;
    [SerializeField] private SteamVR_Input_Sources _hands;


    public Camera eventCamera = null;




    protected override void Awake()
    {
        GetComponent<BaseInputModule>().inputOverride = this;
    }

    void Start()
    {
        squeeze.AddOnStateDownListener(TriggerDown, _hands);
        squeeze.AddOnStateUpListener(TriggerUp, _hands);
    }

    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        print("up");
    }
    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        print("down");
    }


    public override Vector2 mousePosition
    {
        get
        {
            return new Vector2(eventCamera.pixelWidth / 2, eventCamera.pixelHeight / 2);
        }
    }
}
