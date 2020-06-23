using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginInput : MonoBehaviour
{
    public TMP_Text m_TextComponent;
    public GameObject networkManager;

    string input = string.Empty;

    public void EraseText()
    {
        input = string.Empty;
        m_TextComponent.SetText(input);
    }
    public void appendNumber(int number)
    {
        if (input.Length < 5)
        {
            input += number.ToString();

            m_TextComponent.SetText(input);
        }

    }

    public void Login()
    {
        if (input.Length ==5)
        {
            networkManager.GetComponent<NetworkManager>().Login(int.Parse(input));
        }
    }
}
