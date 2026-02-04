using UnityEngine;
using UnityEngine.UI;
public class NPC : MonoBehaviour
{
    public Transform player;
    public Sprite[] messages;
    public GameObject bubble;
    public Image bubbleImage;
    private bool changedLastFrame;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    bubbleImage = bubble.GetComponent<Image>();	    
    Debug.Log(bubbleImage);
    }

    // Update is called once per frame
    void Update()
    {

       if (Vector3.Distance(player.position,transform.position) < 3) {
	       bubble.SetActive(true);

	       if (!changedLastFrame) {
		       bubbleImage.sprite = messages[Random.Range(0,messages.Length)];
	       }
	       changedLastFrame=true;
       }
       else {
	       changedLastFrame=false;
	       bubble.SetActive(false);
       }


    }
}
