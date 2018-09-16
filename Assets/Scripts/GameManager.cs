using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStatus { Ready, Play, Next, GameOver, Win }

public class GameManager : Singleton<GameManager> {

    [SerializeField] private int playerGold;
    [SerializeField] private int maxAllowedEscapes;
    [SerializeField] private float spawnTimeDelay;
				     private float roundSpawnDelay;

	[SerializeField] private GameObject spawn;
	[SerializeField] private GameObject exit;
	[SerializeField] private Enemy[] enemies;

	[SerializeField] private int initialNumberOfEnemies;
	[SerializeField] private int totalWaves;
    private int enemiesToSpawn;

	public List<Enemy> EnemyList { get; private set; }

	public GameStatus GameState { get; private set; }

	public int GoldAmount { get; private set; }

	public int CurrentWave { get; private set; }

	public int NumberOfEnemies { get; private set; }

	public int TotalEscapes { get; private set; }

	public int RoundEscapes { get; private set; }

	public int MaxAllowedEscapes { get { return maxAllowedEscapes; } }

    public int TotalWaves { get { return totalWaves; } }

	public int Kills { get; private set; }

	public GameObject Exit { get { return exit; } } 

	private void Start() {
		GameState = GameStatus.Next;

		GoldAmount = playerGold;
		TotalEscapes = 0;
		RoundEscapes = 0;
		enemiesToSpawn = 0;
		CurrentWave = 0;

		EnemyList = new List<Enemy>();
		NumberOfEnemies = initialNumberOfEnemies;
		roundSpawnDelay = spawnTimeDelay;

		UIManager.Instance.ShowText("Created by Ian Paul");
	}

	private void Update() {
		if (TotalEscapes >= maxAllowedEscapes) 
			GameState = GameStatus.GameOver;
        
		else if (RoundEscapes + Kills == NumberOfEnemies || CurrentWave == 0) {
			if (CurrentWave == totalWaves)
				GameState = GameStatus.Win;
			else
				GameState = GameStatus.Next;
		}
	}

	public void StartWave() {
		WipeAllEnemies();
		Kills = 0;
		RoundEscapes = 0;
		CurrentWave++;

		roundSpawnDelay = spawnTimeDelay - (CurrentWave / 100);
		NumberOfEnemies = initialNumberOfEnemies * CurrentWave;

		if (enemiesToSpawn < enemies.Length) 
			enemiesToSpawn++;

		GameState = GameStatus.Play;
		StartCoroutine(SpawnEnemy());
	}

	public void AddKill() {
		Kills++;
	}

	public void UpdateGold(int amount) {
		GoldAmount += amount;
	}

	public void AddEscape() {
		TotalEscapes++;
		RoundEscapes++;
	}

	private IEnumerator SpawnEnemy() {

		if (GameState == GameStatus.GameOver) 
			StopCoroutine(SpawnEnemy());

        for (int i = 0; i < NumberOfEnemies; i++) {
            if (EnemyList.Count < NumberOfEnemies) {
                Enemy enemy = Instantiate(enemies[Random.Range(0, enemiesToSpawn)]);
                enemy.transform.position = spawn.transform.position;

                yield return new WaitForSeconds(roundSpawnDelay);
            }
        }
    }

	public void AddEnemy(Enemy enemy) {
		EnemyList.Add(enemy);
	}

	public void RemoveEnemy(Enemy enemy) {

		if (EnemyList.Count > 0) {
			EnemyList.Remove(enemy);

			if (enemy.IsDead)
				Destroy(enemy.gameObject, 2f);
			else
			    Destroy(enemy.gameObject);
        }
	}

	public void WipeAllEnemies() {
		Enemy[] array = EnemyList.ToArray();

		foreach (Enemy enemy in array) {
			if (enemy != null)
				RemoveEnemy(enemy);
		}

		EnemyList.Clear();
	}

	public void RestartGame() {
		TowerManager.Instance.DestroyAllTowers();

		GoldAmount = playerGold;
		TotalEscapes = 0;
		enemiesToSpawn = 0;
		NumberOfEnemies = initialNumberOfEnemies;
		roundSpawnDelay = spawnTimeDelay;
		CurrentWave = 0;

		StartWave();
	}

    // TESTING ONLY
	public void HardReset() {
		SceneManager.LoadScene("Level 1");
	}
}