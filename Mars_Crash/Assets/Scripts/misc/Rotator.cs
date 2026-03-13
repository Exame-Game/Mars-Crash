using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 _axis;
    [SerializeField] private float _speed;
    void Update()
    {
        transform.rotation *= Quaternion.Euler(_axis * (_speed * Time.deltaTime));
    }
}
