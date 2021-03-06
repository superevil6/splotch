﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
public List<GameObject> PooledItems;
public List<GameObject> PooledParticleEmitters;
public List<GameObject> PooledTestingObjects;
public PunishmentManager PunishmentManager;
public PlayerManager PlayerManager;
public Transform ParentTransform;
public GameObject ItemToPool;
public GameObject ParticleObjectToPool;
public GameObject TestingObject;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void InstantiateObjects(int Amount){
		for(int i = 0; i <= Amount; i++){
			GameObject obj = (GameObject)Instantiate(ItemToPool);
			obj.tag = "Ball" + PlayerManager.PlayerNumberManager.PlayerPrefix;
			obj.transform.SetParent(ParentTransform);
			obj.SetActive(false);
			PooledItems.Add(obj);
		}
	}
	public void InstatinateParticleEmitters(int Amount){
		for(int i = 0; i <= Amount; i++){
			GameObject obj = (GameObject)Instantiate(ParticleObjectToPool);
			obj.transform.SetParent(ParentTransform);
			//obj.SetActive(false);
			PooledParticleEmitters.Add(obj);
		}
	}
	public IEnumerator RensaCheck(){
		foreach(GameObject obj in PooledItems){
			if(obj.activeInHierarchy){
				obj.GetComponent<Detection>().CheckForMatches(false);
			}
		}
		yield return new WaitForSeconds(0.5f);
		PlayerManager.NumberOfSecondsForPunishment = PlayerManager.ScoreMultiplier;
		PunishmentManager.ShouldPunish = true;
	}
}
