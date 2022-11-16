using UnityEngine;

public class DeltaTimeCompare : MonoBehaviour
{
    private int i = 0;

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Begin");
    }

    // Update is called once per frame
    private void Update()
    {
        if (Random.Range(0, 2f) == 1f)
        {
            Debug.Log(i);
            i = 10;
        }
        else 
        {
            Debug.Log(i);
            i = 5;
        }

        Debug.Log(Time.deltaTime);
        Debug.Log(Time.smoothDeltaTime);
    }
}