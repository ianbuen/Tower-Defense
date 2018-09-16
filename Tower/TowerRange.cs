using System.Collections.Generic;
using UnityEngine;

public class TowerRange : MonoBehaviour {

    public List<Enemy> EnemiesInRange { get; private set; }

	void Start() {
		EnemiesInRange = new List<Enemy>();
	}

	void Update() {
		if (transform.parent == null)
		    Destroy(this);
		
		EnemiesInRange.RemoveAll((Enemy item) => item == null);
		EnemiesInRange.RemoveAll((Enemy item) => item.IsDead);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Enemy")
		    EnemiesInRange.Add(collision.gameObject.GetComponent<Enemy>());
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.tag == "Enemy")
		    EnemiesInRange.Remove(collision.gameObject.GetComponent<Enemy>());
	}
}
