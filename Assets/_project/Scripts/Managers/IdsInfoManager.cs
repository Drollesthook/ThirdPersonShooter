using System.Collections.Generic;

using UnityEngine;

namespace Project.Managers {
    public class IdsInfoManager : MonoBehaviour {
        private static IdsInfoManager _instance;

        [SerializeField] private Dictionary<int, string> _unitsDictionary = new Dictionary<int, string>();
        
        public static IdsInfoManager instance => _instance;

        private void Awake() {
            _instance = this;
        }
    }
}
