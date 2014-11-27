using UnityEngine;
using System.Collections;

public class Board : Target {

	public Player owner;

	protected override void init() {
		base.init();
		//TargetType = TargetType.Board;
	}

}
