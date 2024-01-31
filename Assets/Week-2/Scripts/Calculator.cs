using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Calculator : MonoBehaviour
{
    public TextMeshProUGUI output;

    // float variable to store the previous input value when performaing calculations
    private float prevInput;

    // bool variable to flip it to true/false if we should clear the prior input when typing in values.
    // Example, if we type in the value 402 and then press the + button, the next value I enter should replace the 402 I previously entered
    private bool clearPrevInput = false;

    private EquationType equationType;

    private void Start() {
        Clear(); //immedietly resets the calculator- prevents anything funky from happening 
    }

    //Resets the state of the calculator
    //Makes the calculator display 0
    //Resets the previous input & the equation type
    public void Clear()
    {
        clearPrevInput = true;
        output.text = "0";
        prevInput = 0;

        equationType = EquationType.None;
    }


    //Called by any of the number buttons
    //If an operation was called before pressing a number, the output display is reset
    //Adds the number pressed onto the end of the number 
    public void AddInput(string input)
    {
        if (clearPrevInput) {
            output.text = string.Empty;
            clearPrevInput = false;
        }

        output.text += input;
        //Debug.Log(input);
    }


    //Called when pressing any of the operation buttons
    //Stores the current number, marks the output text to clear for the next number to be entered, and sets the equation type
    //for when it's time to calculate
    public void SetEquationAsAdd()
    {
        prevInput = float.Parse(output.text);
        clearPrevInput = true;
        equationType = EquationType.ADD;
    }
    public void SetEquationAsSubtract()
    {
        prevInput = float.Parse(output.text);
        clearPrevInput = true;
        equationType = EquationType.SUBTRACT;
    }
    public void SetEquationAsMultiply()
    {
        prevInput = float.Parse(output.text);
        clearPrevInput = true;
        equationType = EquationType.MULTIPLY;
    }
    public void SetEquationAsDivide()
    {
        prevInput = float.Parse(output.text);
        clearPrevInput = true;
        equationType = EquationType.DIVIDE;
    }


    // takes current input & converts to a float
    // sets output to the result of the operation
    // changes prevInput to the current display value so if you keep doing operations it will keep track of the answer of the previous
    // operation instead of the last number you typed
    public void Add() {
        float currentInput = float.Parse(output.text);
        output.text = (prevInput + currentInput).ToString();
        prevInput += currentInput;
    }
    public void Subtract() {
        float currentInput = float.Parse(output.text);
        output.text = (prevInput - currentInput).ToString();
        prevInput -= currentInput;
    }
    public void Multiply() {
        float currentInput = float.Parse(output.text);
        output.text = (prevInput * currentInput).ToString();
        prevInput *= currentInput;  
    }
    public void Divide() {
        float currentInput = float.Parse(output.text);
        output.text = (prevInput / currentInput).ToString();
        prevInput /= currentInput;
    }


    //Called via the equals button: determines equations type then sublets the operation to the proper method
    //condensed code- should it be???
    public void Calculate()
    {
        if (equationType == EquationType.ADD) Add();
        else if (equationType == EquationType.SUBTRACT) Subtract();
        else if (equationType == EquationType.MULTIPLY) Multiply();
        else if (equationType == EquationType.DIVIDE) Divide();
    }

    //used to store value of operation from pressing the button to determining what type of operation is used
    public enum EquationType
    {
        None = 0,
        ADD = 1,
        SUBTRACT = 2,
        MULTIPLY = 3,
        DIVIDE = 4
    }
}
