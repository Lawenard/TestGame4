using UnityEngine;
using UnityEngine.UI;

public class HitCount : MonoBehaviour
{
    private Text text;
    private int count;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    public void Count()
    {
        count++;
        text.text = count.ToString();
    }
}
