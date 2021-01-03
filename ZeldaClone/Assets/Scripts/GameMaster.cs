using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{

    public GameObject lifeBar;
    public GameObject scoreText;
    private string startText;

    public GameObject emptyHearth;
    public GameObject halfHearth;
    public GameObject fullHearth;
    [HideInInspector]
    public static float Score;

    void Start()
    {   
        startText = scoreText.GetComponent<Text>().text;
        UpdatePlayerHealth(FindObjectOfType<PlayerController>().GetComponent<InteractAble>().stats.Health);
        UpdatePlayerScore();
    }

    public void UpdatePlayerHealth(float health)
    {
        for (int i = 0; i < lifeBar.transform.childCount; i++)
        {
            GameObject child = lifeBar.transform.GetChild(i).gameObject;
            Destroy(child);
        }

        for (int i = 0; i <= health - 1; i++)
            hearthInstantiate(fullHearth);

        float lastHearth = health % 1;

        if (lastHearth >= 0.5)
            hearthInstantiate(halfHearth);

        else if (lastHearth > 0)
            hearthInstantiate(emptyHearth);
    }

    public void UpdatePlayerScore()
    {
        scoreText.GetComponent<Text>().text = startText + Score;
    }

    public void pauseProjectileMovement()
    {
        projectileWeakness[] allProjectiles = FindObjectsOfType<projectileWeakness>();
        for (int i = 0; i < allProjectiles.Length; i++)
            allProjectiles[i].pauseMovement();

    }
    public void resumeProjectileMovement()
    {
        projectileWeakness[] allProjectiles = FindObjectsOfType<projectileWeakness>();
        for (int i = 0; i < allProjectiles.Length; i++)
            allProjectiles[i].resumeMovement();
    }

    private void hearthInstantiate(GameObject hearth)
    {
        GameObject child = Instantiate(hearth) as GameObject;
        child.transform.SetParent(lifeBar.transform, false);
    }
}
