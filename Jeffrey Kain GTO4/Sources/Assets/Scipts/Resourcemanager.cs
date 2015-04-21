using UnityEngine;
using System.Collections;

public class Resourcemanager : MonoBehaviour {


    public static Resourcemanager instance;

    public int goldamount;
    public int foodamount;
    public int ertsamount;

    public Player player;

	// Use this for initialization
	void Start () {
        player = Gamemanager.instance.Current();
	}
	
	// Update is called once per frame
	void Update () {
  
	}


    void Awake()
    {
        instance = this;
    }

    void OnGUI()
    {
        if (Gamemanager.instance.Enabled != false)
        {


            player = Gamemanager.instance.Current();
            if (!player) return;

            GUI.Label(new Rect(10, 10, 200, 100), "Gold: " + player.gold + "  +" + player.goldamount);
            GUI.Label(new Rect(10, 30, 200, 100), "Food: " + player.food + "  +" + player.foodamount);
            GUI.Label(new Rect(10, 50, 200, 100), "Erts: " + player.erts + "  +" + player.ertsamount);

            if (GUI.Button(new Rect(500, 10, 70, 50), "End Turn"))
            {
                EndTurn();
            }
        }
    }
    public void EndTurn()
    {
        addresources();
        Gamemanager.instance.ResetGridTiles();
        Gamemanager.instance.Upgrade = false;
        if (Gamemanager.instance.SelectedObject != null)
        {
            Gamemanager.instance.SelectedObject.GetComponent<Light>().enabled = false;
        }
        Gamemanager.instance.ChangeTurn();
        player = Gamemanager.instance.Current();
        if (Gamemanager.instance.SelectedObject != null)
            if (Gamemanager.instance.SelectedObject.gameObject.tag == "Player1")
            {
                Gamemanager.instance.SelectedObject.transform.renderer.material = Gamemanager.instance.MatPlayer1;
            }
            else
            {
                Gamemanager.instance.SelectedObject.transform.renderer.material = Gamemanager.instance.MatPlayer2;
            }
    }
    void addresources()
    {
        /*goldamount =10;
        foodamount =15;
        ertsamount =15;*/
        player.gold += player.goldamount;
        player.food += player.foodamount;
        player.erts += player.ertsamount;
    }

    public void DefeatEnemyUnit()
    {
        player.gold += 6;
        player.food += 6;
        player.erts += 6;
    }
    public bool CheckRes(Unit unit)
    {

        if (unit.Gold <= player.gold && unit.Food <= player.food && unit.Erts <= player.erts)
        {
            player.gold -= unit.Gold;
            player.food -= unit.Food;
            player.erts -= unit.Erts;
            return true;
        }
        return false;
    }
    public bool CheckRes2(Building building)
    {
        if (building.Gold <= player.gold && building.Food <= player.food && building.Erts <= player.erts)
        {
            player.gold -= building.Gold;
            player.food -= building.Food;
            player.erts -= building.Erts;
            if (building.tag == "Gold")
            {
                player.goldamount += 10;
            }
            if (building.tag == "Erts")
            {
                player.ertsamount += 10;
            }
            if (building.tag == "Food")
            {
                player.foodamount += 10;
            }
            return true;
        }
        return false;
    }

}
