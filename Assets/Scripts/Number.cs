using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Number : MonoBehaviour
{
    [SerializeField] int m_id;
    [SerializeField] Vector3 m_size;
    [SerializeField] SpriteRenderer m_sprite;
    [SerializeField] Sprite[] m_numberSprite;

    public int ID => m_id;

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
