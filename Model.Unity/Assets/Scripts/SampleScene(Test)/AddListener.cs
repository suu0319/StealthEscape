using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddListener : MonoBehaviour
{
    public Button[] Buttons;

    // Start is called before the first frame update
    void Start()
    {
        Buttons[0].onClick.AddListener(Test0);
        Buttons[1].onClick.AddListener(Test1);
        Buttons[2].onClick.AddListener(Test2);
        Buttons[3].onClick.AddListener(Test3);
        Buttons[4].onClick.AddListener(Test4);
        Buttons[5].onClick.AddListener(Test5);
        Buttons[6].onClick.AddListener(Test6);
        Buttons[7].onClick.AddListener(Test7);
        Buttons[8].onClick.AddListener(Test8);
        Buttons[9].onClick.AddListener(Test9);
    }

    public void Test0() 
    {
    
    }
    public void Test1()
    {

    }
    public void Test2()
    {

    }
    public void Test3()
    {

    }
    public void Test4()
    {

    }
    public void Test5()
    {

    }
    public void Test6()
    {

    }
    public void Test7()
    {

    }
    public void Test8()
    {

    }
    public void Test9()
    {

    }
}
