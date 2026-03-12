using System;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    private Movement _movement;
    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _movement = GetComponentInParent<Movement>();
    }

    void Update()
    {
        if (_movement._inControl && _movement._isSplit)
        {
            _renderer.enabled = true;
        }
        else
        {
            _renderer.enabled = false;
        }
    }
}
