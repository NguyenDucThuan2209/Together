using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] Number m_numberPrefab;
    [SerializeField] Transform m_numberPlace;
    [SerializeField] Transform m_numberHolder;

    private List<Hex> m_hexList;

    private void DFS(Hex hex, ref List<Hex> visited)
    {
        visited.Add(hex);
        
        foreach (var neighbor in hex.Neighbors)
        {
            if (neighbor.IsHoldNumber && !visited.Contains(neighbor))
            {
                if (neighbor.Number.ID == hex.Number.ID)
                {
                    DFS(neighbor, ref visited);
                }
            }
        }
    }
    private void CollectNeighbor(Hex startHex)
    {
        var visited = new List<Hex>();
        DFS(startHex, ref visited);

        if (visited.Count >= 3)
        {
            int score = 0;
            foreach (var hex in visited)
            {
                score += hex.Number.ID;
                if (hex == startHex)
                {
                    continue;
                }
                else
                {
                    hex.MergeWithNumber(startHex.transform);
                }
            }
            GameManager.Instance.ScorePoint(score);
            startHex.Number.IncreaseNumber(() => CollectNeighbor(startHex));
        }
        else
        {
            for (int i = 0; i < m_hexList.Count; i++)
            {
                if (!m_hexList[i].IsHoldNumber) return;
            }
            GameManager.Instance.EndGame();
        }
    }
    private void SpawnNumber()
    {
        if (GameManager.Instance.State != GameState.Playing) return;

        var number = Instantiate(m_numberPrefab, m_numberHolder);
        number.InitializeNumber(m_numberPlace, Random.Range(1, 4));
    }
    private void ResetData()
    {
        m_hexList = new List<Hex>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out Hex hex))
            {
                m_hexList.Add(hex);
                hex.Collider.enabled = true;
            }
        }
        for (int i = 0; i < m_hexList.Count; i++)
        {
            m_hexList[i].InitializeHex(this);
        }
        for (int i = 0; i < m_numberHolder.childCount; i++)
        {
            Destroy(m_numberHolder.GetChild(i).gameObject);
        }
    }

    public void Initialize()
    {
        ResetData();
        SpawnNumber();
    }
    public void AttachNumber(Hex startHex)
    {
        CollectNeighbor(startHex);
        SpawnNumber();
    }
}
