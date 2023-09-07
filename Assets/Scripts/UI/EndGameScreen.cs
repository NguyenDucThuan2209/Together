using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : UIScreen
{
    [SerializeField] Text m_scoreText;
    [SerializeField] Text m_highScoreText;
    private bool m_soundState = false;
    private bool m_musicState = false;

    public override void ShowScreen()
    {
        base.ShowScreen();

        m_soundState = SoundManager.Instance.SoundState;
        m_musicState = SoundManager.Instance.MusicState;
    }
    public void SetScoreText(int score)
    {
        m_scoreText.text = score.ToString();
    }

    public void OnHomeButtonPressed()
    {
        SoundManager.Instance.PlaySound("Click");
        MenuManager.Instance.BackToHome();
    }
    public void OnReplayButtonPressed()
    {
        SoundManager.Instance.PlaySound("Click");
        MenuManager.Instance.StartGame();
    }
    public void OnSoundButtonPressed()
    {
        m_soundState = !m_soundState;

        SoundManager.Instance.PlaySound("Click");
        SoundManager.Instance.SetSoundState(m_soundState);
    }
    public void OnMusicButtonPressed()
    {
        m_musicState = !m_musicState;
        SoundManager.Instance.PlaySound("Click");
        SoundManager.Instance.SetMusicState(m_musicState);
    }
}
