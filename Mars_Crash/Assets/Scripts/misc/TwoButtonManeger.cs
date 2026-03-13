using UnityEngine;
using UnityEngine.Events;

public class TwoButtonManeger : MonoBehaviour
{ 
    public int ButtonsPressed = 0;
    [SerializeField] private UnityEvent _onPressed;
    [SerializeField] private UnityEvent _onReleased;

    public void OnButtonPressed()
    {
        if (ButtonsPressed == 0)
        {
            _onPressed.Invoke();
        }
        ButtonsPressed++;
    }

    public void OnButtonReleased()
    {
        if (ButtonsPressed == 1)
        {
            _onReleased.Invoke();
        }
        ButtonsPressed--;
    }
}
