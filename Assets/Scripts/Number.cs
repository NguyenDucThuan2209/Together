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

    private bool m_isMouseDrag = false;
    private List<Transform> m_collisions = new List<Transform>();

    public int ID => m_id;

    private void OnMouseDown()
    {
        Debug.LogWarning("Number choosed: " + transform.name);
        
        m_isMouseDrag = true;
    }
    private void OnMouseDrag()
    {
        Debug.LogWarning("Number dragged: " + transform.name);

        var currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentPosition.z = 0;

        transform.position = currentPosition;
    }
    private void OnMouseUp()
    {
        Debug.LogWarning("Number dropped: " + transform.name);

        m_isMouseDrag = false;
        var index = 0;
        var distance = float.MaxValue;
        for (int i = 0; i < m_collisions.Count; i++)
        {
            if (Vector3.Distance(transform.position, m_collisions[i].position) < distance)
            {
                distance = Vector3.Distance(transform.position, m_collisions[i].position);
                index = i;
            }
        }
        transform.position = m_collisions[index].position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_collisions.Add(collision.transform);
    }

    private IEnumerator IE_Translate(Transform obj, Vector3 start, Vector3 end, float duration, System.Action callbacks = null)
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
    private IEnumerator IE_Scale(Transform obj, Vector3 start, Vector3 end, float duration, System.Action callbacks = null)
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
    private void Increase(System.Action callback = null)
    {
        transform.localScale = m_size;
        m_sprite.sprite = m_numberSprite[m_id - 1];

        callback?.Invoke();
    }

    public void IncreaseNumber(System.Action callback = null)
    {
        m_id++;
        StartCoroutine(IE_Scale(transform, m_size, Vector3.zero, 0.5f, () => Increase(callback)));
    }
    public void MergeWithNumber(Transform target)
    {
        StartCoroutine(IE_Translate(transform, transform.position, target.position, 0.5f));
        StartCoroutine(IE_Scale(transform, m_size, Vector3.zero, 0.5f, () => Destroy(gameObject)));
    }
}
