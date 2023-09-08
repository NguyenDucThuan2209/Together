using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    [SerializeField] float m_neighborRadius = 1.5f;
    [SerializeField] Collider2D m_collider;


    private Number m_number;
    private List<Hex> m_neighbor;
    private MapManager m_mapManager;

    public Number Number => m_number;
    public List<Hex> Neighbors => m_neighbor;
    public bool IsHoldNumber => m_number != null;


    public void InitializeHex(MapManager mapManager)
    {
        m_mapManager = mapManager;
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
        m_number.Collider.enabled = false;

        m_mapManager.AttachNumber(this);
    }
    public void MergeWithNumber(Transform target)
    {
        m_number.MergeWithNumber(target);
        m_number = null;

        m_collider.enabled = true;
    }
}
