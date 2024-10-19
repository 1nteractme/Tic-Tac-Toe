using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager;

    private void Start() => InitInfoData();

    private void InitInfoData()
    {
        var statistica = GetComponent<Statistica>();
        var data = GetComponent<Data>();
        gameManager = new GameManager();

        data.LoadInfo();
        statistica.x.text = $"X: {data.x}";
        statistica.o.text = $"O: {data.o}";
        statistica.d.text = $"D: {data.o}";
    }
    public void Play() => StartCoroutine(gameManager.Load(1));

    public void Menu() => StartCoroutine(gameManager.Load(0));

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif

#if UNITY_STANDALONE
        Application.Quit();
#endif
    }
}
