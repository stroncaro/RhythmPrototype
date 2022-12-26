using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviourObserver : MonoBehaviour
{
    private void Start()
    {
        int test = SingletonMonoBehaviourTest.Instance.testInt;
        Debug.Log($"Test Int is {test}");
    }
}
