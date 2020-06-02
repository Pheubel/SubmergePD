using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Teleport _teleport;
    [SerializeField] private SteamVR_Input_Sources _filterInput;

    [SerializeField] ControllerMenu _activeMenu;

    [SerializeField] ControllerMenu _toolMenu;
    [SerializeField] ControllerMenu _magnifyMenu;
    [SerializeField] ControllerMenu _sketchMenu;
    [SerializeField] ControllerMenu _meassureMenu;
    [SerializeField] ControllerMenu _recorderMenu;
    [SerializeField] ControllerMenu _databaseMenu;

    public void ActivateToolMenu() => ToggleMenu(_toolMenu);
    public void ActivateMagnifyMenu() => ToggleMenu(_magnifyMenu);
    public void ActivateSketchMenu() => ToggleMenu(_sketchMenu);
    public void ActivateMeassureMenu() => ToggleMenu(_meassureMenu);
    public void ActivateRecorderMenu() => ToggleMenu(_recorderMenu);
    public void ActivateDatabaseMenu() => ToggleMenu(_databaseMenu);

    public void EnableActiveMenu()
    {
        if (_activeMenu == null)
            return;

        _teleport.enabled = false;
        _activeMenu.gameObject.SetActive(true);
    }
    public void DisableActiveMenu()
    {
        if (_activeMenu == null)
            return;

        _teleport.enabled = true;
        _activeMenu.gameObject.SetActive(false);
    }

    private void ToggleMenu(ControllerMenu menu)
    {
        if (_activeMenu == menu)
            return;

        if (_activeMenu != null)
            _activeMenu.gameObject.SetActive(false);

        menu.gameObject.SetActive(true);
        _activeMenu = menu;
    }

    public void SetTouchPosition(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        if (fromSource != _filterInput)
            return;

        if (_activeMenu != null)
            _activeMenu.SetTouchPosition(axis);
    }

    public void HandlePressed(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (fromSource != _filterInput)
            return;

        if (_activeMenu != null)
            _activeMenu.HandlePressed();
    }

    public void HandleTouched(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (fromSource != _filterInput)
            return;

        if (newState)
            EnableActiveMenu();
        else
            DisableActiveMenu();
    }
}
