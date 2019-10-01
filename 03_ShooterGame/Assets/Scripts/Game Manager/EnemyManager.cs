using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField]
    private GameObject boar_Prefab;

    public Transform[] boar_SpawnPoints;

    [SerializeField]
    private int boar_Enemy_Count;

    private int initial_boar_Count;

    public float wait_Before_Spawn_Enemies_Time = 10f;

    private void Awake() {
        MakeInstance();
    }

    private void Start() {
        initial_boar_Count = boar_Enemy_Count;

        SpawnEnemies();
        StartCoroutine("CheckToSpawnEnemies");
    }

    void MakeInstance(){
        if(instance == null){
            instance = this;
        }
    }

    void SpawnEnemies(){
        SpawnBoars();
    }

    void SpawnBoars(){
        int index = 0;
        

        for(int i=0; i<boar_Enemy_Count; i++){
            if(index >= boar_SpawnPoints.Length){
                index = 0;
            }
            Instantiate(boar_Prefab, boar_SpawnPoints[index].position, Quaternion.identity);
            index++;
        }
        boar_Enemy_Count = 0;
    }

    IEnumerator CheckToSpawnEnemies(){
        yield return new WaitForSeconds(wait_Before_Spawn_Enemies_Time);

        SpawnBoars();
        
        StartCoroutine("CheckToSpawnEnemies");
    }

    public void EnemyDied(bool boar){
        if(boar){
            boar_Enemy_Count++;
            if(boar_Enemy_Count > initial_boar_Count){
                boar_Enemy_Count = initial_boar_Count;
            }
        }
    }

    public void StopSpawning(){
        StopCoroutine("CheckToSpawnEnemies");
    }
}
