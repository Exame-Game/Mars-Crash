using UnityEngine;

public class SetParent : MonoBehaviour
{
    public void SetParentObj(GameObject parent)
    {
        this.transform.parent = parent.transform;
    }

    public void SetChildObj(GameObject child)
    {
        child.transform.parent = this.transform;
    }

    public void RemoveParentFromChild(GameObject child)
    {
        if (child == null)
            return;

        child.transform.parent = null;
    }
}
