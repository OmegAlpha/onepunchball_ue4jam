using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = System.Object;

public static class Wolfy_ClassExtensions
{
    public static Vector2 Get2D(this Vector3 orig)
    {
        return new Vector3(orig.x, orig.y);
        
        
    }

    public static void CopyTo(this Vector3 orig, Vector3 other)
    {
        other.x = orig.x;
        other.y = orig.y;
        other.z = orig.z;
    }
    
        
    public static Vector3 GetClone(this Vector3 orig)
    {
        return new Vector3(orig.x, orig.y, orig.z);
    } 
    
    public static T Random<T>(this List<T> listOfT) 
    {
        return listOfT[UnityEngine.Random.Range(0, listOfT.Count)];
    }
}