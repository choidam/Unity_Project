using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
   private Transform camTr;
   private Transform tr;

   private void Start() {
       
       camTr = Camera.main.GetComponent<Transform>();
       tr = GetComponent<Transform>();
       
   }

   private void LateUpdate() {
       tr.LookAt(camTr.position);
   }
}
