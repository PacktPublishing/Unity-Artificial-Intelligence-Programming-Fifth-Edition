using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;

public class SlotMachineWeighted : MonoBehaviour {
	
	public float spinDuration = 2.0f;
	public int numberOfSym = 10;

    public Text firstReel;
    public Text secondReel;
    public Text thirdReel;
    public Text betResult;

    public Text totalCredits;
    public InputField inputBet;

    private bool startSpin = false;
	private bool firstReelSpinned = false;
	private bool secondReelSpinned = false;
	private bool thirdReelSpinned = false;
	
	private int betAmount = 100;
    private int credits = 1000;

	[Serializable]
	public struct WeightedProbability {
		public int number;
		public int weight;
	}

    private List<WeightedProbability> weightedReelPoll = new List<WeightedProbability>();
    private int zeroProbability = 50;
	
	private int firstReelResult = 0;
	private int secondReelResult = 0;
	private int thirdReelResult = 0;
	
	private float elapsedTime = 0.0f;


	
	// Use this for initialization
	void Start () {
		weightedReelPoll.Add(new WeightedProbability {
			number = 0,
			weight = zeroProbability
		});
		
		int remainingValuesProb = (100 - zeroProbability)/9;
		
		for (int i = 1; i < 10; i++) {
			weightedReelPoll.Add(new WeightedProbability {
				number = i,
				weight = remainingValuesProb
			});
		}
	}


    public void Spin()
    {
        if (betAmount > 0)
        {
            startSpin = true;
        }
        else
        {
            betResult.text = "Insert a valid bet!";
        }
    }


    void OnGUI() {
        try
        {
            betAmount = int.Parse(inputBet.text);
        }
        catch
        {
            betAmount = 0;
        }
        totalCredits.text = credits.ToString();
    }
	
	void checkBet() {
		if (firstReelResult == secondReelResult && secondReelResult == thirdReelResult) {
			betResult.text = "JACKPOT!";
			credits += betAmount * 50;
		}
		else if (firstReelResult == 0 && thirdReelResult == 0) {			
			betResult.text = "YOU WIN " + (betAmount/2).ToString();
            credits -= (betAmount/2);
		}
		else if (firstReelResult == secondReelResult) {			
			betResult.text = "AWW... ALMOST JACKPOT!";
		}
		else if (firstReelResult == thirdReelResult) {			
			betResult.text = "YOU WIN " + (betAmount*2).ToString();
            credits -= (betAmount*2);
		}
		else {
			betResult.text = "YOU LOSE!";
            credits -= betAmount;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {		
		if (startSpin) {
			elapsedTime += Time.deltaTime;
			int randomSpinResult = UnityEngine.Random.Range(0, numberOfSym);
			if (!firstReelSpinned) {
				firstReel.text = randomSpinResult.ToString();
				if (elapsedTime >= spinDuration) {
					int weightedRandom = PickNumber();
                    firstReel.text = weightedRandom.ToString();
					firstReelResult = weightedRandom;
					firstReelSpinned = true;
					elapsedTime = 0;
				}
			}
			else if (!secondReelSpinned) {
				secondReel.text = randomSpinResult.ToString();
				if (elapsedTime >= spinDuration) {
					secondReelResult = randomSpinResult;
					secondReelSpinned = true;
					elapsedTime = 0;
				}
			}
			else if (!thirdReelSpinned) {
				thirdReel.text = randomSpinResult.ToString();
				if (elapsedTime >= spinDuration) {						
					if ((firstReelResult == secondReelResult) &&
						randomSpinResult != firstReelResult) {
						//the first two reels have resulted the same symbol
						//but unfortunately the third reel missed
						//so instead of giving a random number we'll return a symbol which is one less than the other 2
						
						randomSpinResult = firstReelResult - 1;
						if (randomSpinResult < firstReelResult) randomSpinResult = firstReelResult - 1;
						if (randomSpinResult > firstReelResult) randomSpinResult = firstReelResult + 1;
						if (randomSpinResult < 0) randomSpinResult = 0;
						if (randomSpinResult > 9) randomSpinResult = 9;

                        thirdReel.text = randomSpinResult.ToString();	
						thirdReelResult = randomSpinResult;
					}
					else {
						int weightedRandom = PickNumber();
                        thirdReel.text = weightedRandom.ToString();
						thirdReelResult = weightedRandom;
					}
					
					startSpin = false;
					elapsedTime = 0;
					firstReelSpinned = false;
					secondReelSpinned = false;
					
					checkBet();
				}
			}
		}
	}

	private int PickNumber() {
		// Sum the weights of every state.
		var weightSum = weightedReelPoll.Sum(state => state.weight);
		var randomNumber = UnityEngine.Random.Range(0, weightSum);
		var i = 0;
		while (randomNumber >= 0) {
			var candidate = weightedReelPoll[i];
			randomNumber -= candidate.weight;
			if (randomNumber <= 0) {
				return candidate.number;
			}
			i++;
		}
		// It should not be possible to reach this point!
		throw new Exception("Something is wrong in the selectState algorithm!");
	}
}
