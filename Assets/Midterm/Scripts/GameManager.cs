using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Flow
{
    public class GameManager : MonoBehaviour
    {
        //1 = red, 2 = green, 3 = blue, 4 = yellow, 5 = purple
        private int[,] grid =
            {  //top left 0,0
                { 0, 0, 0, 0, 2 },
                { 4, 5, 0, 0, 1 },
                { 3, 0, 0, 0, 0 },
                { 0, 5, 4, 2, 0 },
                { 0, 0, 3, 1, 0 },
            }; //bottom right 4,4

        private bool[,] hasColor;

        private int nRows;
        private int nCols;
        private int row;
        private int col;

        //parent of cells
        [SerializeField] Transform GridRoot;
        //template to populate gird
        [SerializeField] GameObject cellPrefab;
        [SerializeField] GameObject colorEnds;
        [SerializeField] GameObject winLabel;

        [SerializeField] TextMeshProUGUI redComplete;
        [SerializeField] TextMeshProUGUI greenComplete;
        [SerializeField] TextMeshProUGUI blueComplete;
        [SerializeField] TextMeshProUGUI yellowComplete;
        [SerializeField] TextMeshProUGUI purpleComplete;

        //variables that are set in one method and used in another without the methods calling each other to send the value
        private Transform prevLineStart;
        private Transform prevCell;
        private bool lineEndSelected;
        private bool lineDisconnected;
        private string currentColor;

        //win conditions
        bool redLineComplete;
        bool greenLineComplete;
        bool blueLineComplete;
        bool yellowLineComplete;
        bool purpleLineComplete;

        //checking move direction
        bool upWasPressed;
        bool downWasPressed;
        bool leftWasPressed;
        bool rightWasPressed;

        //input actions
        public Input input;
        private InputAction click;
        private InputAction move;

        private void Awake()
        {
            //set up game board
            nRows = grid.GetLength(0);
            nCols = grid.GetLength(1);

            //create bool[] w/ dimensions of grid
            hasColor = new bool[nRows, nCols];
            //determines if a cell has a color in it (color/line ends) at the start
            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    if (grid[i, j] > 0) hasColor[i, j] = true;
                }
            }
            //finds where the ends of the colors are at the start and sets them as a different type of cell prefab
            //if no color, set as a regular cell perfab
            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    if (hasColor[i, j])
                    {
                        Instantiate(colorEnds, GridRoot);

                        //shows the specific color of the color end
                        row = i; col = j;
                        Transform cell = GetCurrentCell();
                        switch (grid[i, j])
                        {
                            case 1:
                                Transform red = cell.Find("Red");
                                red.gameObject.SetActive(true);
                                break;
                            case 2:
                                Transform green = cell.Find("Green");
                                green.gameObject.SetActive(true);
                                break;
                            case 3:
                                Transform blue = cell.Find("Blue");
                                blue.gameObject.SetActive(true);
                                break;
                            case 4:
                                Transform yellow = cell.Find("Yellow");
                                yellow.gameObject.SetActive(true);
                                break;
                            case 5:
                                Transform purple = cell.Find("Purple");
                                purple.gameObject.SetActive(true);
                                break;
                        }
                    }
                    else Instantiate(cellPrefab, GridRoot);
                }
            }
            //reset rows & cols, select the first cell in the grid
            row = 0;
            col = 0;
            SelectCurrentCell();
            //set up player inputs
            input = new Input();
            click = input.Player.Click;
            move = input.Player.Move;
            //win conditions are all false at start
            redLineComplete = false;
            greenLineComplete = false;
            blueLineComplete = false;
            yellowLineComplete = false;
            purpleLineComplete = false;

            lineDisconnected = false;
            lineEndSelected = false;
        }

        //set up player inputs
        private void OnEnable()
        {
            click.Enable();
            click.performed += Click;
            move.Enable();
        }
        private void OnDisable()
        {
            click.Disable();
            move.Disable();
        }

        // Update is called once per frame
        void Update()
        {
            //code idea from https://docs.unity3d.com/Packages/com.unity.inputsystem@1.7/api/UnityEngine.InputSystem.Controls.ButtonControl.html#UnityEngine_InputSystem_Controls_ButtonControl_wasPressedThisFrame
            //way of keeping each key to one specific input but checking which specific key was pressed to get a direction for movement
            upWasPressed = Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame;
            downWasPressed = Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame;
            leftWasPressed = Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame;
            rightWasPressed = Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame;
            HandleMovement();
        }

        void DrawLines(Transform cell)
        {
            if (cell.gameObject.tag == "Color End" || lineEndSelected == false) return;

            //does not draw if there is not line end selected (aka start of game)
            if (lineDisconnected)
            {
                Debug.Log($"lineDisconnected: {lineDisconnected}");
                return;
            }

            //clear the color in that space if there isn't already a color in the space
            //essential overrides the current color there
            if (hasColor[row, col])
            { 
                string lineColor = FindLineColor(cell);
                if (lineColor != currentColor)
                {
                    SetLineFinished(lineColor, false);
                    ClearLine(lineColor);
                    Debug.Log("reset end color");                
                }
            }

            Transform colorLayer = cell.Find(FindLineColor(prevLineStart));
            colorLayer.gameObject.SetActive(true);
            hasColor[row, col] = true;
        }
        void ClearLine(string color)
        {
            for (int r = 0; r < grid.GetLength(0); r++)
            {
                for (int c = 0; c < grid.GetLength(1); c++)
                {
                    Transform currentCell = GridRoot.GetChild((r * nCols) + c);
                    //if the cell it's looking at is a color end, do not reset it
                    if (currentCell.gameObject.tag == "Color End") break;
                    //turn off the specified color 
                    Transform colorLayer = currentCell.Find(color);
                    if (hasColor[r,c] && FindLineColor(currentCell) == color)
                    //if (colorLayer != null) 
                    {
                        colorLayer.gameObject.SetActive(false);
                        hasColor[r, c] = false;
                    }
                }
            }
        }

        Transform GetCurrentCell()
        {
            //finds the current cell in the grid
            int index = (row * nCols) + col;
            return GridRoot.GetChild(index);
        }

        void SelectCurrentCell()
        {
            //shows what cell the player is on by displaying an alternate outline
            Transform cell = GetCurrentCell();
            Transform cursor = cell.Find("Cursor");
            cursor.gameObject.SetActive(true);
            prevCell = cell;
        }
        void DeselectCurrentCell()
        {
            if (prevCell != null) //prevents error when clicking for the first time & prevCell hasn't been assigned yet
            {
                Transform cell = GetCurrentCell();
                Transform cursor = cell.Find("Cursor");
                cursor.gameObject.SetActive(false);
            }
        }
        void SelectLineStart()
        {
            //shows which end of a color (specifically) is selected by displaying a unique outline 
            Transform cell = GetCurrentCell();
            if (cell.gameObject.tag == "Color End") //check to make sure the cell is the end of a color
            {
                Transform select = cell.Find("Selected");
                select.gameObject.SetActive(true); //draws the outline

                //check to see if the line was completed by comparing the colors of the ends form prevLineStart & current select
                //if selecting the current cell of a color that is not finished, reset all lines of that color
                currentColor = FindLineColor(cell);
                if (prevLineStart != null && currentColor == FindLineColor(prevLineStart))
                { 
                    SetLineFinished(currentColor, true);
                    CheckEnding();
                }

                prevLineStart = cell; //the next previous start of the line is the current start of a line
                lineEndSelected = true;
                lineDisconnected = false;
            }
        }
        void DeselectLineStart()
        {
            if (prevLineStart != null) //prevents error when clicking for the first time & prevLineStart hasn't been assigned yet
            {
                prevLineStart.Find("Selected").gameObject.SetActive(false);
                lineDisconnected = false;
                lineEndSelected = false; 
            }
        }

        string FindLineColor(Transform line)
        {
            if (line.Find("Red").gameObject.active) return "Red";
            if (line.Find("Green").gameObject.active) return "Green";
            if (line.Find("Blue").gameObject.active) return "Blue";
            if (line.Find("Yellow").gameObject.active) return "Yellow";
            if (line.Find("Purple").gameObject.active) return "Purple";
            return "none";
        }
        void SetLineFinished(string lineColor, bool finished)
        {
            //changes the status of a complete line boolean to determine if the game ended or not
            switch (lineColor)
            {
                case "Red":
                    redLineComplete = finished;
                    redComplete.text = $"Red Line Complete: {finished}";
                    break;
                case "Green":
                    greenLineComplete = finished;
                    greenComplete.text = $"Green Line Complete: {finished}";
                    break;
                case "Blue":
                    blueLineComplete = finished;
                    blueComplete.text = $"Blue Line Complete: {finished}";
                    break;
                case "Yellow":
                    yellowLineComplete = finished;
                    yellowComplete.text = $"Yellow Line Complete: {finished}";
                    break;
                case "Purple":
                    purpleLineComplete = finished;
                    purpleComplete.text = $"Purple Line Complete: {finished}";
                    break;
            }
        }
        void CheckEnding()
        {
            if (redLineComplete && greenLineComplete && blueLineComplete && yellowLineComplete && purpleLineComplete)
            {
                winLabel.SetActive(true); 
            }
        }

        void HandleMovement()
        {
            //updates cursor
            if (upWasPressed) MoveVertical(-1);
            if (downWasPressed) MoveVertical(1);
            if (leftWasPressed) MoveHorizontal(-1);
            if (rightWasPressed) MoveHorizontal(1);
            //updates lines
            DrawLines(GetCurrentCell());
        }

        public void MoveHorizontal(int amt)
        {
            DeselectCurrentCell();
            //move to new cell & make sure it stays within bounds
            col += amt;
            col = Mathf.Clamp(col, 0, nCols - 1);
            //select new cell
            SelectCurrentCell();
        }
        public void MoveVertical(int amt)
        {
            DeselectCurrentCell();
            //move to new cell & make sure it stays within bounds
            row += amt;
            row = Mathf.Clamp(row, 0, nRows - 1);
            //select new cell
            SelectCurrentCell();
        }

        void Click(InputAction.CallbackContext context)
        {
            //called when player clicks
            //use to identify where the player clicked, & if the cell is a line end
            DeselectLineStart();
            SelectLineStart();
        }

    }
}