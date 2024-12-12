using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class MakeSounds
{
    public static void MakeNoise(Sound sound)
    {
        Collider[] colliders = Physics.OverlapSphere(sound.position, sound.range);
        
        for (int i = 0; i < colliders.Length; i++) 
        {
            if (colliders[i].TryGetComponent(out IHear hearer))
            {
                hearer.RespondToSound(sound);
            }
        }
    }
}
