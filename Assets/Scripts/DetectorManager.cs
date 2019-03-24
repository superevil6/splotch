using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorManager : MonoBehaviour {
public List<Detector> VerticalDetectors;
public List<Detector> HorizontalDetectors;
public List<Detector> Detectors;
public Detector Detector;
public int RandomRange;
//For measurements, this will grab the prefab.
public GameObject Ball;
//Gameboard so it knows where to place the initial detector.
public GameBoard GameBoard;
public float InitialY;

	// Use this for initialization
	void Start () {
		GenerateDetectors(GameBoard.Rows, GameBoard.Columns);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GenerateDetectors(int rows, int columns){
		//print(GameBoard.InitialColumnPosition);
		Vector2 Offset = Constants.FindOffset(Ball);
		//First make horizontal detectors
		// for(int i = 1; i < rows; i++){
		// 	Detector obj = Instantiate(Detector);
		// 	HorizontalDetectors.Add(obj);
		// 	obj.transform.SetParent(GameBoard.transform);
		// 	obj.Orientation = Enums.Orientation.Horizontal;
		// 	obj.transform.position = new Vector3(GameBoard.InitialColumnPosition.x + Offset.x, InitialY - Offset.y * i, 0);
		// }
		for(int i = 1; i <= columns; i++){
			Detector obj = Instantiate(Detector);
			VerticalDetectors.Add(obj);
			obj.transform.SetParent(GameBoard.transform);
			obj.Orientation = Enums.Orientation.Vertical;
			//obj.transform.position = new Vector3(GameBoard.InitialColumnPosition.x + Offset.x * i, GameBoard.InitialColumnPosition.y, 0);
			obj.transform.position = new Vector3(GameBoard.InitialColumnPosition.x + Offset.x * i, InitialY, 0);
		}
	}
	public void SpawnDetectors(int Rows, int Columns){
		int total = Rows + Columns;
		for(int i = 0; i <= total; i++){
			Detector obj = Instantiate(Detector);
			Detectors.Add(obj);
			obj.transform.SetParent(GameBoard.transform);
		} 
	}
	public void SpawnDetector(){
		Detector obj = Instantiate(Detector);
		Detectors.Add(obj);
		
	}
	public void PlaceDetector(Vector2 Location){

	}
}
