using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseButton : MonoBehaviour
{
    [HideInInspector]
    public bool IsActive = false;

    private int toggleValue = 1;

    [GiveTag]
    public string[] InteractWith = new string[] { };

    [SerializeField]
    private bool StayDown; // if triggered, stay down.
    private bool isDown;

    public bool HideOnInteraction; // Hide the sprite if the button is active.

    [Header("Item requirments")]

    [SerializeField]
    private bool useItemIndex;
    public int itemIndex;

    [Header("Trigger behavior")]

    [SerializeField]
    private bool useTriggerIndex;
    public int TriggerIndex; // Other buttons ID

    [SerializeField]
    private int TriggerCount; // Amount of buttons that need to be down before you do your shit. ?? Only show if TriggerIndex is used?
    [HideInInspector]
    public int triggerCounter;

    [Header("Sounds")]
    [SerializeField]
    private bool Sounds;
    [SerializeField]
    private AudioClip ActivationSound;
    [SerializeField]
    private AudioClip DeactivationSound;

    private AudioSource myAudio;

    private List<BaseButton> otherButtons = new List<BaseButton>();
    private List<GameObject> brothers = new List<GameObject>();

    void Start()
    {
        if (Sounds)
            myAudio = GetComponent<AudioSource>();

        // Find other buttons with same TriggerIndex
        otherButtons.AddRange(FindObjectsOfType<BaseButton>());

        if (useTriggerIndex)
        {
            for (int i = 0; i < otherButtons.Count; i++)
            {
                if (!otherButtons[i].GetComponent<BaseButton>().useTriggerIndex)
                    continue;

                if (otherButtons[i].GetComponent<BaseButton>().TriggerIndex == TriggerIndex)
                {
                    brothers.Add(otherButtons[i].gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        ValidityCheck(obj.gameObject, +1);
    }

    private void OnTriggerExit2D(Collider2D obj)
    {
        ValidityCheck(obj.gameObject, -1);
    }

    private void ValidityCheck(GameObject obj, int change)
    {
        for (int i = 0; i < InteractWith.Length; i++)
            if (obj.tag == InteractWith[i])
            {
                if (useItemIndex)
                {
                    if (obj.GetComponent<ItemIndex>()) // Incase the object dosen't have the script "ItemIndex" on it we dont want error message spam.
                    {
                        if (obj.GetComponent<ItemIndex>().Index == itemIndex)
                        {
                            ChangeCount(change);

                            if (obj.GetComponent<ItemIndex>().DestroyOnUse)
                                Destroy(obj);
                        }                     
                    }
                }
                else if (!useItemIndex)
                    ChangeCount(change);
            }
    }

    private void ChangeCount(int change)
    {
        if (useTriggerIndex)
            for (int i = 0; i < brothers.Count; i++)
                brothers[i].GetComponent<BaseButton>().triggerCounter += change;
        else
            triggerCounter += change;

        CountChange();
    }

    private void CountChange()
    {
        if (!isDown)
        {
            // Activate interaction
            if (triggerCounter >= TriggerCount && !IsActive)
            {
                DoStuff();
                IsActive = true;

                if (Sounds)
                    myAudio.PlayOneShot(ActivationSound);

                if (HideOnInteraction)
                    GetComponent<SpriteRenderer>().enabled = false;

                if (StayDown)
                    isDown = true;
            }
            // Deactivate interaction
            else if (triggerCounter < TriggerCount && IsActive)
            {
                UndoStuff();
                IsActive = false;

                if (Sounds)
                    myAudio.PlayOneShot(DeactivationSound);

                if (HideOnInteraction)
                    GetComponent<SpriteRenderer>().enabled = true;
               
            }
        }
    }

    public void Toggle()
    {
        ChangeCount(toggleValue);
        toggleValue *= -1;
    }

    protected virtual void DoStuff()
    {

    }
    protected virtual void UndoStuff()
    {

    }
}
