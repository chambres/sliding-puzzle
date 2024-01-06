using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<Tile> Tiles;
    public Tile TilePrefab;
    public GameObject boardCanvas;
    private Camera _camera; 
    public int[,] board;
    public float startX=-225, startY=225, shift=150;
    private List<int> availableNumbers;

    public delegate void SomeAction();

    public static event SomeAction OnTileMoved;

    public static event SomeAction OnWonCondition;


    public bool isImageOn;

    public void Start() {
        InitiateBoard();

        //Debug.Log(availableNumbers[0]);

        _camera = Camera.main;
        ResetBoard();
    }

    public void switchisImageOn(){
        isImageOn = !isImageOn;
    }

    private void Update() {
        TurnOnImagesOrNot(isImageOn);
    }

    void TurnOnImagesOrNot(bool determiner){
        if(determiner){
            foreach(Tile tile in Tiles){
                tile.transform.Find("image").GetComponent<Image>().enabled = true;
            }
        }
        else{
            foreach(Tile tile in Tiles){
                tile.transform.Find("image").GetComponent<Image>().enabled = false;
            }
        }
    }

    public void InitiateBoard() {
        board = new int[4,4];
        availableNumbers = new List<int>();
        Tiles = new List<Tile>();

        FillListWithAvailableNumbers(availableNumbers);
    }

    public void FillListWithAvailableNumbers(List<int> list) {
        list.Clear();
        for (int i=0; i<board.Length; i++) {
            list.Add(i);
        }
    }

    public TextMeshPro WinText;
    public TMP_Text WWinText;

    public void ButtonResetBoard(){
        //if(WWinText.text != ""){
            ResetBoard();
            GameObject.Find("Move Count").GetComponent<MoveCounterBehaviour>().ResetCount();
            WWinText.text = "";
        //}

    }

    public void ResetBoard() {
        removeTiles();
        InitiateBoard();
        initiateShuffledBoard(board);
        if (!CheckSolvability(board)) {
            ResetBoard();
        }
    }

    public void OnTileMove() {
        //Debug.Log("Caught Tile move.");
        OnTileMoved?.Invoke();
        Debug.Log("won: " + checkWinCondition());
        if (checkWinCondition()) {
            for (int i=0; i<board.GetLength(0); i++) {
                for (int j=0; j<board.GetLength(1); j++) {
                    if (i==board.GetLength(0)-1 && j==board.GetLength(1)-1)
                        break;
                    int index = i*board.GetLength(0)+j;
                    WWinText.text = "You Won!!! Took " + GameObject.Find("Move Counter").GetComponent<TMP_Text>().text + " moves.";
                }
            }
            Debug.Log("You Won!!!");
            OnWonCondition?.Invoke();
        }
    }

    public bool checkWinCondition() {
        for (int i=0; i<board.GetLength(0); i++) {
            for (int j=0; j<board.GetLength(1); j++) {
                if (i==board.GetLength(0)-1 && j==board.GetLength(1)-1)
                        break;
                int index = i*board.GetLength(0)+j;
                if (board[i, j] != (index+1)) {
                    return false;
                }
            }
        }
        return true;
    }

    public bool CheckSolvability(int[,] board) {
        int sum = 0;
        var array = board.Cast<int>().ToArray();
        for (int i=0; i<array.Length; i++) {
        //     Debug.Log(" index="+i+" row="+(i/board.GetLength(0)+1+" number="+array[i]));
            if (array[i]==0) {
                sum += i/board.GetLength(0)+1;
                continue;
            }
            for(int j=i;  j<array.Length; j++) {
                if (array[j]<array[i] && array[j]!=0) {
                    sum++;
                }
            }
        }
        Debug.Log(sum);
        return (sum%2==0)? true : false;
    }

    public void initiateShuffledBoard(int[,] board) {
        int pos = 0;
        int count = 0;
        for (int i=0; i<board.GetLength(0); i++) {
            for (int j=0; j<board.GetLength(1); j++) {

                int index = availableNumbers[Random.Range(0, availableNumbers.Count)];
                count++;
                Debug.Log("pos: " + count);


                foreach(var record in availableNumbers) {
                    Debug.Log("availableNumbers"+record);
                }
                availableNumbers.Remove(index);


                Debug.Log(index);

                board[i, j] = index;
                //Debug.Log("index="+index+" i="+i+" j="+j);
                if (index == 0)
                    continue;
                Tiles.Add(generateTile(index, i, j));
                pos++;
            }
        }
    }

    // public void initiateShuffledBoard(int[,] board) {
    // int pos = 0;
    // for (int i = 0; i < board.GetLength(0); i++) {
    //     for (int j = 0; j < board.GetLength(1); j++) {
    //         if (i == board.GetLength(0) - 1 && j == board.GetLength(1) - 1) {
    //             board[i, j] = 0;
    //         } else {
    //             board[i, j] = pos + 1;
    //             Tiles.Add(generateTile(pos + 1, i, j));
    //             pos++;
    //         }
    //     }
    // }
    // }

    public Tile generateTile(int index, int i, int j) {
        Tile go = Instantiate(TilePrefab);
        go.transform.SetParent(boardCanvas.transform);
        go.name = "Square ("+index+")";
        go.transform.localPosition = new Vector2(startX+shift*j, startY-shift*i);
        go.GetComponent<Tile>().initialize(index, j, i);
        go.moveAction += OnTileMove;
    
        return go;
    }

    public int getNumberFromAvailable(List<int> an) {
        int randomNumber = Random.Range(0, an.Count);
        an.Remove(randomNumber);
        return randomNumber;
    }

    public void removeTiles() {
        foreach(var Tile in Tiles) {
            Destroy(Tile.gameObject);
        }
    }
}