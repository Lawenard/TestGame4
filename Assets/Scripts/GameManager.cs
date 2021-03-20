using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    static public GameManager Instance { get; private set; }
    public bool UseSettings { get => useSettings; }

    [SerializeField]
    public bool useSettings;

    [SerializeField]
    public GameSettings settings;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }
}
