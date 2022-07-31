using UnityEngine;

public class UIManager : MonoBehaviour
{
    void Start()
    {
        foreach(Transform child in transform)
            child.gameObject.SetActive(child.CompareTag(TAGS.HOME_SCREEN));
    }

}
