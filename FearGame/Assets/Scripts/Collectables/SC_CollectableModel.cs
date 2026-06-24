using UnityEngine;

public class SC_CollectableModel : MonoBehaviour
{
    [SerializeField] SC_Collectable mParent;

    private void Update()
    {
        Debug.Log(mParent.IsVisible());
        gameObject.GetComponent<Renderer>().enabled = mParent.IsVisible();
    }
}
