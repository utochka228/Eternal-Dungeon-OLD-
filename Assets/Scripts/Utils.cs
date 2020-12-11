﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
public static class Utils
{
    private static readonly Random Random = new Random();
    public static void Shuffle<T>(this IList<T> list)
         {
             int n = list.Count;
             while (n > 1)
             {
                 n--;
                 int k = Random.Next(n + 1);
                 T value = list[k];
                 list[k] = list[n];
                 list[n] = value;
             }
         }
}
