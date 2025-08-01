using UnityEngine;
using Yarn.Unity;

public class Test : MonoBehaviour
{
    [YarnCommand("Umaru")]
    public void ThisIsADebug(string[] a)
    {
        for (int i = 0; i < a.Length; i++)
        {
            Debug.Log(a[i]);
        }
    }
}
