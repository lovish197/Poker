using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public static Player instance;
    public TextMeshPro _name, money;

    private void Awake()
    {
        instance = this;
    }
}
