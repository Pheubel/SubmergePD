using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginInput : MonoBehaviour
{
    public TMP_Text m_TextComponent;
    public GameObject networkManager;
    public void EraseText()
    {
        m_TextComponent.SetText("");
    }
    public void appendNumber(int number)
    {
        if (m_TextComponent.text.Length < 5)
        {
            m_TextComponent.SetText(m_TextComponent.text + number);
        }

    }

    public void Login()
    {
        if (m_TextComponent.text.Length ==5)
        {
            networkManager.GetComponent<NetworkManager>().Login(int.Parse(m_TextComponent.text));
        }
    }
}
