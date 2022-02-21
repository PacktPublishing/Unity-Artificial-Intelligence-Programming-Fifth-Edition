using UnityEngine;
using UnityEngine.UI;

public class DiceGameLoaded : MonoBehaviour {

    public string inputValue = "1";

    public Text outputText;
    public InputField inputField;
    public Button button;

    int throwDice() {
        Debug.Log("Throwing dice...");

        int randomProbability = Random.Range(0, 100);
        int diceResult;
        if (randomProbability < 35) {
            diceResult = 6;
        } else {
            diceResult = Random.Range(1, 5);
        }

        Debug.Log("Result: " + diceResult);

        return diceResult;
    }

    public void processGame() {
        inputValue = inputField.text;
        try {
            int inputInteger = int.Parse(inputValue);
            int totalSix = 0;
            for (var i = 0; i < 10; i++) {
                var diceResult = throwDice();
                if (diceResult == 6) { totalSix++; }

                if (diceResult == inputInteger) {
                    outputText.text = $"DICE RESULT: {diceResult} \r\nYOU WIN!";
                } else {
                    outputText.text = $"DICE RESULT: {diceResult} \r\nYOU LOSE!";
                }
            }
            Debug.Log($"Total of six: {totalSix}");
        } catch {
            outputText.text = "Input is not a number!";
            Debug.LogError("Input is not a number!");
        }
    }

}
