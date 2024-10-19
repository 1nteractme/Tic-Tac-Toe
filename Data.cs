using UnityEngine;

public class Data : MonoBehaviour
{
    public int x, o, d;

    public void LoadInfo()
    {
        x = PlayerPrefs.GetInt("WX", 0);
        o = PlayerPrefs.GetInt("WO", 0);
        d = PlayerPrefs.GetInt("WD", 0);
    }
}
