using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum Type {
    Player,
    Card,
    Board
}
[RequireComponent(typeof(Selectable))]
public abstract class Target : MonoBehaviour {

    public Type type;
    public string fullName;
    public Selectable selectable;

    public void Start()
    {
        init();
    }

	protected virtual void init() {
		selectable = GetComponent<Selectable>();
	}

    void OnMouseUp()
    {
		GameManager.instance.elementClicked(this);
    }
}