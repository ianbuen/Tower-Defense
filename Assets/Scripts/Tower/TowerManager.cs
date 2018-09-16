using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Singleton<TowerManager> {

	private TowerButton pressedButton;
	private SpriteRenderer spriteRenderer;

	private List<Tower> listTowers;
	private List<Collider2D> listBuildSites;

	void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.enabled = (false);
		listTowers = new List<Tower>();
		RegisterBuildSites();
	}

	void Update() {

        if (GameManager.Instance.GameState == GameStatus.Play) {

			if (Input.GetMouseButtonDown(0)) {

                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

				if (pressedButton != null) {

					if (hit.collider.tag == "BuildSite" || hit.collider.tag == "Tower") {

                        bool build = true;

                        if (hit.collider.tag == "BuildSite")
						    hit.collider.GetComponent<Collider2D>().enabled = (false);
						
						else if (hit.collider.tag == "Tower") {
                            Sprite sprite = hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite;

                            if (sprite != spriteRenderer.sprite)
							    Destroy(hit.collider.gameObject);
							else {
								UIManager.Instance.ShowText("You've already built the same tower.");
								build = false;
							}
						}

						if (build)
							PlaceTower(hit);
						else
						    CancelPlacement();
					}

					else 
						CancelPlacement();
				}
			}

			if (spriteRenderer.enabled) {
				FollowMouse();

				if (Input.GetKeyDown(KeyCode.Escape))
				    CancelPlacement();
			}

		} else {
			spriteRenderer.enabled = false;
			pressedButton = null;
		}
	}

	private void RegisterBuildSites() {
		listBuildSites = new List<Collider2D>();

		Collider2D[] buildSites = FindObjectsOfType<Collider2D>();

        foreach (Collider2D collider in buildSites) {
			if (collider.tag == "BuildSite")
			    listBuildSites.Add(collider);
		}
	}

	private void PlaceTower(RaycastHit2D hit) {
		Tower tower = Instantiate(pressedButton.TowerObject);
		tower.transform.position = hit.transform.position;

		SoundManager.Instance.Play(SFX.TowerBuilt);
		listTowers.Add(tower);

		GameManager.Instance.UpdateGold(-pressedButton.TowerCost);

		spriteRenderer.enabled = false;
		pressedButton = null;
	}

	private void CancelPlacement() {
		spriteRenderer.enabled = false;
		pressedButton = null;

		SoundManager.Instance.Play(SFX.Cancel);
	}

	public void SelectedTower(TowerButton selectedTower) {

		if (selectedTower.TowerCost > GameManager.Instance.GoldAmount && GameManager.Instance.GameState == GameStatus.Play) {
			CancelPlacement();
			UIManager.Instance.ShowText("Not enough gold");
		} else {
			pressedButton = selectedTower;
			spriteRenderer.sprite = pressedButton.DragSprite;
			spriteRenderer.enabled = true;
		}
	}

	private void FollowMouse() {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

	public void DestroyAllTowers() {

		foreach (Tower tower in listTowers.ToArray()) {
			if (tower != null)
			    Destroy(tower.gameObject);
		}

		listTowers.Clear();

		foreach (Collider2D listBuildSite in listBuildSites) {
			listBuildSite.enabled = true;
		}
	}
}
