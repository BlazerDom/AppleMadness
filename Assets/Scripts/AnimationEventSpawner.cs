using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventSpawner : MonoBehaviour
{
    private AppleSpawner asSkript;

    private void Start()
    {
        asSkript = gameObject.GetComponentInParent<AppleSpawner>();
    }
    private void Event_SpawnAppleRight()
    {
        asSkript.Event_SpawnAppleRight();
    }

    private void Event_SpawnAppleLeft()
    {
        asSkript.Event_SpawnAppleLeft();
    }
}
