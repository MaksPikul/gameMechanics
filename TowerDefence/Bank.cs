using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 175;
    [SerializeField] int currentBalance;
    public int CurrentBalance {get{return currentBalance;}}

    [SerializeField] TextMeshProUGUI displayBalance;

    void Awake(){
        currentBalance = startingBalance;
        UpdateDisplay();
    }

    public void Deposit(int amount){
        currentBalance += Mathf.Abs(amount);
        UpdateDisplay();
    }

    public void Withdraw(int amount){
        currentBalance -= Mathf.Abs(amount);
        UpdateDisplay();

        if (currentBalance <0){
            reloadScene();
        }
    }

    void UpdateDisplay(){
        displayBalance.text = "Gold: " + currentBalance;
    }

    void reloadScene(){
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
