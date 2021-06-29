using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    public bool _clicked;
    private Collider2D _coll;
    void Start()
    {
        _clicked = false;
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
}
