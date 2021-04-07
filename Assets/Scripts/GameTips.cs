using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameTips
{
    static string[] gameTips = new string[]{
        "Design with users in mind",
        "Find your game’s hook",
        "Hear that? Sounds like sound design",
        "Try for viral",
        "Hyperfocus on fun",
        "Keep controls intuitive",
        "Balance the gameplay",
        "Make your game stand out",
        "Get organized",
        "Market your game"
    };
    public static string GetTip(){
        int index = Random.Range(0, gameTips.Length);
        return gameTips[index];
    } 
}
