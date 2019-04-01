using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour {
public ObjectPooler ObjectPooler;
public int Columns;
public int Rows;
public float Scale;
public Vector2 InitialColumnPosition;
public Vector2 ObjectOffset;
public GameObject GO; //This is for size reference
public int InitialBallCount;
public float GameboardWidth;
public float GameboardHeight;

	// Use this for initialization
	void Start () {
		GameboardWidth = GetComponentInParent<RectTransform>().rect.width;
		GameboardHeight = GetComponentInParent<RectTransform>().rect.height / 2;
		// print(GameboardWidth);
		Scale = 1f - (Columns * 0.2f);
		ObjectPooler = GetComponent<ObjectPooler>();
		// ObjectOffset = Constants.FindOffset(GO);
		// ObjectOffset = new Vector2(ObjectOffset.x - Scale, ObjectOffset.y - Scale);
		
		SetUpGameBoard(Columns, Rows);
	}
	
	// Update is called once per frame
	void Update () {
		if(PlayerManager.RensaCheck){
			PlayerManager.RensaCheck = false;
			StartCoroutine(ObjectPooler.RensaCheck());
		}
	}

	public void SetUpGameBoard(int Columns, int Rows){
		int AmountOfObjects = (Columns * Rows);
		ObjectPooler.InstantiateObjects(AmountOfObjects);
		StartCoroutine(SpawnGameObjects());
	}
	public IEnumerator SpawnGameObjects(){
		List<Vector2> Locations =  new List<Vector2>();
		foreach(GameObject obj in ObjectPooler.PooledItems){
			if(!obj.activeInHierarchy){
				for(int i = 1; i <= Rows; i++){
					for(int j = 0; j < Columns; j++){
						Locations.Add(
						new Vector2(((GameboardWidth/Columns) * j) + ((GameboardWidth/Columns)/2), 
						(GameboardHeight/2) + i * 100 )
						);
					}
				}
			}
		}
		for(int i = 0; i <= InitialBallCount; i++){
			yield return new WaitForSeconds(0.025f);
			ObjectPooler.PooledItems[i].transform.localPosition = Locations[i];
			ObjectPooler.PooledItems[i].SetActive(true);
		}
	}
}
