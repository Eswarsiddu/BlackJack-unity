using UnityEngine;

public class UIManager : MonoBehaviour
{
    private const int HOME_SCREEN_INDEX = 0;
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
		{
            transform.GetChild(i).gameObject.SetActive(i == HOME_SCREEN_INDEX);
        }
    }

}
