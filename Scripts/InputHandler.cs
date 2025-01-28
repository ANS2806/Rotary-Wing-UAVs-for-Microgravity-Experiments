using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] Text resultText;

    public float yPos; 

    public void ValidateInput()
    {
        string input = inputField.text;

        if (float.TryParse(input, out yPos))  // Ensure the input is a valid float
        {
            resultText.text = "Input received!";
            resultText.color = Color.green;
        }
        else
        {
            resultText.text = "Invalid input!";
            resultText.color = Color.red;
        }
    }
}

