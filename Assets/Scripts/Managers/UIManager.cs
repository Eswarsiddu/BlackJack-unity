using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
		{
            // true for homescreen
            transform.GetChild(i).gameObject.SetActive( i==0 );
        }
    }

}
