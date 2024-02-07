using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTube : MonoBehaviour
{

    private void OnMouseUp()
    {
        GameManager.instance.GameLogic(this.gameObject);
    }
    
}
