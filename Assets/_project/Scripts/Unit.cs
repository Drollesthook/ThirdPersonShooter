using UnityEngine;

namespace Project {
    public class Unit : MonoBehaviour {
        [SerializeField] int _identifier;

        public int identifier => _identifier;
    }
}