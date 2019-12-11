using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enums{

	public enum BallColor{
		white,
		black,
		brown,
		green,
		purple,
		orange,
		red,
		blue,
		yellow
	}
	public enum PlayerColor{
		red,
		blue,
		yellow
	}
	public enum BallType {
		normal,
		powerup,
		rainbow
	}
	public enum GameMode{
		Marathon,
		Arcade,
		Puzzle,
		Story,
		Mission,
		VS,
		Battle
	}
	public enum PlayerNumber{
		one,
		two,
		three,
		four,
		cpuOne,
		cpuTwo,
		cpuThree,
		cpuFour
	}
	public enum Orientation {
		Horizontal,
		Vertical
		}
	public enum Direction {
		up,
		down,
		left,
		right
	}
	public enum Difficulty{
		VeryEasy,
		Easy,
		Normal,
		Hard,
		VeryHard
	}
	public enum cpuDifficulty{
		VeryEasy,
		Easy,
		Normal,
		Hard,
		VeryHard
	}
	public enum CPUActions{
		Move,
		FindBall,
		ChangeColor,
		WhiteOut,
		CheckColumns,
		MakeBrown,
		Nothing
	}
	public enum Powerups{
		RainbowOut,
		MegaWhiteOut,
		ColorBomb,
		BlackBomb
	}
	public enum PlayerType{
		Human,
		CPU,
		None
	}
	public enum MissionType{
		ClearColor,
		ScoreAttack,
		TimeAttack,
		AvoidColor,
		Rensa
	}
}