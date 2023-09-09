using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None,
    Initializing,
    Playing,
    End
}

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public static GameManager Instance => m_instance;

    [Header("Game Properties")]
    [SerializeField] GameState m_state;
    [Header("Game Preferences")]
    [SerializeField] MapManager[] m_maps;

    private int m_score;
    private int m_bestScore;
    private int m_currentMapIndex;

    private static Vector2 m_vertical;
    private static Vector2 m_horizontal;
    public static Vector2 Vertical => m_vertical;
    public static Vector2 Horizontal => m_horizontal;

    public GameState State => m_state;

    private void Awake()
    {
        if (m_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        m_instance = this;

        CalculateScreenSize();
    }

    private void HideAllMap()
    {
        for (int i = 0; i < m_maps.Length; i++)
        {
            m_maps[i].gameObject.SetActive(false);
        }
    }
    private void InitializeGame()
    {
        m_score = 0;
        for (int i = 0; i < m_maps.Length; i++)
        {
            if (i == m_currentMapIndex)
            {
                m_maps[i].gameObject.SetActive(true);
                m_maps[i].Initialize();
            }
            else
            {
                m_maps[i].gameObject.SetActive(false);
            }
        }
    }
    private void CalculateScreenSize()
    {
        var bottomLeft = Camera.main.ScreenToWorldPoint(Vector2.zero);
        var upperRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        m_vertical = new Vector2(bottomLeft.y, upperRight.y);
        m_horizontal = new Vector2(bottomLeft.x, upperRight.x);
    }

    public void ScorePoint(int score = 1)
    {
        m_score += score;
        m_bestScore = (m_bestScore < m_score) ? m_score : m_bestScore;

        SoundManager.Instance.PlaySound("Merge");
        MenuManager.Instance.SetScore(m_score, m_bestScore);
    }
    public void SetMapIndex(int index)
    {
        m_currentMapIndex = (index < m_maps.Length)? index : m_currentMapIndex;
    }

    public void StartGame()
    {
        Debug.LogWarning("Start Game");
        m_state = GameState.Playing;

        InitializeGame();
    }
    public void EndGame()
    {
        Debug.LogWarning("End Game");
        m_state = GameState.End;

        HideAllMap();
        MenuManager.Instance.EndGame();
    }
    public void ExitGame()
    {
        Debug.LogWarning("Exit Game");
        m_state = GameState.End;

        HideAllMap();
    }

    #region IENumerator
    public static IEnumerator IE_Translate(Transform obj, Vector3 start, Vector3 end, float duration, System.Action callbacks = null)
    {
        float t = 0;
        while (t < duration)
        {
            obj.position = Vector3.Lerp(start, end, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        obj.position = end;
        callbacks?.Invoke();
    }
    public static IEnumerator IE_Scale(Transform obj, Vector3 start, Vector3 end, float duration, System.Action callbacks = null)
    {
        float t = 0;
        while (t < duration)
        {
            obj.localScale = Vector3.Lerp(start, end, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        obj.localScale = end;
        callbacks?.Invoke();
    }
    #endregion
}
