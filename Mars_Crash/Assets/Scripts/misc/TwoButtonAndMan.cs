using UnityEngine;
using UnityEngine.Events;

public class TwoButtonAndMan : MonoBehaviour
{
    public int ButtonsPressed = 0;
    [SerializeField] private UnityEvent _onPressed;
    [SerializeField] private UnityEvent _onReleased;

    public void OnButtonPressed()
    {
        if (ButtonsPressed == 1)
        {
            _onPressed.Invoke();
        }
        ButtonsPressed++;
    }

    public void OnButtonReleased()
    {
        if (ButtonsPressed == 2)
        {
            _onReleased.Invoke();
        }
        ButtonsPressed--;
    }
}
