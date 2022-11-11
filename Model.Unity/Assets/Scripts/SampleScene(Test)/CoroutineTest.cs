using System.Collections;
using UnityEngine;

public class CoroutineTest : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Test());
    }

    IEnumerator Test() 
    {
        print("1");

        yield return new WaitForSeconds(5f);

        print("2");

        StartCoroutine(Test());
    }
}