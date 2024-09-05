using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Cooldowns
{
    public static void cooldowns(float cooldowns, string name)
    {
        float cooldownInS = Mathf.Round(cooldowns);
        Debug.Log($"{name} cooldown  {cooldownInS} s");
    }

}
