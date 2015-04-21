using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

public Transform CamToUse;
public float speed = 10.0f; 
  
    
 void Start ()
     {
         

 }
 
 // Update is called once per frame
 void Update()
 {

     var movement = Vector3.zero;
     movement.z = Input.GetAxis("Mouse ScrollWheel")*20;
     movement.x = Input.GetAxis("Horizontal");
     movement.y = Input.GetAxis("Vertical");

     if (Input.GetKey("w"))
     {
         movement.x = 0;
         movement.y++;

     }
     if (Input.GetKey("s"))
     {
         movement.x = 0;
         movement.y--;


     }
     if (Input.GetKey("a"))
     {
         movement.x--;
     }
     if (Input.GetKey("d"))
     {
         movement.x++;
     }

     CamToUse.Translate(movement * speed * Time.deltaTime, Space.Self);
 }
    }
