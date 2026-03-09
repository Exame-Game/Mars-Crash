using System;
using UnityEngine;

[ExecuteAlways]
public class MaterialAssigner : MonoBehaviour
{
    [SerializeField] private Material _material;

    private void OnEnable()
    {
        ApplyMaterial();
    }

    private void OnValidate()
    {
        ApplyMaterial();
    }

    public void ApplyMaterial()
    {
        if (_material == null) 
            return;

        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        { 
            if (renderer.transform.parent.CompareTag("Player"))
                continue;

            renderer.sharedMaterial = _material;
        }
    }
    public void ApplyMaterial(Material mat = null)
    {
        if (_material == null) 
            return;

        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            if (renderer.transform.parent.CompareTag("Player"))
                continue;

            renderer.sharedMaterial = mat;
        }
    }
}
