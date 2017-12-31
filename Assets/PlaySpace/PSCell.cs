using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityARInterface;


public class PSCell {

	public GameObject cellGO;
	public bool valid { get; set; }
	public int numVerify { get; set; }
	public int numError { get; set; }

	public PSCell(GameObject go) {
		cellGO = go;
		cellGO.SetActive (true);
		valid = true;
//		cellGO.SetActive(false);
//		valid = false;

		numVerify = 0;
		numError = 0;
	}
		
		
}
