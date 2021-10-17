using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{

    private bool isDragged = false;
    private Vector3 mouseDragStartPos;
    private Vector3 spriteDragStartPos;
    private Vector3 endPosition;
    [SerializeField] private GameObject decoy;
    private GameObject decoyObj;
    
    bool notNull = false;
    [SerializeField] private Sprite highlight;
    [SerializeField] private Sprite initialSprite;
    private RaycastHit2D[] hits;
    private Move move;

    public void Awake(){
        move = GetComponent<Move>();
    }

    private void OnMouseDown(){
        //Can only click on robot if it's not already moving
        if(!(move.shouldMove)){  

            //Spawn a scriptless copy of the robot at its location while the real robot is being dragged
            decoyObj = Instantiate(decoy, transform.position, transform.rotation); 

            //Make the real robot invisible while it's being dragged
            GetComponent<SpriteRenderer>().enabled = false;   

            isDragged = true;

            //The robot shouldn't collide with stone while it's being dragged, so its collider is set to trigger
            gameObject.GetComponent<Collider2D>().isTrigger = true;

            //Remember the start position
            mouseDragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spriteDragStartPos = transform.localPosition;

            //Send the start position to the movement script
            move.startPosition = transform.localPosition;
        }
      
    }

    private void OnMouseDrag(){

        //Drag the robot along with the mouse cursor  
        transform.localPosition = spriteDragStartPos + Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouseDragStartPos;
         
        //Change the sprites of the stones hit by the raycast    
        if(notNull){
            foreach (RaycastHit2D hit in hits){
                if(hit.collider.gameObject.tag == "stone"){
                    if(hit.collider.gameObject.layer == 7){
                        hit.collider.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    }
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite = initialSprite;
                }
            }
        }
        
        //If the robot is being dragged and not moving on its own
        if(isDragged && !(move.shouldMove)){
            
            //Calculate the position where the robot would travel to if the user stops dragging it this frame
            endPosition = transform.localPosition;
            
            if(Mathf.Abs(endPosition.x - spriteDragStartPos.x) > Mathf.Abs(endPosition.y - spriteDragStartPos.y)){

                endPosition.y = spriteDragStartPos.y;
                endPosition.x = spriteDragStartPos.x + (Mathf.Round((endPosition.x-spriteDragStartPos.x)/move.unitLength) * move.unitLength);
            }
            else{
                endPosition.x = spriteDragStartPos.x;
                endPosition.y = spriteDragStartPos.y + (Mathf.Round((endPosition.y-spriteDragStartPos.y)/move.unitLength) * move.unitLength);
            }
            
            //Highlight all the stones the robot would mine on its way to destination (this also happens at the start of this function in order to avoid flickering)
            hits = Physics2D.LinecastAll(spriteDragStartPos, endPosition);
            notNull = true;
            foreach (RaycastHit2D hit in hits){
                if(hit.collider.gameObject.tag == "stone"){
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite = highlight;
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().enabled =true;
                }
                
            }

        }
    }

    private void OnMouseUp(){

        //If the user lets go of the robot
        if(!(move.shouldMove) && isDragged){
            
            isDragged = false;

            //Restore the collider to normal, remember the current position, restore the robot to the starting position, make it visible and destroy the decoy
            gameObject.GetComponent<Collider2D>().isTrigger = false;
            endPosition = transform.localPosition;
            transform.localPosition = spriteDragStartPos;
            GetComponent<SpriteRenderer>().enabled = true;
            Destroy(decoyObj);

            //Calculate the destination coords
            if(Mathf.Abs(endPosition.x - spriteDragStartPos.x) > Mathf.Abs(endPosition.y - spriteDragStartPos.y)){

                endPosition.y = spriteDragStartPos.y;
                endPosition.x = spriteDragStartPos.x + (Mathf.Round((endPosition.x-spriteDragStartPos.x)/move.unitLength) * move.unitLength);
            }
            else{

                endPosition.x = spriteDragStartPos.x;
                endPosition.y = spriteDragStartPos.y + (Mathf.Round((endPosition.y-spriteDragStartPos.y)/move.unitLength) * move.unitLength);
            }

            //Send the destination coords to the movement script
            move.endPosition = endPosition;
            move.shouldMove = true;
        }       
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        //If robot collides with stone while moving and not being dragged
        if (collision.gameObject.tag == "stone")
        {
            if(!isDragged){
                //Make the stone invisible and set its collider to trigger
                Debug.Log("mining");
                collision.gameObject.layer = 7;
                collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
                collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision){
       
        if (collision.gameObject.tag == "stone")
        {                      
            if(!isDragged){
                Debug.Log("mined");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other){

        //Layer 7 is stone that has already been mined
        if(other.gameObject.layer == 7){
            //Remove the highlight of the empty spaces after the robot passes through
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}