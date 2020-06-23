using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfilePanel : MonoBehaviour
{
    public TMP_Text idText;
    public TMP_Text first_nameText;
    public TMP_Text last_nameText;
    public TMP_Text  temp_idText;
    public Officer officer;
    public NetworkManager NetworkManager;
    public bool active = false;

    public ProfilePanel(Officer officer)
    {
        this.officer = officer;
        SetIdText(officer.id);
        SetFirstNameText(officer.first_name);
        SetLastNameText(officer.last_name);
        SetTempIdText(officer.temp_id.ToString());
    }

   public void SetIdText(string text)
    {
        idText.SetText(text);
    }

    public void SetFirstNameText(string text)
    {
        first_nameText.SetText(text);
    }

    public void SetLastNameText(string text)
    {
        last_nameText.SetText(text);
    }

    public void SetTempIdText(string text)
    {
        temp_idText.SetText(text);
    }

    public void LoadOtherWorkspace()
    {
        if (active == false)
        {
            NetworkManager.loadWorkspace(officer.temp_id.ToString());
            active = true;
        }
        if (active == true)
        {
            Destroy(GameObject.Find(officer.id));
            active = false;
        }
    }
}
