using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingScreen : UIScreen
{
    [SerializeField] RectTransform m_tick;
    [SerializeField] RectTransform[] m_levelButton;
    private int m_levelID;

    public void OnLevelButtonPressed(int id)
    {
        m_levelID = id;
        m_tick.anchoredPosition = m_levelButton[id].anchoredPosition;
    }
    public void OnBackButtonPressed()
    {
        SoundManager.Instance.PlaySound("Click");
        MenuManager.Instance.BackToHome();
    }
}
