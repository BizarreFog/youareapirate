using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public List<Chest> chests = new List<Chest>();

    private int chestsToFind = 0;
    private int currentChestsFound = 0;

    public Chest currentFavored;

    public TextMeshProUGUI status;

    public void ChestFound(Chest _chest)
    {
        if (chests.Contains(_chest))
        {
            currentChestsFound++;
            chests.Remove(_chest);

            if(chests.Count > 1)
            {
                GetNewFavorite();   
            }
        }

        if(currentChestsFound >= chestsToFind)
        {
            WinGame();
        }

        UpdateStatusText();
    }

    void UpdateStatusText()
    {
        status.text = currentChestsFound.ToString() + " out of " + chestsToFind + " chests found.";
    }

    public void RegisterChest(Chest _chest)
    {
        if (!chests.Contains(_chest))
        {
            chests.Add(_chest);
            chestsToFind = chests.Count;
        }

        UpdateStatusText();
        GetNewFavorite();
    }

    public void GetNewFavorite()
    {
        int randomIndex = Random.Range(0, chests.Count);

        if (chests.Contains(chests[randomIndex]))
        {
            currentFavored = chests[randomIndex];
        }
        else
        {
            if (chests.Contains(chests[0]))
            {
                currentFavored = chests[0];
            }
        }
    }

    void WinGame()
    {
        Debug.Log("You won buddy");
    }

}
