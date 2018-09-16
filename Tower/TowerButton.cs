using UnityEngine;

public class TowerButton : MonoBehaviour {

    [SerializeField] private Tower towerObject;
    [SerializeField] private Sprite dragSprite;
    [SerializeField] private int towerCost;

	public Tower TowerObject { get { return towerObject; } }

	public Sprite DragSprite { get { return dragSprite; } }

    public int TowerCost { get { return towerCost; } }
}
