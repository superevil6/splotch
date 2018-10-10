using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
public List<GameObject> PooledItems;
public Transform ParentTransform;
public GameObject ItemToPool;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void InstantiateObjects(int Amount){
		for(int i = 0; i <= Amount; i++){
			GameObject obj = (GameObject)Instantiate(ItemToPool);
			obj.transform.SetParent(ParentTransform);
			obj.SetActive(false);
			PooledItems.Add(obj);
		}
	}
	
	public IEnumerator RensaCheck(){
		foreach(GameObject obj in PooledItems){
			if(obj.activeInHierarchy){
				obj.GetComponent<Detection>().CheckForMatches();
			}
		}
		yield return new WaitForSeconds(0.5f);
	}
}
