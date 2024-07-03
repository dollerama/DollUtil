using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DollUtil.Buggy;
using DollUtil.Attributes;

public class ConsoleTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Get the console once and it will be created. 
        Console.Get.Log("Init");
    }

    //console callbacks must be public and parameters must be string, int, float, or bool
    //type help to get all logged callbacks
    [ConsoleCallback]
    public void Log(string message)
    {
        Console.Get.Log(message);
    }
}
