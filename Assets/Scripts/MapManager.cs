using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] Number m_numberPrefab;
    [SerializeField] Transform m_numberPlaceHolder;
    private List<Hex> m_hexList;

    private void Start()
    {
        Initialize();
    }
    private void Update()
    {
        
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
    private void CollectNeighbor(Hex startHex)
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
            CollectNeighbor(startHex);
        }
    }
    private void SpawnNumber()
    {
        var number = Instantiate(m_numberPrefab, transform);
        number.InitializeNumber(Random.Range(1, 4));

        number.transform.position = m_numberPlaceHolder.position;
    }

    public void Initialize()
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
    public void AttachNumber(Hex startHex)
    {
        CollectNeighbor(startHex);
    }
}
