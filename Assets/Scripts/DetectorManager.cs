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
private bool StartGeneration = false;

	// Use this for initialization
	void Start () {
		StartCoroutine("GameStartDelay");
	}
	
	// Update is called once per frame
	void Update () {
		if(StartGeneration){
			GenerateDetectors(GameBoard.Rows, GameBoard.Columns);
			StartGeneration = false;
		}
	}

	public void GenerateDetectors(int rows, int columns){
		//Vector2 Offset = GameBoard.Scale;//= Constants.FindOffset(Ball);
		float GameboardWidth = GameBoard.GetComponentInParent<RectTransform>().rect.width;
		float GameboardHeight = GameBoard.GetComponentInParent<RectTransform>().rect.height / 2;
		Vector2 ObjectDimensions = new Vector2((GameboardWidth / columns), 10);
		for(int i = 0; i < columns; i++){
			Detector obj = Instantiate(Detector);
			VerticalDetectors.Add(obj);
			obj.transform.SetParent(GameBoard.transform);
			obj.transform.localScale = new Vector2((ObjectDimensions.x / columns) * 10, 10);
			obj.transform.localPosition = new Vector2(
				((GameboardWidth/columns) * i) + ((GameboardWidth/columns)/2), GameboardHeight);

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
	public IEnumerator GameStartDelay(){
		print("starting");
		yield return new WaitForSeconds(2);
		StartGeneration = true;
	}
}
