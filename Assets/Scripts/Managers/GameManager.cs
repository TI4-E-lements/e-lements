using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : Singleton<GameManager>
{
    public TowerBtn ClickedBtn { get; set; }

    private int currency;

    [SerializeField] private Text currencyTxt;

    private Tower selectedTower;

    public int Currency{
        get{
            return currency;
        }
        set{
            this.currency = value;
            this.currencyTxt.text = value.ToString() + " <color=lime>$</color>";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Currency = 10;
    }

    // Update is called once per frame
    void Update()
    {
        HandleEscape();
    }

    public void PickTower(TowerBtn towerBtn){
        if(Currency >= towerBtn.Price){
            this.ClickedBtn = towerBtn;
            Hover.Instance.Activate(towerBtn.Sprite);
        }
    }

    public void BuyTower(){
        if(Currency >= ClickedBtn.Price){
            Currency -= ClickedBtn.Price;

            Hover.Instance.Deactivate();
        }
    }

    public void SelectTower(Tower tower) {
        if (selectedTower != null) {
            selectedTower.Select();
        }

        selectedTower = tower;
        selectedTower.Select();
    }

    public void DeselectTower() {
        if (selectedTower != null) {
            selectedTower.Select();
        }

        selectedTower = null;
    }

    private void HandleEscape(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            Hover.Instance.Deactivate();
        }
    }
}
