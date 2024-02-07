using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    public int FillUpTubes;

    public static Level Instance;

    private void Start()
    {
        Instance = this;
    }
}
