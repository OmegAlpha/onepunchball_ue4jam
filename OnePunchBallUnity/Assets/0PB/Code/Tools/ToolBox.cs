


using System.Collections.Generic;
using UnityEngine;

public static class ToolBox
{
   public static bool IntBetween(int lower, int higher, int value)
   {
      return (value >= lower && value <= higher);
   }
}