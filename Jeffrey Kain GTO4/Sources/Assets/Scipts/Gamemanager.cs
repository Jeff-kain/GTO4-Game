using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gamemanager : MonoBehaviour {

    public GameObject Warrior;
    public GameObject Golem;
    public GameObject Goblin;
    public GameObject Paladin;
    public GameObject Food;
    public GameObject Erts;
    public GameObject Gold;
    public GameObject CurrentObject;

    public GameObject Tile;
    public GameObject PlayerPrefab;
    public GameObject SelectedObject;
    public GameObject Building;

    // Materials
    public Material HL_P1;
    public Material HL_P2;
    public Material MatPlayer1;
    public Material MatPlayer2;
    public Material GridSelect;
    public Material Lambert1;

    public Light lightcolor;
    public static Gamemanager instance;
    Player Player1;
    Player Player2;
    Player player;
    public bool Moveable = false;
    RaycastHit hit;
    public Transform Startpos;
    public Transform Endpos;
    Collider[] Hits;
    public GameObject[] Units;
    public bool Enabled = false;
    public bool Upgrade = false;

    int AttackHash = Animator.StringToHash("Attack");
    int DeathHash = Animator.StringToHash("Death");

	// Use this for initialization
	void Start () {
        
        for (int i = 0; i < 2;i++)
        {
            GameObject player = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            if (i == 0)
            {
                Player1 = player.GetComponent<Player>();
                Player1.CurrentPlayer = i;
                Player1.ActivePlayer = true;
            }
            else
            {
                Player2 = player.GetComponent<Player>();
                Player2.CurrentPlayer = i;
            }          
        }


      
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.gameObject.tag == "Player1" || hit.collider.gameObject.tag == "Player2")
                {
                    if (hit.collider.GetComponent<Unit>().CurrentPlayer == Current().CurrentPlayer)
                    {
                        SelectedObject = hit.collider.gameObject;
                        Debug.DrawLine(ray.origin, SelectedObject.transform.position);
                        SelectUnit(hit, ray);
                    }
                    CurrentObject = null;
                }
                if (SelectedObject != null)
                {
                    Debug.DrawLine(ray.origin, SelectedObject.transform.position);
                    Startpos = SelectedObject.transform;
                    MoveObject(SelectedObject, hit);
                    if (hit.collider.transform != null)
                    {
                        Endpos = hit.collider.transform;
                    }
                }
                if (CurrentObject != null && hit.collider.GetComponent<GridTile>() != null)
                {
                    Spawn(CurrentObject, player, hit.collider.gameObject.GetComponent<GridTile>());
                }
                if(Building != null && hit.collider.GetComponent<GridTile>().tag == "Building")
                {
                    Spawn(Building, player, hit.collider.gameObject.GetComponent<GridTile>());
                    Building = null;
                }
            }
       
        }
        if (Input.GetMouseButtonDown(1) && SelectedObject != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.gameObject.tag == "Player1" || hit.collider.gameObject.tag == "Player2")
                {
                    if (hit.collider.GetComponent<Unit>().CurrentPlayer == Current().CurrentPlayer && Upgrade ==false)
                    {
                        Heal(SelectedObject, hit);
                    }
                    if (hit.collider.GetComponent<Unit>().CurrentPlayer == Current().CurrentPlayer && Upgrade == true)
                    {
                        upgrade(hit);
                    }
                }
            }
        }
        if (SelectedObject != null && Endpos != null && hit.collider.transform.GetComponent<GridTile>() && hit.collider.transform.GetComponent<GridTile>().transform.tag == "Selectable")
        {
            if (Moveable == true)
            {
                SelectedObject.transform.position = Vector3.MoveTowards(Startpos.position, Endpos.position + new Vector3(0, 1f, 0), 2 * Time.deltaTime);
                SelectedObject.transform.LookAt(Endpos.position + new Vector3(0,1f,0));
                if (Vector3.Distance(Startpos.position, Endpos.position + new Vector3(0, 1f, 0)) < 0.1)
                {
                    Moveable = false;
                    Endpos.transform.tag = "UnSelectable";
                    Endpos = null;
                    ResetGridTiles();
                    ShowAvailableGrids(SelectedObject);
                }
            }
        }
        if(SelectedObject != null && Moveable == false)
        {
            ShowAvailableGrids(SelectedObject);
        }

	}


    void Awake()
    {
        instance = this;
    }

    void OnGUI()
    {
        if (Enabled != false)
        {
            player = Current();
            if (GUI.Button(new Rect(680, 10, 70, 50), "Paladin"))
            {
                player = Current();
                CurrentObject = Paladin;
                //Spawn(Paladin,player);
                Upgrade = false;
            }

            if (GUI.Button(new Rect(680, 60, 70, 50), "Golem"))
            {
                player = Current();
                CurrentObject = Golem;
                //Spawn(Golem,player);
                Upgrade = false;
            }
            if (GUI.Button(new Rect(680, 110, 70, 50), "Goblin"))
            {
                player = Current();
                CurrentObject = Goblin;
                Upgrade = false;
                //Spawn(Goblin,player);
            }
            if (GUI.Button(new Rect(750, 10, 90, 50), "Food Building"))
            {
                player = Current();
                Building = Food;
                //Spawn(Paladin,player);
                Upgrade = false;
            }

            if (GUI.Button(new Rect(750, 60, 90, 50), "Gold Building"))
            {
                player = Current();
                Building = Gold;
                //Spawn(Golem,player);
                Upgrade = false;
            }
            if (GUI.Button(new Rect(750, 110, 90, 50), "Erts Building"))
            {
                player = Current();
                Building = Erts;
                Upgrade = false;
                //Spawn(Goblin,player);
            }

            if (GUI.Button(new Rect(680, 180, 70, 50), "Upgrade"))
            {
                Upgrade = true;
            }
            GUI.Label(new Rect(10, 70, 200, 100), "Player: " + (player.CurrentPlayer + 1));
            if (SelectedObject != null)
            {
                GUI.Label(new Rect(10, 150, 200, 100), "Health: " + SelectedObject.GetComponent<Unit>().Health);
                GUI.Label(new Rect(10, 170, 200, 100), "Strength: " + SelectedObject.GetComponent<Unit>().Strength);
                GUI.Label(new Rect(10, 190, 200, 100), "Armor: " + SelectedObject.GetComponent<Unit>().Armor);
                GUI.Label(new Rect(10, 210, 200, 100), "Healing Power: " + SelectedObject.GetComponent<Unit>().Heals);
            }
            if(Upgrade == true)
            {
                GUI.Label(new Rect(10, 250, 200, 100), "Upgrade!");

            }
        }
    }
    public void ResetGridTiles()
    {
        // Reset all the gridtiles to their original attributes
               foreach (GridTile Tile in GridGenerator.Instance.Tiles)
               {
                  Tile.renderer.material = Lambert1;
               }
    }

    void MoveObject(GameObject Selected,RaycastHit Objecthit)
    {
        GameObject Gameobjecthit = Objecthit.collider.gameObject;
        //Check if object is a tile or a unit
        if (Objecthit.transform.gameObject.GetComponent<Unit>())
        {
            //Get all the objects in range of the selected unit
            Hits = Physics.OverlapSphere(SelectedObject.transform.position, SelectedObject.GetComponent<Unit>().AttackRange);
            if (Objecthit.collider.GetComponent<Unit>().CurrentPlayer != Selected.GetComponent<Unit>().CurrentPlayer)
            {
                foreach(var objects in Hits)
                {
                    if (objects.GetComponent<Unit>() && objects.GetComponent<Unit>().gameObject == Gameobjecthit)
                    {
                        Battle(SelectedObject, Gameobjecthit);
                    }
                }
            }
        }
        if(Objecthit.transform.gameObject.GetComponent<GridTile>())
        {
            TileSelectable(SelectedObject);
            Hits = Physics.OverlapSphere(SelectedObject.transform.position, SelectedObject.GetComponent<Unit>().MoveRange);
            foreach (var objects in Hits)
            {
                if(objects.GetComponent<GridTile>() && objects.GetComponent<GridTile>().gameObject == Gameobjecthit && Gameobjecthit.tag =="Selectable")
                {
                    Moveable = true;
                }
            }
        }
    }

    void Heal(GameObject Selected, RaycastHit Objecthit)
    {
        GameObject Gameobjecthit = Objecthit.collider.gameObject;
        if (Objecthit.collider.GetComponent<Unit>().CurrentPlayer == Selected.GetComponent<Unit>().CurrentPlayer)
        {
            Hits = Physics.OverlapSphere(SelectedObject.transform.position, SelectedObject.GetComponent<Unit>().AttackRange);

            foreach (var objects in Hits)
            {
                if (objects.GetComponent<Unit>() && objects.GetComponent<Unit>().gameObject == Gameobjecthit)
                {
                    Heal(SelectedObject, Gameobjecthit);
                    Gameobjecthit.GetComponent<ParticleSystem>().Play();
                }
            }
        }
    }
    void TileSelectable(GameObject SelectedObject)
    {
        // Make a tile Selectable
        RaycastHit Hit;
        if(Physics.Raycast(SelectedObject.transform.position,Vector3.down,out Hit))
        {
            Hit.collider.GetComponent<GridTile>().tag = "Selectable";
        }
    }

    // Upgrades based on the unit selected
    void upgrade(RaycastHit Objecthit)
    {
        GameObject Hit = Objecthit.collider.gameObject;
        string Name = Objecthit.collider.gameObject.name;
        if(Name == "golem(Clone)")
        {
            Hit.GetComponent<Unit>().Heals += 3;
            Hit.GetComponent<Unit>().Strength += 1;
        }
        if (Name == "Paladin(Clone)")
        {
            Hit.GetComponent<Unit>().Armor += 2;
            Hit.GetComponent<Unit>().Strength += 2;
        }
        if (Name == "Goblin(Clone)")
        {
            Hit.GetComponent<Unit>().Strength += 4;
        }
    }
    void Heal(GameObject Selected, GameObject Target)
    {

        float Healdistance = Vector3.Distance(Selected.transform.position, Target.transform.position);
        Target.GetComponent<Unit>().Health = Target.GetComponent<Unit>().Health + Selected.GetComponent<Unit>().Heals;

        //Play Animation when Healed
        if (Selected.GetComponent<Animation>() != null)
        {
            Selected.GetComponent<Animation>().Play("hpunch");
        }
    }
    void Battle(GameObject Selected, GameObject Enemy)
    {
        // Battle
        Selected.transform.LookAt(Enemy.transform.position);
        Enemy.transform.LookAt(Selected.transform.position);
        float attackdistance = Vector3.Distance(Selected.transform.position, Enemy.transform.position);
        Enemy.GetComponent<Unit>().Health = Enemy.GetComponent<Unit>().Health - Selected.GetComponent<Unit>().Strength + Enemy.GetComponent<Unit>().Armor;

        //Check if enemy is in attack range
        if (Enemy.GetComponent<Unit>().Health > 0 && Enemy.GetComponent<Unit>().AttackRange >= attackdistance)
        {
            Selected.GetComponent<Unit>().Health = Selected.GetComponent<Unit>().Health - Enemy.GetComponent<Unit>().Strength + Selected.GetComponent<Unit>().Armor;
        }

        //Play Animation when attacked
        if (Selected.GetComponent<Animation>() != null)
        {
            Selected.GetComponent<Animation>().Play("hpunch");
        }

        //Check if Selected Unit is (should be) dead
        if (Selected.GetComponent<Unit>().Health <= 0 && Selected.GetComponent<Animator>()!=null)
        {
            TileSelectable(Selected);
            SelectedObject.light.enabled = false;
            SelectedObject = null;
            Selected.GetComponent<Animator>().SetTrigger(DeathHash);
        }
        else if (Selected.GetComponent<Unit>().Health <= 0 && Selected.GetComponent<Animator>() == null)
        {
            TileSelectable(Selected);
            SelectedObject.light.enabled = false;
            SelectedObject = null;
            Selected.GetComponent<Animation>().Play("death");
            Selected.transform.tag = "UnSelectable";
        }

        //Check if enemy Unit is (should be) dead
        if (Enemy.GetComponent<Unit>().Health <= 0 && Enemy.GetComponent<Animator>() != null)
        {
            TileSelectable(Enemy);
            Enemy.GetComponent<Animator>().SetTrigger(DeathHash);
            Endpos = null;
            Resourcemanager.instance.DefeatEnemyUnit();
        }
        else if (Enemy.GetComponent<Unit>().Health <= 0 && Enemy.GetComponent<Animator>() == null)
        {
            TileSelectable(Selected);
            Endpos = null;
            Enemy.GetComponent<Animation>().Play("death");
            Enemy.transform.tag = "UnSelectable";
            Resourcemanager.instance.DefeatEnemyUnit();

        }
    }

    void SelectUnit(RaycastHit hit, Ray ray)
    {
        // Selecting a unit
            if (SelectedObject.GetComponent<Unit>())
            {
                ResetGridTiles();
                if (SelectedObject.GetComponent<Unit>().CurrentPlayer == Current().CurrentPlayer)
                {
                    hit.transform.tag = "Untagged";
                    GameObject[] Units;
                    if (SelectedObject.GetComponent<Unit>().CurrentPlayer == 0)
                    {
                        Units = GameObject.FindGameObjectsWithTag("Player1");
                        SelectedObject.renderer.material = HL_P1;
                        foreach (GameObject Unit in Units)
                        {
                            Unit.renderer.material = MatPlayer1;
                            Unit.GetComponent<Light>().enabled = false;
                        }
                        hit.transform.tag = "Player1";
                        SelectedObject.GetComponent<Light>().enabled = true;
                        //ShowAvailableGrids(SelectedObject);
                    }
                    else
                    {
                        Units = GameObject.FindGameObjectsWithTag("Player2");
                        SelectedObject.renderer.material = HL_P2;
                        foreach (GameObject Unit in Units)
                        {
                            Unit.renderer.material = MatPlayer2;
                            Unit.GetComponent<Light>().enabled = false;

                        }
                        hit.transform.tag = "Player2";
                        SelectedObject.GetComponent<Light>().enabled = true;
                        //ShowAvailableGrids(SelectedObject);

                    }
                }
            }
    }

    void ShowAvailableGrids(GameObject SelectedObject)
    {
        // Highlight gridtiles in range of selectedobject moverange
        Hits = Physics.OverlapSphere(SelectedObject.transform.position, SelectedObject.GetComponent<Unit>().MoveRange);
        foreach (var objects in Hits)
        {
            if (objects.GetComponent<GridTile>() == true && objects.tag != "UnSelectable" && objects.tag != "Building")
            {
                objects.renderer.material = GridSelect;
            }
        }
    }


    public void ChangeTurn()
    {
        // Method for changing turns
        Player1.ActivePlayer = !Player1.ActivePlayer;
        Player2.ActivePlayer = !Player2.ActivePlayer;
        Endpos = null;
        SelectedObject = null;
    }

    public Player Current()
    {
        // Check who is the current player
        if (Player1 && Player1.ActivePlayer == true)
        {
            return Player1;
        }
        else
        {
            return Player2;
        }
    }



    void Spawn(GameObject SpawnObject, Player player, GridTile Tile)
    {
        // Spawn a unit
        if (SpawnObject.GetComponent<Unit>())
        {
            if (Resourcemanager.instance.CheckRes(SpawnObject.GetComponent<Unit>()))
            {
                if (player == Player1)
                {
                    SpawnObject.transform.tag = "Player1";
                    SpawnObject.renderer.material = MatPlayer1;
                    SpawnObject.GetComponent<Unit>().CurrentPlayer = 0;
                    GameObject Spawned = Instantiate(SpawnObject, Tile.transform.position + new Vector3(0, 1f, 0), Quaternion.identity) as GameObject;
                    Spawned.transform.parent = GameObject.Find("Units_P1").transform;
                    Spawned.GetComponent<Light>().color = Color.red;
                    Tile.transform.tag = "UnSelectable";
                }
                else
                {
                    SpawnObject.transform.tag = "Player2";
                    SpawnObject.renderer.material = MatPlayer2;
                    SpawnObject.GetComponent<Unit>().CurrentPlayer = 1;
                    GameObject Spawned = Instantiate(SpawnObject, Tile.transform.position + new Vector3(0, 1f, 0), Quaternion.identity) as GameObject;
                    Spawned.transform.parent = GameObject.Find("Units_P2").transform;
                    Spawned.transform.LookAt(new Vector3(0, 0, -20));
                    Spawned.GetComponent<Light>().color = Color.blue;
                    Tile.transform.tag = "UnSelectable";
                }
            }
        }
        if(SpawnObject.GetComponent<Building>())
        {
            if(Resourcemanager.instance.CheckRes2(SpawnObject.GetComponent<Building>()))
            {
                if (player == Player1)
                {
                    SpawnObject.GetComponent<Building>().CurrentPlayer = 0;
                    GameObject Spawned = Instantiate(SpawnObject, Tile.transform.position + new Vector3(0, 1f, 0), Quaternion.identity) as GameObject;
                    Spawned.transform.parent = GameObject.Find("Units_P1").transform;
                    Tile.transform.tag = "UnSelectable";
                }
                else
                {
                    SpawnObject.GetComponent<Building>().CurrentPlayer = 1;
                    GameObject Spawned = Instantiate(SpawnObject, Tile.transform.position + new Vector3(0, 1f, 0), Quaternion.identity) as GameObject;
                    Spawned.transform.parent = GameObject.Find("Units_P2").transform;
                    Tile.transform.tag = "UnSelectable";
                }
            }
        }
        Endpos = null;
        CurrentObject = null;
        Resourcemanager.instance.EndTurn();
    }
     
    Vector3 SpawnPos()
    {
        return GridGenerator.Instance.NewTile().transform.position + new Vector3(0, 1f, 0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
