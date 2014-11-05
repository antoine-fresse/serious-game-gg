using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {


    public Text textReputationJ1;
    public Text textReputationJ2;

    public Slider sliderReputationJ1;
    public Slider sliderCorruptionJ1;
    public Slider sliderSexismeJ1;

    public Slider sliderReputationJ2;
    public Slider sliderCorruptionJ2;
    public Slider sliderSexismeJ2;


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
        sliderCorruptionJ1.value = p1.corruption;
        sliderSexismeJ1.value = p1.sexisme;

        sliderReputationJ2.value = p2.reputation;
        sliderCorruptionJ2.value = p2.corruption;
        sliderSexismeJ2.value = p2.sexisme;


	}
}
