using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    public float shotPower, charRunSpeed, charMoveSpeed, stunTime;
    public Color[] ballColors;
}
