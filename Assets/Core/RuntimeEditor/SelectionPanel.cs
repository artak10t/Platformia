using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionPanel : MonoBehaviour {
    public WorldEditor worldEditor;
    public GameObject SelectionPrefab;
    private List<GameObject> SelectionButtons = new List<GameObject>();

    private void Start()
    {
        TypeChanged(0);
    }

    public void TypeChanged(int type)
    {
        for (int i = 0; i < SelectionButtons.Count; i++)
        {
            Destroy(SelectionButtons[i]);
        }
        SelectionButtons.Clear();

        if (type == 0) //RuleTile
        {
            worldEditor.SelecteType(TileTypes.RuleTile);
            for (int i = 0; i < Selections.singleton.RuleTiles.Length; i++)
            {
                GameObject obj = Instantiate(SelectionPrefab, Vector3.zero, Quaternion.identity);
                obj.transform.SetParent(gameObject.transform);
                int value = i;
                obj.GetComponent<Button>().onClick.AddListener(() => worldEditor.SelecteTile(value));
                obj.GetComponent<Image>().sprite = Selections.singleton.RuleTiles[i].m_DefaultSprite;
                SelectionButtons.Add(obj);
            }
        }
        if (type == 1) //Special
        {
            worldEditor.SelecteType(TileTypes.Special);
            for (int i = 0; i < Selections.singleton.Special.Length; i++)
            {
                GameObject obj = Instantiate(SelectionPrefab, Vector3.zero, Quaternion.identity);
                obj.transform.SetParent(gameObject.transform);
                int value = i;
                obj.GetComponent<Button>().onClick.AddListener(() => worldEditor.SelecteTile(value));
                obj.GetComponent<Image>().sprite = Selections.singleton.Special[value].GetComponent<SpriteRenderer>().sprite;
                SelectionButtons.Add(obj);
            }
        }
        if (type == 2) //Traps
        {
            worldEditor.SelecteType(TileTypes.Traps);
            for (int i = 0; i < Selections.singleton.Traps.Length; i++)
            {
                GameObject obj = Instantiate(SelectionPrefab, Vector3.zero, Quaternion.identity);
                obj.transform.SetParent(gameObject.transform);
                int value = i;
                obj.GetComponent<Button>().onClick.AddListener(() => worldEditor.SelecteTile(value));
                obj.GetComponent<Image>().sprite = Selections.singleton.Traps[value].GetComponent<SpriteRenderer>().sprite;
                SelectionButtons.Add(obj);
            }
        }
    }
}