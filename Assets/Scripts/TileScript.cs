using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{
    public Point GridPosition { get; private set; }

    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2),
                transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));
        }
    }

    private Color32 fullColor = new Color32(255, 0, 0, 255);

    private Color32 emptyColor = new Color32(12, 183, 9, 255);

    public bool IsEmpty { get; set; }

    private Towers myTower;

    private SpriteRenderer spriteRenderer;

    private Graph graph;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Setup(Point gridPos, Transform parent)
    {
        IsEmpty = true;
        this.GridPosition = gridPos;
        transform.SetParent(parent);
    }

    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null)
        {
            if (IsEmpty)
            {
                ColorTile(emptyColor);
            }
            if (!IsEmpty)
            {
                ColorTile(fullColor);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                PlaceTower();
            }
        }else if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn == null && Input.GetMouseButtonDown(0)){
            if(myTower!=null){
                GameManager.Instance.SelectTower(myTower);
            }else{
                  GameManager.Instance.DeselectTower();
            }
        }
    }

    private void OnMouseExit()
    {
        ColorTile(Color.white);
    }

    private void PlaceTower()
    {
        GameObject tower = (GameObject)Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, new Vector2(transform.position.x, transform.position.y+0.5f), Quaternion.identity);

        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;

        tower.transform.Find("Mage").GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;

        tower.transform.SetParent(transform);

        this.myTower = tower.transform.Find("Range").GetComponent<Towers>();

        // Debug.Log(transform.position);

        GameObject graphObject = GameObject.Find("Graph");
        graph = graphObject.GetComponent<Graph>();

        graph.RecalculateDistance(transform.position.x, transform.position.y);

        IsEmpty = false;

        ColorTile(Color.white);

        myTower.Price = GameManager.Instance.ClickedBtn.Price;

        GameManager.Instance.BuyTower();
    }

    private void ColorTile(Color newColor)
    {
        spriteRenderer.color = newColor;
    }
}