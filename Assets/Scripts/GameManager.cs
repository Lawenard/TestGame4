using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance { get; private set; }
    static public bool UseSettings { get => Instance.useSettings; }
    static public GameSettings Settings { get => Instance.settings; }

    [SerializeField]
    private bool useSettings;
    [SerializeField]
    private GameSettings settings;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }
}
