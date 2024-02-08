using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Battleship { 
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private int[,] grid = new int[,] {
            { 1, 1, 0, 0, 1 }, //top left is (0,0)
            { 0, 0, 0, 0, 0 },
            { 0, 0, 1, 0, 1 },
            { 1, 0, 1, 0, 0 },
            { 1, 0, 1, 0, 1 } //bottom right is (4,4)
        };
    
        private bool[,] hits; //tracks where the player has fired

        //total number of rows and columns for board
        private int nRows;
        private int nCols;

        //current row/col
        private int row;
        private int col;

        private int score; //correctly hit ships
        private int time; //time game has been running 

        [SerializeField] Transform gridRoot; //parent of all cells
        [SerializeField] GameObject cellPrefab; //used to populate grid
        [SerializeField] GameObject winLabel;
        [SerializeField] TextMeshProUGUI scoreLabel;
        [SerializeField] TextMeshProUGUI timeLabel;


        private void Awake() {
            //initialize rows/cols
            nRows = grid.GetLength(0);
            nCols = grid.GetLength(1);  
            //create identical 2D array of above for bool
            hits = new bool[nRows, nCols];

            //populate grid with loop- 
            for (int i = 0; i < nRows * nCols; i++) {
                Instantiate(cellPrefab, gridRoot); //creats an instance of the prefab & childs it to the gridRoot object
            }
            SelectCurrentCell();
            InvokeRepeating("IncrementTime", 1f, 1f);
        }

        void IncrementTime() {
            time++;
            //update time label 
            //format mm:ss where ss only has 2 digits
            timeLabel.text = string.Format("{0}:{1}", time/60, (time%60).ToString("00"));
        }

        Transform GetCurrentCell() { //finds the child index of the cell thats part of the grid by the following calculation 
            int index = (row * nCols) + col;
            return gridRoot.GetChild(index);
        }

        void SelectCurrentCell() { //get current cell and turn cursor image on 
            Transform cell = GetCurrentCell();
            Transform cursor = cell.Find("Cursor");
            cursor.gameObject.SetActive(true);
        }
        void DeselectCurrentCell() { //get current cell and turn cursor image off 
            Transform cell = GetCurrentCell();
            Transform cursor = cell.Find("Cursor");
            cursor.gameObject.SetActive(false);
        }

        public void MoveHorizontal(int amount) { 
            //moving to a new cell: deselect current cell
            DeselectCurrentCell();
            col += amount;
            //check bounds of grid : ensures col stays within grid
            col = Mathf.Clamp(col, 0, nCols -1); //what does this function do?
            SelectCurrentCell();
        }
        public void MoveVertical(int amount) {
            //moving to a new cell: deselect current cell
            DeselectCurrentCell();
            row += amount;
            //check bounds of grid : ensures row stays within grid
            row = Mathf.Clamp(row, 0, nRows - 1);
            SelectCurrentCell();
        }
        public void Fire() {
            //check if cell in hit[] is true/false 
            //if true, already fired, do nothing
            if (hits[row, col]) return; //terminates method early

            //if false - mark as true (fired)
            hits[row, col] = true;
            if (grid[row, col] == 1) { //if is a ship, hit & update score
                ShowHit();
                UpdateScore();
                TryEndGame();
            }
            else { //no ship, open water, miss
                ShowMiss();
            }
        }

        void ShowHit() { //get current cell & set hit image to on 
            Transform cell = GetCurrentCell();
            Transform hit = cell.Find("Hit");
            hit.gameObject.SetActive(true);
        }
        void ShowMiss() { //get current cell & set miss image to on
            Transform cell = GetCurrentCell();
            Transform miss = cell.Find("Miss");
            miss.gameObject.SetActive(true);
        }

        //add to score & update label
        void UpdateScore() {
            score++;
            scoreLabel.text = string.Format("Score: {0}", score);
        }

        void TryEndGame() { //check rows & cols for ships & if they're hit
            for (int i = 0; i < nRows; i++) {
                for (int j = 0; j < nCols; j++) {
                    //if a cell is not a ship, ignore
                    if (grid[i, j] == 0) continue; //is this neccesary? does the continue keyword mean "else return" but just for the loop?
                    //if a cell is a ship but has not been hit, return (game has not finished)
                    if (hits[i, j] == false) return;
                }
            }
            //if loop completes then all ships have been hit, set win label
            winLabel.SetActive(true);
            CancelInvoke("IncrementTime");
        }

        public void Restart() {
            DeselectCurrentCell();
            time = 0; score = -1;
            UpdateScore();
            if (winLabel.active) {
                InvokeRepeating("IncrementTime", 1f, 1f);
            }
            winLabel.SetActive(false);
            hits = new bool[nRows, nCols];

            for (int r = 0; r < nRows; r++) {
                for (int c = 0; c < nCols; c++) {
                    row = r; col = c;
                    Transform cell = GetCurrentCell();
                    Transform hit = cell.Find("Hit");
                    Transform miss = cell.Find("Miss");
                    hit.gameObject.SetActive(false);
                    miss.gameObject.SetActive(false);
                }
            }
            row = 0; col = 0;
            SelectCurrentCell();

            int random;
            for (int i = 0; i < grid.GetLength(0); i++) {
                for (int j = 0; j < grid.GetLength(1); j++) {
                    random = Random.Range(0, 11);
                    if (random > 6) {
                        grid[i, j] = 1;
                    } else { 
                        grid[i, j] = 0; 
                    }
                }
            }

        }

    }
    
}
