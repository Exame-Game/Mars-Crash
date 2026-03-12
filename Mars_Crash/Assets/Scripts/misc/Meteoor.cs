using System;
using UnityEngine;

public class Meteoor : MonoBehaviour
{
    [SerializeField] private GameObject TargetA;

    [SerializeField] private GameObject TargetB;

    [SerializeField] private GameObject TargetC;

    [SerializeField] private float rotationSpeed = 10f;

    [SerializeField] private float flySpeed = 10f;

    private GameObject currentTarget;

    private void Start()
    {
        currentTarget = TargetB;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + rotationSpeed * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, flySpeed * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, currentTarget.transform.position) < 0.1f)
        {
            if (currentTarget == TargetA)
                currentTarget = TargetB;
            else if (currentTarget == TargetB)
                currentTarget = TargetC;
            else if (currentTarget == TargetC)
                currentTarget = TargetA;
        }
    }

}
