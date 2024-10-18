using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject[] line;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameManager gameManager;
    #endregion

    #region Variables
    private List<GameObject> tiles;
    public Tile CurrentTileID;
    #endregion

    public void InitIiles()
    {
        if (tiles == null) tiles = new List<GameObject>();

        foreach (var l in line)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject tileInstance = Instantiate(tilePrefab, l.transform);
                tileInstance.AddComponent<Tile>();
                tileInstance.transform.position = new Vector3(0f, 0f, 1f);
                tiles.Add(tileInstance);
            }
        }
        for (int i = 0; i < tiles.Count; i++)
            tiles[i].GetComponent<Tile>().Id = i;

        foreach (GameObject button in tiles)
            button.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(button));
    }

    private void OnButtonClick(GameObject i)
    {
        CurrentTileID = i.GetComponent<Tile>();
        // i.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(i)); //to save move data
        gameManager.CheckMove(CurrentTileID);
    }

    public void Inactive()
    {
        foreach (GameObject i in tiles) i.GetComponent<Button>().interactable = false;
    }
}
