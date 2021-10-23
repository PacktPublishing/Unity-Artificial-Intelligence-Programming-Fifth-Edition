using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour {
	
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
	
	private int betAmount;
    private int credits = 1000;
	
	private int firstReelResult = 0;
	private int secondReelResult = 0;
	private int thirdReelResult = 0;
	
	private float elapsedTime = 0.0f;
	
	public void Spin()
    {
        if (betAmount > 0)
        {
            startSpin = true;
        } else
        {
            betResult.text = "Insert a valid bet!";
        }
    }

    private void OnGUI()
    {
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
			betResult.text = "YOU WIN!";
            credits += 50*betAmount;
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
			int randomSpinResult = Random.Range(0, numberOfSym);
			if (!firstReelSpinned) {
				firstReel.text = randomSpinResult.ToString();
				if (elapsedTime >= spinDuration) {
					firstReelResult = randomSpinResult;
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
					thirdReelResult = randomSpinResult;
					
					startSpin = false;
					elapsedTime = 0;
					firstReelSpinned = false;
					secondReelSpinned = false;
					
					checkBet();
				}
			}
		}
	}
}
