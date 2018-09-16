using UnityEngine;

public enum SFX { Arrow, Cancel, Death, Fireball, GameOver, Hit, NewGame, Rock, TowerBuilt }

public class SoundManager : Singleton<SoundManager> {

	[SerializeField] private AudioClip[] clips;

	private AudioSource audioSource;

	private GameStatus lastStatus; 

	private void Update() {
		if (GameManager.Instance.GameState == GameStatus.GameOver && lastStatus != GameStatus.GameOver) {
			lastStatus = GameStatus.GameOver;
			Play(SFX.GameOver);
		}

        if (GameManager.Instance.GameState == GameStatus.Next && lastStatus != GameStatus.Next) {
			lastStatus = GameStatus.Next;
			Play(SFX.NewGame);
		}

		if (GameManager.Instance.GameState == GameStatus.Win && lastStatus != GameStatus.Win) {
			lastStatus = GameStatus.Win;
			Play(SFX.NewGame);
		}

		if (GameManager.Instance.GameState == GameStatus.Play && lastStatus != GameStatus.Play)
			lastStatus = GameStatus.Play;
	}

	private void Start() {
		audioSource = GetComponent<AudioSource>();
	}

	public void Play(SFX soundIndex) {
		audioSource.PlayOneShot(clips[(int)soundIndex]);
	}
}