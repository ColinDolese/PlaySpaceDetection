using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityARInterface;

public class PSGrid : MonoBehaviour {

	// tunable paramters
	public int rows = 10;
	public int columns = 10;
	public float yAnchor = 0;
	public const float MAX_ERROR = 0.25f;
	public const float REQUIRED_VERIFIES = 3;
	public const float ALLOWED_ERRORS = 1;




	// grid
	public List<List<PSCell>> cells;

	// prefabs
	public GameObject cellPrefab;
	public GameObject pointPrefab;


	public float width { get { return cellWidth * columns; } }
	public float length { get { return cellLength * rows; } }


	private ARInterface.PointCloud m_PointCloud;
	private float cellWidth;
	private float cellLength;

	// invalid grid indices found at each timestep

	private List<string> validIndices;

	public void generateGrid() {
		cells = new List<List<PSCell>> ();

		validIndices = new List<string>();


		if (cellPrefab != null) {

			cellWidth = cellPrefab.GetComponent<Renderer> ().bounds.size.x;
			cellLength = cellPrefab.GetComponent<Renderer> ().bounds.size.z;

			buildGrid (cellWidth, cellLength);

//			if (ARInterface.GetInterface ().TryGetPointCloud (ref m_PointCloud)) {
//
//
//				foreach (Vector3 point in m_PointCloud.points) {
//
//					GameObject go = Instantiate (pointPrefab);
//					go.transform.position = point;
//
//				}
//			}				
		}
	}


	private void buildGrid(float width, float length) {

		float currentX = 0;
		float currentZ = 0;

		for (int i = 0; i < columns; i++) {

			cells.Add (new List<PSCell>());


			for (int j = 0; j < rows; j++) {
				
				if (cellPrefab != null) {
					
					GameObject go = Instantiate (cellPrefab, gameObject.transform);

					go.transform.localPosition = new Vector3 (currentX, 0f, currentZ);
					//go.transform.SetParent (gameObject.transform);

					PSCell cell = new PSCell (go);
					cells [i].Add (cell);

					currentZ += length;


				}

			}
			currentZ = 0;
			currentX += width;

		}
	}
		

	// Update is called once per frame
	void Update () {
//		if (ARInterface.GetInterface().TryGetPointCloud(ref m_PointCloud))
//		{
//			int[,] verifies = new int[columns, rows];
//
//			int[,] errors = new int[columns, rows];
//
//
//			float x0 = transform.position.x - (cellWidth/2);
//			float x1 = x0 + width;
//			float z0 = transform.position.z - (cellWidth/2);
//			float z1 = z0 + length;
//			float y = transform.position.y;
//
//			// check which grid cells are invalid this time step
//			foreach (Vector3 point in m_PointCloud.points) {
//
//				if (point.x > x0 && point.x < x1 && point.z > z0 && point.z < z1) {
//
//
//					int x = (int)((point.x - x0) / cellWidth);
//					int z = (int)((point.z - z0) / cellLength);
//
//					if (Mathf.Abs (y - point.y) > MAX_ERROR) {
//
//						errors [x, z]++;
//				
//					} else if (errors[x,z] <= ALLOWED_ERRORS) {
//						verifies [x, z]++;
//
//						if (verifies [x, z] >= REQUIRED_VERIFIES) {
//							string index = x.ToString() + "," + z.ToString();
//							if (!validIndices.Contains (index))
//								validIndices.Add (index);
//						
//						}
//
//					}
//
//
//				}
//					
//			}
//
//			// update grid based on above results
//			for (int i = validIndices.Count - 1; i >= 0; i--) {
//				
//				string[] indices = validIndices [i].Split (',');
//				int x = int.Parse (indices [0]);
//				int z = int.Parse (indices [1]);
//
//				if (errors[x, z] == 0) {
//					cells [x] [z].cellGO.SetActive (true);
//					cells [x] [z].valid = true;
//
//
//				} else {
//					cells [x] [z].cellGO.SetActive (false);
//					cells [x] [z].valid = false;
//
//					validIndices.RemoveAt (i);
//				}
//
//			}
//
//		}
	}



}
