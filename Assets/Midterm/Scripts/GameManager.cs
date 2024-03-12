using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Flow { 
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
        private int time;

        //parent of cells
        [SerializeField] Transform GridRoot;
        //template to populate gird
        [SerializeField] GameObject cellPrefab;
        [SerializeField] GameObject colorEnds;
        //[SerializeField] GameObject winLabel;
        //[SerializeField] TextMeshProUGUI timeLabel;

        //variables that are set in one method and used in another without the methods calling each other to send the value
        private Transform prevLineStart;
        private Transform prevCell;
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
            nRows = grid.GetLength(0);
            nCols = grid.GetLength(1);

            //create bool[] w/ dimensions of grid
            hasColor = new bool[nRows, nCols];  
            //determines if a cell is already filled/blocked/has a color
            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                { 
                    if (grid[i,j] > 0) hasColor[i,j] = true;    
                }
            }
            //finds where the ends of the colors are at the start and sets them as a different type of cell prefab
            //if no color, regular cell perfab
            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    if (hasColor[i, j]) 
                    {
                        Instantiate(colorEnds, GridRoot);
                    }
                    else Instantiate(cellPrefab, GridRoot);
                }
            }
            //shows ends of the colors
            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    row = i; col = j;
                    Transform cell = GetCurrentCell();
                    switch (grid[i,j])
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
            }
            row = 0;
            col = 0;    
            SelectCurrentCell();

            input = new Input();
            click = input.Player.Click;
            move = input.Player.Move;

            redLineComplete = false;
            greenLineComplete = false;
            blueLineComplete = false;
            yellowLineComplete = false;
            purpleLineComplete = false;
        }

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
            upWasPressed = Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame;
            downWasPressed = Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame;
            leftWasPressed = Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame;
            rightWasPressed = Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame;
            HandleMovement();
        }

        void DrawLines(string color)
        { 
            //check hasColor[] to see if the current cell already is filled
            Transform cell = GetCurrentCell();
            if (hasColor[row, col]) return; //cannot draw a line in that space

            Transform colorLayer = cell.Find(color);
            colorLayer.gameObject.SetActive(true);
        }
        void CancelLine(string color)
        { 
            //clears out all instances of a certain line color 
            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    Transform cell = GridRoot.GetChild((i * nCols) + j);
                    Transform colorLayer = cell.Find(color);
#pragma warning disable CS0618 // Type or member is obsolete
                    if (colorLayer.gameObject.active) colorLayer.gameObject.SetActive(false);
#pragma warning restore CS0618 // Type or member is obsolete
                }
            }
        }

        Transform GetCurrentCell()
        {
            int index = (row * nCols) + col;
            return GridRoot.GetChild(index);
        }
        void SelectCurrentCell()
        { 
            Transform cell = GetCurrentCell();
            Transform cursor = cell.Find("Cursor");
            cursor.gameObject.SetActive(true);
            prevCell = cell;
        }
        void DeselectCurrentCell()
        {
            if (prevCell != null)
            {
                Transform cell = GetCurrentCell();
                Transform cursor = cell.Find("Cursor");
                cursor.gameObject.SetActive(false);
            }
        }
        void SelectLineStart()
        {
            Transform cell = GetCurrentCell();
            if (cell.gameObject.tag == "Color End") 
            { 
                Transform select = cell.Find("Selected");
                select.gameObject.SetActive(true);

                //set a bool to the color of the end selected
                //check to see if the line was completed by comparing the colors of the ends form prevLineStart & current select
                //if selecting the current cell of a color that is not finished, reset all lines of that color

                prevLineStart = select;
            }
        }
        void DeselectLineStart()
        {
            if (prevLineStart != null) //prevents error when clicking for the first time
            { 
                prevLineStart.gameObject.SetActive(false); 
            }
        }

        void HandleMovement() 
        {
            if (upWasPressed) MoveVertical(-1);
            if (downWasPressed) MoveVertical(1);
            if (leftWasPressed) MoveHorizontal(-1);
            if (rightWasPressed) MoveHorizontal(1);
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
            //use to identify where the player clicked, if the cell is a starting cell, and 
            DeselectLineStart();
            SelectLineStart();
        }

    }

}
