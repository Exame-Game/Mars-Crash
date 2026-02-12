using System.Collections;
using UnityEngine;

public class TempWalk : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField]  private int _target;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.parent = _waypoints[0];
        transform.position = _waypoints[0].position;
        StartCoroutine(Slerp(1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Slerp(float speed)
    {
        transform.parent = _waypoints[_target];
        Vector3 start = transform.position;
        float t = 0;
        while (true)
        {
            t += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(start, _waypoints[_target].position, t);
            yield return new WaitForEndOfFrame();
            if (t >= 1)
            {
                transform.position = _waypoints[_target].position;
                if (_target == _waypoints.Length - 1)break;
                _target++;
                StartCoroutine(Slerp(1));
                break;
            }
        }
    }
}
