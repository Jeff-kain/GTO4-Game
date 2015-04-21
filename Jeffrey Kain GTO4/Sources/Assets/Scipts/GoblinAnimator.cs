using UnityEngine;
using System.Collections;

public class GoblinAnimator : MonoBehaviour
{

    //Animator anim;
    int AttackHash = Animator.StringToHash("Attack");
    int DeathHash = Animator.StringToHash("Death");
    //int runStateHash = Animator.StringToHash("Base Layer.Paladin_running");
    float move;
    void Start()
    {
        //anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (Gamemanager.instance.SelectedObject != null)
        {
            if (Gamemanager.instance.SelectedObject.GetComponent<Animator>() != null)
            {
                //Stop animation move if movable is false
                if (Gamemanager.instance.Moveable == false && Gamemanager.instance.SelectedObject != null)
                {
                    move = 0f;
                    Gamemanager.instance.SelectedObject.GetComponent<Animator>().SetFloat("Speed", move);
                }
                //Play animation move if movable is true
                if (Gamemanager.instance.Moveable == true && Gamemanager.instance.SelectedObject != null && Gamemanager.instance.Endpos != null)
                {
                    move = 1f;
                    Gamemanager.instance.SelectedObject.GetComponent<Animator>().SetFloat("Speed", move);

                }

                //Play animation attack if the attack target is within range
                if (Gamemanager.instance.SelectedObject != null && Gamemanager.instance.Endpos != null && Gamemanager.instance.Endpos.GetComponent<Unit>() != null && Gamemanager.instance.Endpos.GetComponent<Unit>().CurrentPlayer != Gamemanager.instance.Current().CurrentPlayer)
                {
                    if (Gamemanager.instance.SelectedObject.GetComponent<Unit>().AttackRange >= Vector3.Distance(Gamemanager.instance.SelectedObject.transform.position, Gamemanager.instance.Endpos.GetComponent<Unit>().transform.position))
                    {
                        Gamemanager.instance.SelectedObject.GetComponent<Animator>().SetTrigger(AttackHash);
                    }
                    Gamemanager.instance.Endpos = null;
                }
            }
        }
    }

}
