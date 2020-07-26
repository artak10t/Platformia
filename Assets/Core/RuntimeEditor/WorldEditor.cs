using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.UI;

public enum TileTypes {RuleTile, Special, Traps}

public class WorldEditor : MonoBehaviour {
    public static WorldEditor singleton;

    #region Variables

    public Tilemap WorldTilemap;
    public Tilemap Tilemap_Front;
    public Tilemap Tilemap_Back;
    public Camera MainCamera;
    public GameObject Cursor;
    public TileTypes Type = TileTypes.RuleTile;
    public int DefaultWorldTile = 0;
    private Vector2 cursorOffset = new Vector2(0.5f, 0.5f);
    public Text TileName;

    private EventSystem currentEventSystem;

    private World newWorld = new World();
    private string worldName = "";
    [HideInInspector]
    public int SelectedTile = 0;
    [HideInInspector]
    public int CurrentRotation = 0;

    private bool WorldTileback = false;

    private List<GameObject> worldObjects = new List<GameObject>();

    private const int worldSizeX = 64;
    private const int worldSizeY = 16;
    private const int worldSizeH = 4;
    private const int worldSizeW = 16;

    #endregion

    private void Update()
    {
        InputManager();
    }

    private void InputManager()
    {
        Vector3Int MousePosition = getMousePosition();
        Cursor.transform.position = new Vector3(MousePosition.x + cursorOffset.x, MousePosition.y + cursorOffset.y, 0);

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (WorldTilemap.GetTile(MousePosition) == null && Type == TileTypes.RuleTile)
            {
                AddTile(MousePosition.x, MousePosition.y, CurrentRotation, SelectedTile, Type);
            }
            else
            {
                AddTile(MousePosition.x, MousePosition.y, CurrentRotation, SelectedTile, Type);
            }
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
                RemoveTile(MousePosition.x, MousePosition.y, Type);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            CurrentRotation -= 45;
            Cursor.transform.rotation = Quaternion.Euler(0, 0, CurrentRotation);
        }
    }

    private Vector3Int getMousePosition()
    {
        Vector3 mousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        int x = Mathf.RoundToInt(mousePosition.x - 0.5f);
        int y = Mathf.RoundToInt(mousePosition.y - 0.5f);
        Vector3Int mousePositionInt = new Vector3Int(x, y, 0);
        return mousePositionInt;
    }

    void Start () {
        singleton = this;
        currentEventSystem = EventSystem.current;
        GenerateEmptyWorld();
	}

    private void GenerateEmptyWorld()
    {
        WorldTileBack = false;

        for (int x = -worldSizeX; x < worldSizeX; x++)
        {
            for (int y = -worldSizeY; y < -worldSizeY/worldSizeH; y++)
            {
                AddTile(x, y, CurrentRotation, DefaultWorldTile, Type);
            }
        }
        for (int x = -worldSizeX; x < worldSizeX; x++)
        {
            for (int y = worldSizeY; y > worldSizeY/worldSizeH; y--)
            {
                AddTile(x, y, CurrentRotation, DefaultWorldTile, Type);
            }
        }
        for (int x = -worldSizeX; x < -(worldSizeX - worldSizeW); x++)
        {
            for (int y = -worldSizeY; y < worldSizeY; y++)
            {
                AddTile(x, y, CurrentRotation, DefaultWorldTile, Type);
            }
        }
        for (int x = worldSizeX; x > worldSizeX - worldSizeW; x--)
        {
            for (int y = -worldSizeY; y < worldSizeY; y++)
            {
                AddTile(x, y, CurrentRotation, DefaultWorldTile, Type);
            }
        }

        //BackLayer
        WorldTileBack = true;

        for (int x = -worldSizeX; x < worldSizeX; x++)
        {
            for (int y = -worldSizeY; y < worldSizeY; y++)
            {
                AddTile(x, y, CurrentRotation, DefaultWorldTile, Type);
            }
        }

        WorldTileBack = false;
    }

    public void AddTile(int x, int y, int rotation, int tile, TileTypes type)
    {
        if (!currentEventSystem.IsPointerOverGameObject())
        {
            int index;
            if (type == TileTypes.RuleTile)
            {
                if (WorldTilemap.GetTile(new Vector3Int(x, y, 0)) == null)
                {
                    newWorld.AddCell(x, y, 0, tile, (byte)type, WorldTileback);
                    WorldTilemap.SetTile(new Vector3Int(x, y, 0), Selections.singleton.RuleTiles[tile]);
                }
            }
            if (type == TileTypes.Special)
            {
                Vector3 pos = new Vector3(x + cursorOffset.x, y + cursorOffset.y, 0);
                if (CheckIfEmpty(pos, TileTypes.Special, out index))
                {
                    newWorld.AddCell(x, y, rotation, tile, (byte)type, WorldTileback);
                    GameObject obj = Instantiate(Selections.singleton.Special[tile], pos, Quaternion.identity);
                    obj.GetComponent<EmptyObject>().Position = pos;
                    obj.GetComponent<EmptyObject>().Rotation = rotation;
                    if (WorldTileback)
                    {
                        obj.GetComponent<EmptyObject>().BackLayer = true;
                    } else
                    {
                        obj.GetComponent<EmptyObject>().BackLayer = false;
                    }
                    worldObjects.Add(obj);
                }
            }
            if (type == TileTypes.Traps)
            {
                Vector3 pos = new Vector3(x + cursorOffset.x, y + cursorOffset.y, 0);
                if (CheckIfEmpty(pos, TileTypes.Traps, out index))
                {
                    newWorld.AddCell(x, y, rotation, tile, (byte)type, WorldTileback);
                    GameObject obj = Instantiate(Selections.singleton.Traps[tile], pos, Quaternion.identity);
                    obj.GetComponent<EmptyObject>().Position = pos;
                    obj.GetComponent<EmptyObject>().Rotation = rotation;
                    if (WorldTileback)
                    {
                        obj.GetComponent<EmptyObject>().BackLayer = true;
                    }
                    else
                    {
                        obj.GetComponent<EmptyObject>().BackLayer = false;
                    }
                    worldObjects.Add(obj);
                }
            }
        }
    }

    public void RemoveTile(int x, int y, TileTypes type)
    {
        if (!currentEventSystem.IsPointerOverGameObject())
        {
            int index;


            if (type == TileTypes.RuleTile && WorldTilemap.GetTile(getMousePosition()) != null)
            {
                if (WorldTilemap.GetTile(new Vector3Int(x, y, 0)) != null)
                {
                    newWorld.RemoveCell(x, y, (byte)type, WorldTileback);
                    WorldTilemap.SetTile(new Vector3Int(x, y, 0), null);
                }
            }
            if(type == TileTypes.Special)
            {
                Vector3 pos = new Vector3(x + cursorOffset.x, y + cursorOffset.y, 0);
                if (!CheckIfEmpty(pos, TileTypes.Special, out index))
                {
                    Destroy(worldObjects[index]);
                    worldObjects.RemoveAt(index);
                    newWorld.RemoveCell(Mathf.RoundToInt(pos.x - cursorOffset.x), Mathf.RoundToInt(pos.y - cursorOffset.y), (byte)type, WorldTileback);
                }
            }
            if (type == TileTypes.Traps)
            {
                Vector3 pos = new Vector3(x + cursorOffset.x, y + cursorOffset.y, 0);
                if (!CheckIfEmpty(pos, TileTypes.Traps, out index))
                {
                    Destroy(worldObjects[index]);
                    worldObjects.RemoveAt(index);
                    newWorld.RemoveCell(Mathf.RoundToInt(pos.x - cursorOffset.x), Mathf.RoundToInt(pos.y - cursorOffset.y), (byte)type, WorldTileback);
                }
            }
        }
    }

    public void SelecteTile (int value)
    {
        SelectedTile = value;
        //Types
        if (Type == TileTypes.RuleTile)
        {
            cursorOffset = new Vector2(0.5f, 0.5f);
            Cursor.GetComponent<SpriteRenderer>().sprite = Selections.singleton.RuleTiles[SelectedTile].m_DefaultSprite;
            TileName.text = Selections.singleton.RuleTiles[SelectedTile].name;
        }
        if (Type == TileTypes.Special)
        {
            cursorOffset = new Vector2(0.5f, 0.5f);
            Cursor.GetComponent<SpriteRenderer>().sprite = Selections.singleton.Special[SelectedTile].GetComponent<SpriteRenderer>().sprite;
            TileName.text = Selections.singleton.Special[SelectedTile].name;
        }
        if (Type == TileTypes.Traps)
        {
            cursorOffset = new Vector2(0.5f, 0.5f);
            Cursor.GetComponent<SpriteRenderer>().sprite = Selections.singleton.Traps[SelectedTile].GetComponent<SpriteRenderer>().sprite;
            TileName.text = Selections.singleton.Traps[SelectedTile].name;
        }
    }

    public void SelecteType(TileTypes type)
    {
        if (type != Type)
        {
            Type = type;
        }
    }

    public bool WorldTileBack
    {
        get
        {
            return WorldTileBack;
        }
        set
        {
            if (value == true)
            {
                WorldTileback = true;
                WorldTilemap = Tilemap_Back;
            }
            else
            {
                WorldTileback = false;
                WorldTilemap = Tilemap_Front;
            }
        }
    }

    #region PlayerProperties

    public bool DoubleJump
    {
        get
        {
            return GameManager.singleton.doublejump;
        }
        set
        {
            GameManager.singleton.doublejump = value;
            PlayerProperties.singleton.SetDoubleJump(value);
        }
    }

    public bool WallJump
    {
        get
        {
            return GameManager.singleton.walljump;
        }
        set
        {
            GameManager.singleton.walljump = value;
            PlayerProperties.singleton.SetWallJump(value);
        }
    }

    public bool WallStick
    {
        get
        {
            return GameManager.singleton.wallstick;
        }
        set
        {
            GameManager.singleton.wallstick = value;
            PlayerProperties.singleton.SetWallStick(value);
        }
    }

    public bool WallSlide
    {
        get
        {
            return GameManager.singleton.wallslide;
        }
        set
        {
            GameManager.singleton.wallslide = value;
            PlayerProperties.singleton.SetWallSlide(value);
        }
    }

    public bool CornerGrab
    {
        get
        {
            return GameManager.singleton.cornergrab;
        }
        set
        {
            GameManager.singleton.cornergrab = value;
            PlayerProperties.singleton.SetCornerGrab(value);
        }
    }

    public bool Dash
    {
        get
        {
            return GameManager.singleton.dash;
        }
        set
        {
            GameManager.singleton.dash = value;
            PlayerProperties.singleton.SetDash(value);
        }
    }

    #endregion

    #region SaveNLoad

    public void SaveWorld(bool getNotification)
    {
        if (worldName != "")
        {
            if (Directory.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FlavorlessArt"))
            {
                if (Directory.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FlavorlessArt\\TheMadMiners"))
                {
                    if (Directory.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FlavorlessArt\\TheMadMiners\\Maps"))
                    {
                        if (!getNotification)
                        {
                            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FlavorlessArt\\TheMadMiners\\Maps" + "\\mapbackup_" + worldName;
                            newWorld.SaveToFile(path, getNotification);
                        }
                        else
                        {
                            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FlavorlessArt\\TheMadMiners\\Maps" + "\\map_" + worldName;
                            newWorld.SaveToFile(path, getNotification);
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FlavorlessArt\\TheMadMiners\\Maps");
                        if (!getNotification)
                        {
                            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FlavorlessArt\\TheMadMiners\\Maps" + "\\mapbackup_" + worldName;
                            newWorld.SaveToFile(path, getNotification);
                        }
                        else
                        {
                            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FlavorlessArt\\TheMadMiners\\Maps" + "\\map_" + worldName;
                            newWorld.SaveToFile(path, getNotification);
                        }
                    }
                }
                else
                {
                    Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FlavorlessArt\\TheMadMiners");
                    Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FlavorlessArt\\TheMadMiners\\Maps");
                    if (!getNotification)
                    {
                        string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FlavorlessArt\\TheMadMiners\\Maps" + "\\mapbackup_" + worldName;
                        newWorld.SaveToFile(path, getNotification);
                    }
                    else
                    {
                        string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FlavorlessArt\\TheMadMiners\\Maps" + "\\map_" + worldName;
                        newWorld.SaveToFile(path, getNotification);
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FlavorlessArt");
                Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FlavorlessArt\\TheMadMiners");
                Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FlavorlessArt\\TheMadMiners\\Maps");
                if (!getNotification)
                {
                    string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FlavorlessArt\\TheMadMiners\\Maps" + "\\mapbackup_" + worldName;
                    newWorld.SaveToFile(path, getNotification);
                }
                else
                {
                    string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FlavorlessArt\\TheMadMiners\\Maps" + "\\map_" + worldName;
                    newWorld.SaveToFile(path, getNotification);
                }
            }
        }
        else
        {
            Notifications.singleton.GetNotification("Empty Name!", Color.yellow);
        }
    }

    public void LoadWorld(bool backup)
    {
        if (worldName != "")
        {
            string path;
            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FlavorlessArt\\TheMadMiners\\Maps" + "\\map_" + worldName;
            if (backup)
            {
                path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FlavorlessArt\\TheMadMiners\\Maps" + "\\mapbackup_" + worldName;
            }
            if (File.Exists(path))
            {
                newWorld.LoadFromFile(path);
                Tilemap_Back.ClearAllTiles();
                Tilemap_Front.ClearAllTiles();

                for (int i = 0; i < worldObjects.Count; i++)
                {
                    Destroy(worldObjects[i]);
                }
                worldObjects.Clear();

                for (int i = 0; i < newWorld.worldCells.Count; i++)
                {
                    if (newWorld.worldCells[i].worldtileback)
                    {
                        WorldTileBack = true;
                    }
                    else
                    {
                        WorldTileBack = false;
                    }
                    //RuleTiles
                    if (newWorld.worldCells[i].type == (byte)TileTypes.RuleTile)
                    {
                        WorldTilemap.SetTile(new Vector3Int(newWorld.worldCells[i].x, newWorld.worldCells[i].y, 0), Selections.singleton.RuleTiles[newWorld.worldCells[i].value]);
                    }
                    //Special
                    if (newWorld.worldCells[i].type == (byte)TileTypes.Special)
                    {
                        GameObject obj = Instantiate(Selections.singleton.Special[newWorld.worldCells[i].value], new Vector3(newWorld.worldCells[i].x, newWorld.worldCells[i].y, 0), Quaternion.identity);
                        obj.GetComponent<EmptyObject>().Position = new Vector3(newWorld.worldCells[i].x + 0.5f, newWorld.worldCells[i].y + 0.5f, 0);
                        obj.GetComponent<EmptyObject>().Rotation = newWorld.worldCells[i].rotation;
                        if (WorldTileback)
                        {
                            obj.GetComponent<EmptyObject>().BackLayer = true;
                        }
                        else
                        {
                            obj.GetComponent<EmptyObject>().BackLayer = false;
                        }
                        worldObjects.Add(obj);
                    }
                    //Traps
                    if (newWorld.worldCells[i].type == (byte)TileTypes.Traps)
                    {
                        GameObject obj = Instantiate(Selections.singleton.Traps[newWorld.worldCells[i].value], new Vector3(newWorld.worldCells[i].x, newWorld.worldCells[i].y, 0), Quaternion.identity);
                        obj.GetComponent<EmptyObject>().Position = new Vector3(newWorld.worldCells[i].x + 0.5f, newWorld.worldCells[i].y + 0.5f, 0);
                        obj.GetComponent<EmptyObject>().Rotation = newWorld.worldCells[i].rotation;
                        if (WorldTileback)
                        {
                            obj.GetComponent<EmptyObject>().BackLayer = true;
                        }
                        else
                        {
                            obj.GetComponent<EmptyObject>().BackLayer = false;
                        }
                        worldObjects.Add(obj);
                    }
                }
                TileProperties.singleton.SetBackLayer(false);
                WorldTileBack = false;
                Notifications.singleton.GetNotification("World Loaded!", Color.green);
            }
            else
            {
                Notifications.singleton.GetNotification("No World!", Color.yellow);
            }
        }
        else
        {
            Notifications.singleton.GetNotification("Empty Name!", Color.yellow);
        }
    }

    private bool CheckIfEmpty(Vector3 targetPos, TileTypes type, out int index)
    {
            foreach (GameObject current in worldObjects)
            {
                if (current.GetComponent<EmptyObject>().Position == targetPos && current.GetComponent<EmptyObject>().ObjectType == type)
                {
                    index = worldObjects.IndexOf(current);
                    return false;
                }
            }
            index = -1;
            return true;
    }

    public string WorldName
    {
        get
        {
            return worldName;
        }
        set
        {
            worldName = value;
        }
    }

    public class World
    {
        public List<Cell> worldCells = new List<Cell>();

        public void AddCell(int x, int y, int rotation, int value, byte type, bool worldtileback)
        {
            Cell newCell;
            newCell.x = x;
            newCell.y = y;
            newCell.rotation = rotation;
            newCell.value = value;
            newCell.type = type;
            newCell.worldtileback = worldtileback;
            worldCells.Add(newCell);
        }

        public void RemoveCell(int x, int y, byte type, bool worldtileback)
        {
            for(int i = 0; i < worldCells.Count; i++)
            {
                if(worldCells[i].x == x && worldCells[i].y == y && worldCells[i].type == type && worldCells[i].worldtileback == worldtileback)
                {
                    worldCells.RemoveAt(i);
                    return;
                }
            }
        }

        public void SaveToFile(string path, bool getNotification)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
            {
                //PlayerProperties
                writer.Write(GameManager.singleton.doublejump);
                writer.Write(GameManager.singleton.walljump);
                writer.Write(GameManager.singleton.wallstick);
                writer.Write(GameManager.singleton.wallslide);
                writer.Write(GameManager.singleton.cornergrab);
                writer.Write(GameManager.singleton.dash);

                writer.Write(worldCells.Count);
                for(int i = 0; i < worldCells.Count; i++)
                {
                    writer.Write(worldCells[i].value);
                    writer.Write(worldCells[i].x);
                    writer.Write(worldCells[i].y);
                    writer.Write(worldCells[i].rotation);
                    writer.Write(worldCells[i].type);
                    writer.Write(worldCells[i].worldtileback);
                }
            }
            if (getNotification)
            {
                Notifications.singleton.GetNotification("World Saved!", Color.green);
            }
        }

        public void LoadFromFile(string path)
        {
            worldCells.Clear();
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                //PlayerProperties
                WorldEditor.singleton.DoubleJump = reader.ReadBoolean();
                WorldEditor.singleton.WallJump = reader.ReadBoolean();
                WorldEditor.singleton.WallStick = reader.ReadBoolean();
                WorldEditor.singleton.WallSlide = reader.ReadBoolean();
                WorldEditor.singleton.CornerGrab = reader.ReadBoolean();
                WorldEditor.singleton.Dash = reader.ReadBoolean();

                int count = reader.ReadInt32();
                for(int i = 0; i < count; i++)
                {
                    Cell newCell;
                    newCell.value = reader.ReadInt32();
                    newCell.x = reader.ReadInt32();
                    newCell.y = reader.ReadInt32();
                    newCell.rotation = reader.ReadInt32();
                    newCell.type = reader.ReadByte();
                    newCell.worldtileback = reader.ReadBoolean();
                    worldCells.Add(newCell);
                }
            }
        }
    }

    public struct Cell
    {
        public int x;
        public int y;
        public int rotation;
        public int value;
        public byte type;
        public bool worldtileback;
    }

    #endregion
}
