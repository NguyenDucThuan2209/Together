using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Number : MonoBehaviour
{
    [SerializeField] int m_id;
    [SerializeField] Vector3 m_size;
    [SerializeField] Collider2D m_collider;
    [SerializeField] SpriteRenderer m_sprite;
    [SerializeField] Sprite[] m_numberSprite;

    private List<Hex> m_collisions = new List<Hex>();

    public int ID => m_id;
    public Collider2D Collider => m_collider;

    private void OnMouseDown()
    {        
    }
    private void OnMouseDrag()
    {
        var currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentPosition.z = 0;

        transform.position = currentPosition;
    }
    private void OnMouseUp()
    {
        var index = -1;
        var distance = float.MaxValue;
        for (int i = 0; i < m_collisions.Count; i++)
        {
            if (Vector3.Distance(transform.position, m_collisions[i].transform.position) < distance)
            {
                distance = Vector3.Distance(transform.position, m_collisions[i].transform.position);
                index = i;
            }
        }

        if (index >= 0)
        {
            transform.position = m_collisions[index].transform.position;
            m_collisions[index].OnAttachNumber(this);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Hex hex))
        {
            m_collisions.Add(hex);
        }
    }

    private void Increase()
    {
        transform.localScale = m_size;
        m_sprite.sprite = m_numberSprite[m_id - 1];
    }

    public void IncreaseNumber()
    {
        m_id++;
        StartCoroutine(GameManager.IE_Scale(transform, m_size, Vector3.zero, 0.25f, () => Increase()));
    }
    public void MergeWithNumber(Transform target)
    {
        StartCoroutine(GameManager.IE_Translate(transform, transform.position, target.position, 0.25f));
        StartCoroutine(GameManager.IE_Scale(transform, m_size, Vector3.zero, 0.25f, () => Destroy(gameObject)));
    }
    public void InitializeNumber(int id = -1)
    {
        m_id = (id > 0) ? id : m_id;
        m_sprite.sprite = m_numberSprite[id - 1];
    }
}
