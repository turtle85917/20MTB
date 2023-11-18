using System.Reflection;
using UnityEngine;

[ExecuteInEditMode()]
public class Test : MonoBehaviour
{
    private void Update()
    {
        // 현재 게임 객체의 각도를 얻음
        float zAngle = transform.eulerAngles.z;

        LogClear();

        Debug.Log(zAngle);
        if(60 >= zAngle || zAngle - 360 >= -60) Debug.Log("1");
        else if(300 >= zAngle && zAngle > 240) Debug.Log("2");
        else if(120 <= zAngle && zAngle <= 240) Debug.Log("3");
        else Debug.Log("4");
    }

    private void LogClear()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}
