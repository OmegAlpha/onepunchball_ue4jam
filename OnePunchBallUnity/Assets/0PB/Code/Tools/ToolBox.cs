


using System.Collections.Generic;
using UnityEngine;

public static class ToolBox
{
   public static bool IntBetween(int lower, int higher, int value)
   {
      return (value >= lower && value <= higher);
   }

   public static void CleanTransformChildren(Transform t, uint targetQty)
   {
      while (t.childCount > targetQty)
      {
         GameObject.Destroy(t.GetChild(0).gameObject);
      }
   }
}