using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour {
public ObjectPooler ObjectPooler;
public int Columns;
public int Rows;
public Vector2 InitialColumnPosition;
private Vector2 ObjectOffset;
public GameObject GO; //This is for size reference
public int InitialBallCount;

	// Use this for initialization
	void Start () {
		ObjectPooler = GetComponent<ObjectPooler>();
		ObjectOffset = Constants.FindOffset(GO);
		SetUpGameBoard(Columns, Rows);
	}
	
	// Update is called once per frame
	void Update () {
		if(Constants.RensaCheck){
			Constants.RensaCheck = false;
			StartCoroutine(ObjectPooler.RensaCheck());
		}
	}

	public void SetUpGameBoard(int Columns, int Rows){
		int AmountOfObjects = (Columns * Rows - 1);
		ObjectPooler.InstantiateObjects(AmountOfObjects);
		SpawnGameObjects();
	}
	public void SpawnGameObjects(){
		ObjectPooler.PooledItems[0].transform.position = InitialColumnPosition;
		ObjectPooler.PooledItems[0].SetActive(true);
		List<Vector2> Locations =  new List<Vector2>();
		foreach(GameObject obj in ObjectPooler.PooledItems){
			if(!obj.activeInHierarchy){
				for(int i = 1; i <= Rows; i++){
					for(int j = 1; j <= Columns; j++){
						Locations.Add(new Vector2(InitialColumnPosition.x + (ObjectOffset.x * j), InitialColumnPosition.y + ObjectOffset.y * i));
					}
				}
			}
		}
		for(int i = 0; i <= InitialBallCount; i++){
			ObjectPooler.PooledItems[i].transform.position = Locations[i];
			ObjectPooler.PooledItems[i].SetActive(true);
		}
	}

}
