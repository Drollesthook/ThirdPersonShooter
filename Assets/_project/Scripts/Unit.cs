using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    [SerializeField] int _identifier = default;

    public int identifier => _identifier;
}
