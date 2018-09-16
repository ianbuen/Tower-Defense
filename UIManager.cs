using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager> {

    [SerializeField] private Text textBanner;
    [SerializeField] private Text textMessage;
    [SerializeField] private Text textGoldAmount;
    [SerializeField] private Text textCurrentWave;
    [SerializeField] private Text textEscaped;

	[SerializeField] private Image uiStats;
    [SerializeField] private Image uiBanner;

	[SerializeField] private Button btnCenter;
    [SerializeField] private Button btnGiveUp;

	[SerializeField] private Text textCenterButton;

	void Start() {
		textMessage.enabled = false;

        ShowBanner();

		ShowText("Created by Ian Paul");
	}

	void Update() {

		if (GameManager.Instance.GoldAmount.ToString() != textGoldAmount.text)
		    textGoldAmount.text = (GameManager.Instance.GoldAmount.ToString());

		if (textEscaped.text != "Escaped: " + GameManager.Instance.TotalEscapes.ToString() + "/" + GameManager.Instance.MaxAllowedEscapes)
		    textEscaped.text = ("Escaped: " + GameManager.Instance.TotalEscapes.ToString() + "/" + GameManager.Instance.MaxAllowedEscapes);

        if (textCurrentWave.text != "Wave: " + GameManager.Instance.CurrentWave.ToString() + "/" + GameManager.Instance.TotalWaves)
		    textCurrentWave.text = ("Wave: " + GameManager.Instance.CurrentWave.ToString() + "/" + GameManager.Instance.TotalWaves);
		
		if (GameManager.Instance.GameState != GameStatus.Play)
		    ShowBanner();
		else {
			uiBanner.enabled = false;
			textBanner.enabled = false;
			btnCenter.gameObject.SetActive(false);
			btnGiveUp.gameObject.SetActive(true);
			textCenterButton.enabled = false;
		}
	}

	public void ShowText(string message) {
		StartCoroutine(DisplayText(message));
	}

	private IEnumerator DisplayText(string message) {
		textMessage.text = message;
		textMessage.enabled = true;

        yield return new WaitForSeconds(1.5f);

        textMessage.enabled = false;
    }

	public void CenterButton() {

		if (GameManager.Instance.GameState == GameStatus.GameOver || GameManager.Instance.GameState == GameStatus.Win)
		    GameManager.Instance.RestartGame();
		else
			GameManager.Instance.StartWave();
	}

	public void ShowBanner() {
		textCenterButton.text = "Play";

		uiBanner.enabled = (true);
		textBanner.enabled = (true);
		btnCenter.gameObject.SetActive(true);
		btnGiveUp.gameObject.SetActive(false);
		textCenterButton.enabled = (true);

		switch (GameManager.Instance.GameState) {

		    case GameStatus.GameOver:
		        textBanner.text = "Game Over!";
                textCenterButton.text = textCenterButton.text + " Again";
                break;

		    case GameStatus.Win:
		        textBanner.text = "You Won!!";
                textCenterButton.text = textCenterButton.text + " Again";
                break;

		    case GameStatus.Next:
			    if (GameManager.Instance.CurrentWave == 0)
			        textBanner.text = "Ready?";
			    else {
				    textBanner.text = "Wave Cleared!";
				    textCenterButton.text = "Next Wave";
			    }

			    break;
		}
	}
}
