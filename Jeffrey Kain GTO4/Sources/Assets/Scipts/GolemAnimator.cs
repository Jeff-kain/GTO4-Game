using UnityEngine;
using System.Collections;

public class GolemAnimator : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if(Gamemanager.instance.GetComponent<Animation>()!= null)
        GetComponent<Animation>().Play("idle");
	}
	
	// Update is called once per frame
	void Update () {
        if (Gamemanager.instance.SelectedObject != null && Gamemanager.instance.SelectedObject.GetComponent<Animation>()!=null)
        {
            //Stop animation move if movable is false
            if (Gamemanager.instance.Moveable == false && Gamemanager.instance.SelectedObject != null)
            {
                Gamemanager.instance.SelectedObject.GetComponent<Animation>().Play("idle");
            }
            //Play animation move if movable is true
            if (Gamemanager.instance.Moveable == true && Gamemanager.instance.SelectedObject != null && Gamemanager.instance.Endpos != null)
            {
                Gamemanager.instance.SelectedObject.GetComponent<Animation>().Play("walk");
            }


            if (Gamemanager.instance.SelectedObject != null && Gamemanager.instance.Endpos != null && Gamemanager.instance.Endpos.GetComponent<Unit>() != null && Gamemanager.instance.Endpos.GetComponent<Unit>().CurrentPlayer != Gamemanager.instance.Current().CurrentPlayer)
            {
                if (Gamemanager.instance.SelectedObject.GetComponent<Unit>().AttackRange >= Vector3.Distance(Gamemanager.instance.SelectedObject.transform.position, Gamemanager.instance.Endpos.GetComponent<Unit>().transform.position))
                {
                    Gamemanager.instance.SelectedObject.GetComponent<Animation>().Play("hpunch");
                }
                Gamemanager.instance.Endpos = null;
            }
        }
	}
}
