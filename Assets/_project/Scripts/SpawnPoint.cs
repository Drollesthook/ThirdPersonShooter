using UnityEngine;

public class SpawnPoint : MonoBehaviour {
    public bool IsAvailableToSpawn {
        get => _isAvailableToSpawn;
        set => _isAvailableToSpawn = value;
    }

    private bool _isAvailableToSpawn = true;
}
