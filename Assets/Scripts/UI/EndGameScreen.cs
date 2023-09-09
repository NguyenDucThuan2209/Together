using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : UIScreen
{
    [SerializeField] Text m_scoreText;
    [SerializeField] Text m_highScoreText;
    [SerializeField] Button m_soundButton;
    [SerializeField] Button m_musicButton;

    private bool m_soundState = false;
    private bool m_musicState = false;

    public override void ShowScreen()
    {
        base.ShowScreen();

        m_soundState = SoundManager.Instance.SoundState;
        m_musicState = SoundManager.Instance.MusicState;

        m_soundButton.image.color = (m_soundState) ? new Color(1f, 1f, 1f, 0.5f) : new Color(1f, 1f, 1f, 1f);
        m_musicButton.image.color = (m_musicState) ? new Color(1f, 1f, 1f, 0.5f) : new Color(1f, 1f, 1f, 1f);
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
        m_soundButton.image.color = (m_soundState) ? new Color(1f, 1f, 1f, 0.5f) : new Color(1f, 1f, 1f, 1f);

        SoundManager.Instance.PlaySound("Click");
        SoundManager.Instance.SetSoundState(m_soundState);
    }
    public void OnMusicButtonPressed()
    {
        m_musicState = !m_musicState;
        m_musicButton.image.color = (m_musicState) ? new Color(1f, 1f, 1f, 0.5f) : new Color(1f, 1f, 1f, 1f);

        SoundManager.Instance.PlaySound("Click");
        SoundManager.Instance.SetMusicState(m_musicState);
    }
}
