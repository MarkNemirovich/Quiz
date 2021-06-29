using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectClick : MonoBehaviour
{
    public int _myNumber;
    public bool _clicked;
    private Collider2D _coll;
    private Transform target;

    private void Start()
    {
        target = gameObject.transform;        
        ScaleMe(0.01f,0.5f,0.3f,0.4f, 0.25f);
        _coll = GetComponent<Collider2D>();
    }

    private void OnMouseDown()
    {
        _clicked = true;
        _coll.enabled = false;
        Invoke("Rehub", 1f);
    }

    private void Rehub()
    {
        _clicked = false;
        _coll.enabled = true;
    }

    private void ScaleMe(float start, float max, float min, float end, float dur)
    {
        target.localScale = new Vector3(start, start, start);
        var seq = DOTween.Sequence();
        seq.Append(transform.DOScale(max, dur*1.5f));
        seq.Append(transform.DOScale(min, dur));
        seq.Append(transform.DOScale(end, dur));
    }

    public void Right()
    {
        ScaleMe(0.4f, 0.6f, 0.4f, 0.5f, 0.5f);
    }

    public void Wrong()
    {
        var seq = DOTween.Sequence();        
        seq.Append(transform.DOMove(target.position + new Vector3(0.4f,0,0), 0.4f));
        seq.Append(transform.DOMove(target.position - new Vector3(0.4f, 0, 0), 0.4f));
        seq.Append(transform.DOMove(target.position, 0.4f));
    }
}
