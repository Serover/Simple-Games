using UnityEngine;

public class TagExample : MonoBehaviour
{
    // use the header "GiveTag" to use the drawer to allow you to select an existing Tag
    // works with Arrays to allow you to select multiple "interacting" tags.
    [GiveTag]
    public string[] InteractWith = new string[] { };

    //change the name of the GiveTagAttribute to change the the name of the header, the header name
    // is automaticly the  full Attributename - attribute. In this case, GiveTagAttribute = GiveTag

    // also works with a single string ofc.
    [GiveTag]
    public string oneTag;
}
