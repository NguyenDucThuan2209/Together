using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    [SerializeField] float m_neighborRadius = 1.5f;
    [SerializeField] Collider2D m_collider;
    
    private List<Hex> m_neighbor;
    private Number m_number;

    public Number Number => m_number;

    private void Start()
    {
        InitializeHex();
    }

    public void InitializeHex()
    {
        m_neighbor = new List<Hex>();
        var objects = Physics2D.OverlapCircleAll(transform.position, m_neighborRadius);
        foreach (var obj in objects)
        {
            if (obj.TryGetComponent(out Hex hex) && obj != m_collider)
            {
                m_neighbor.Add(hex);
            }
        }
    }
    public void OnAttachNumber(Number number = null)
    {
        m_collider.enabled = false;
        m_number = (number != null)? number : m_number;

        var neighborSameIDs = new List<Number>();
        for (int i = 0; i < m_neighbor.Count; i++)
        {
            if (m_neighbor[i].Number == null) return;
            if (m_neighbor[i].Number.ID == m_number.ID)
            {
                neighborSameIDs.Add(m_neighbor[i].Number);
            }
        }

        if (neighborSameIDs.Count >= 3)
        {
            for (int i = 0; i < neighborSameIDs.Count; i++)
            {
                neighborSameIDs[i].MergeWithNumber(m_number.transform);
            }
            m_number.IncreaseNumber(() => OnAttachNumber());
        }
    }
    public void MergeWithNumber(Transform target)
    {
        m_number.MergeWithNumber(target);
        m_number = null;

        m_collider.enabled = true;
    }
}
