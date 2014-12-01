using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {


    public Text textReputationJ1;
    public Text textReputationJ2;

    public Slider sliderReputationJ1;

    public Text textCorruptionJ1;
	public Text textSexismeJ1;

    public Slider sliderReputationJ2;

	public Text textCorruptionJ2;
	public Text textSexismeJ2;

	public Text pseudoJ1;
	public Text pseudoJ2;



    private Player p1;
    private Player p2;
	// Use this for initialization
	void Start () {
        // Copy references
        p1 = GameManager.instance.player1;
        p2 = GameManager.instance.player2;
	}
	
	// Update is called once per frame
	void Update () {

        textReputationJ1.text = p1.reputation.ToString();
        textReputationJ2.text = p2.reputation.ToString();

        sliderReputationJ1.value = p1.reputation;
		textCorruptionJ1.text = p1.corruption.ToString("D");
		textSexismeJ1.text = p1.sexisme.ToString("D");

        sliderReputationJ2.value = p2.reputation;
		textCorruptionJ2.text = p2.corruption.ToString("D");
		textSexismeJ2.text = p2.sexisme.ToString("D");

		pseudoJ1.text = p1.fullName;
		pseudoJ2.text = p2.fullName;
	}
}
