using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

/// <summary>
/// the level select 2D player
/// </summary>
public class Player2D : MonoBehaviour
{
    /// <summary>
    /// the speed at which tilly runs around
    /// </summary>
    public float moveSpeed = 0.1f;

    public Rigidbody2D body;
    
    SortingGroup sortingGroup;

    [SerializeField] Animator anim;

    /// <summary>
    /// the last seen location so the player returns to the same location on the level selector
    /// </summary>
    private static Vector3 location;


    /// <summary>
    /// whether a location has been saved.
    /// </summary>
    private static bool locationSaved = false; // TODO: this could be true if SaveData includes the location of the player on the level select

    void Start()
    {
        if (!locationSaved){
            location = transform.position;
            locationSaved = true;
        }
        else {
            // previous location found, put the player there
            transform.position = location;
        }
    }

    private void Awake()
    {
        sortingGroup = GetComponent<SortingGroup>();
    }


    private void Update()
    {
        Move();
        sortingGroup.sortingOrder = -(int) transform.position.y;
        location = transform.position;
        // exitinig the level select
        if (Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene("StartUpScreen");
        }
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(horizontal, vertical, 0f).normalized;
        body.velocity = movement * moveSpeed;

        // adjust the animation based on input.
        if (horizontal == 0 && vertical == 0)
        {
            anim.SetBool("Moving", false);
            anim.SetBool("Sideways", false);
        }
        else
        {
            anim.SetBool("Moving", true);
            //See if they are moving to the side
            if (horizontal == 0)
            {
                anim.SetBool("Sideways", false);
            }
            else {
                //See which direction they are moving
                if (horizontal > 0)
                {
                    anim.SetBool("Sideways", true);
                    anim.SetBool("Left", false);
                }
                else if (horizontal < 0)
                {
                    anim.SetBool("Sideways", true);
                    anim.SetBool("Left", true);
                }
            }
            //See if they are moving up or down
            if (vertical > 0)
            {
                anim.SetBool("Upwards", true);
            }
            else
            {
                anim.SetBool("Upwards", false);
            } 
        }
    }
}

