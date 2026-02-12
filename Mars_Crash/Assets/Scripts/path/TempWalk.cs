using System.Collections;
using UnityEngine;

public class TempWalk : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField]  private int _target;
    private int _direction = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.parent = _waypoints[0];
        transform.position = _waypoints[0].position;
        StartCoroutine(Slerp(5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Slerp(float speed)
    {
        while (true)
        {
            Vector3 start = transform.position;
            Vector3 end = _waypoints[_target].position;
            float t = 0;

            transform.parent = _waypoints[_target];

            while (t < 1f)
            {
                t += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(start, end, t);
                yield return null; // Use null instead of WaitForEndOfFrame unless you really need it
            }

            // Snap to exact target
            transform.position = end;

            // Update target and direction
            _target += _direction;
            if (_target == _waypoints.Length - 1 || _target == 0)
                _direction = -_direction;
        }
    }

}