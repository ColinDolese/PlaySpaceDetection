using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace UnityARInterface
{
	public class DetectValidPlayspace : ARBase
	{

		public ARController arController;

		public GameObject[,] grids;
		public GameObject gridPrefab;
		public GameObject agentPrefab;
		private bool agentSpawned = false;

		private float gridHeight = float.MaxValue;

		private int maxGridsColumn = 3;
		private int maxGridsRow = 3;
		private float gridWidth;
		private float gridLength;

		private GameObject currentGrid;
		private int curX;
		private int curZ;

		void OnEnable()
		{
			ARInterface.planeAdded += PlaneAddedHandler;
		}

		void OnDisable()
		{
			ARInterface.planeAdded -= PlaneAddedHandler;
		}

		void Start() {
			grids = new GameObject[maxGridsColumn,maxGridsRow];
		}

		protected virtual void SetGridHeight(BoundedPlane plane) {
			if (plane.center.y < gridHeight) {
				gridHeight = plane.center.y;
				for (int i = 0; i < maxGridsColumn; i++) {
					for (int j = 0; j < maxGridsRow; j++) {
						if (grids [i, j] != null) {
							Vector3 currentPos = grids [i, j].transform.localPosition;

							grids [i, j].transform.localPosition = new Vector3 (currentPos.x, gridHeight, currentPos.z);
						}


					}
				}
			}
		}

		private GameObject addGrid(int x, int z)
		{

			//GameObject grid = Instantiate (gridPrefab, GetRoot());
			GameObject grid = Instantiate (gridPrefab);

			PSGrid psGrid = grid.GetComponent<PSGrid> ();

			psGrid.generateGrid ();
			grids [x,z] = grid;
			return grid;


		}

		protected virtual void PlaneAddedHandler(BoundedPlane plane)
		{
			SetGridHeight(plane);
		}

		private Vector3 getInitialPos(int x, int z) {
			int centerX = maxGridsColumn / 2;
			int centerZ = maxGridsRow / 2;
			int centerXOffset = centerX - x;
			int centerZOffset = centerZ - z;
			float xPos = grids[centerX,centerZ].transform.localPosition.x - (gridWidth*centerXOffset);
			float zPos = grids[centerX,centerZ].transform.localPosition.z - (gridLength*centerZOffset);
			return new Vector3 (xPos, gridHeight, zPos);
		}

		private GameObject findCurrentGrid() {

			Vector3 camPos = Camera.main.transform.position;
			Vector3 gridPos0 = currentGrid.transform.localPosition;
			Vector3 gridPos1 = new Vector3 (gridPos0.x + gridWidth, gridPos0.y, gridPos0.z + gridLength);

			if (camPos.x > gridPos0.x && camPos.x < gridPos1.x && camPos.z > gridPos0.z && camPos.z < gridPos1.z) {
				return grids[curX,curZ];
			} 

			for (int i = 0; i < maxGridsColumn; i++) {
				for (int j = 0; j < maxGridsRow; j++) {
					if (grids [i, j] != null) {
						gridPos0 = grids [i,j].transform.localPosition;

					} else {
						gridPos0 = getInitialPos (i, j);
					}
					gridPos1 = new Vector3 (gridPos0.x + gridWidth, gridPos0.y, gridPos0.z + gridLength);
					if (camPos.x > gridPos0.x && camPos.x < gridPos1.x && camPos.z > gridPos0.z && camPos.z < gridPos1.z) {
						curX = i;
						curZ = j;

					} 

				}
			}

			if (grids [curX,curZ] != null) {
				return grids [curX,curZ];
			}
			return null;
		}


		void Update() {
			
			if (currentGrid == null && gridHeight < float.MaxValue) {
				curX = maxGridsColumn / 2;
				curZ = maxGridsRow / 2;

				currentGrid = addGrid (curX, curZ);
				PSGrid psGrid = currentGrid.GetComponent<PSGrid> ();
				gridWidth = psGrid.width;
				gridLength = psGrid.length;

				currentGrid.transform.localPosition = new Vector3 (transform.position.x - (gridWidth / 2), 
					gridHeight, transform.localPosition.z - (gridLength / 2));


			} else if (gridHeight < float.MaxValue) {
				
				currentGrid = findCurrentGrid ();
				if (currentGrid == null) {
					currentGrid = addGrid (curX, curZ);
					currentGrid.transform.localPosition = getInitialPos (curX, curZ);			
				}

				if (!agentSpawned) {
					int X = maxGridsColumn / 2;
					int Z = maxGridsRow / 2;
						
					Vector3 pos = grids [X, Z].GetComponent<PSGrid> ().cells [5] [5].cellGO.transform.position;
					NavMeshHit closestHit;
					if (NavMesh.FindClosestEdge (pos, out closestHit, NavMesh.AllAreas)) {
						GameObject agent = Instantiate (agentPrefab);
						agent.transform.position = closestHit.position;
						agentSpawned = true;

					}

				}	
			}

		}
			
	}
}