using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] Number m_numberPrefab;
    private List<Hex> m_hexList;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        m_hexList = new List<Hex>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out Hex hex))
            {
                m_hexList.Add(hex);
                hex.InitializeHex(this);
            }
        }
    }
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

    public void AttachNumber(Hex startHex)
    {
        var visited = new List<Hex>();
        DFS(startHex, ref visited);

        if (visited.Count >= 3)
        {
            foreach (var hex in visited)
            {
                if (hex == startHex)
                {
                    hex.Number.IncreaseNumber();
                }
                else
                {
                    hex.MergeWithNumber(startHex.transform);
                }
            }
            AttachNumber(startHex);
        }
    }
}
