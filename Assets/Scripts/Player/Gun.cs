using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //same index as ammo index
    [Tooltip("Make sure the index of the ammo is same as Player script")]
    public GameObject[] ammoPrefabs;

    //spawn of the bullet
    [SerializeField] Transform bulletSpawn;

    [SerializeField] Targetting aim;

    [SerializeField] float bulletSpeed;

    public AudioSource gunAudioSource;

    public AudioClip shootSoundClip;

    //Returns the name of the animation trigger to set
    public string Shoot(int ammoIndex)
    {
        gunAudioSource.PlayOneShot(shootSoundClip);
        //print("bullet at: " + bulletSpawn.position);
        GameObject projectile = Instantiate(ammoPrefabs[ammoIndex], transform.position, Quaternion.identity); //bulletSpawn.rotation
        // Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        // if (projectileRb != null)
        // {
        //     projectileRb.AddForce(bulletSpawn.forward * 20f, ForceMode.Impulse);
        // }
        Projectile bullet = projectile.GetComponent<Projectile>();
        Vector3 direction = aim.targetLocation - bullet.transform.position;
        direction.y = 0;
        bullet.SetDirection(direction, bulletSpeed);
        bullet.AdjustYSpawn();

        float angle = (Mathf.Atan(direction.z / direction.x) * Mathf.Rad2Deg);
        if (direction.z < 0)
        {
            angle = angle + 180;
        }
        //print("Difference in positions is " + direction);
        //print("Shot, Angle is " + (Mathf.Atan(direction.z / direction.x) * Mathf.Rad2Deg));

        //This is to decide which animation to play, which is based on which direction you are shooting
        float animationAngle = getPlayerPerspectiveAngle();
        //Because the angles are really weird, these numbers look messed up, but I have tested them.
        print("Shooting at angle " + animationAngle);
        if (animationAngle >= 340 || animationAngle < 20)
        {
            return "ShootUp";
        }
        else if (animationAngle >= 20 && animationAngle < 80)
        {
            return "ShootUpLeft";
        }
        else if (animationAngle >= 80 && animationAngle < 140)
        {
            return "ShootDownLeft";
        }
        else if (animationAngle >= 140 && animationAngle < 220)
        {
            return "ShootDown";
        }
        else if (animationAngle >= 220 && animationAngle < 285)
        {
            return "ShootDownRight";
        }
        else if (animationAngle >= 285 && animationAngle < 340)
        {
            return "ShootUpRight";
        }
        else {
            Debug.Log("The gun animation is not working!");
            return "";
        }
    }

    private void Update()
    {
        Vector3 direction = aim.targetLocation - transform.position;
        float angle = Mathf.Abs(Mathf.Atan(direction.z / direction.x) * Mathf.Rad2Deg);
        if (direction.x < 0)
        {
            if (direction.z > 0)
            {
                angle = 90 + (90 - angle);
            }
            else
            {
                angle += 180;
            }
        }
        else if (direction.z < 0)
        {
            angle = 270 + (90 - angle);
        }
        //print("Difference in positions is " + direction);
        float playerPerspectiveAngle = angle - 45;
        if (playerPerspectiveAngle < 0) {
            playerPerspectiveAngle = 360 + playerPerspectiveAngle;
        }
        //Due to our perspective, angles are bizarre
        //So the left 90* angle is at 65 and the right 90* angle is at 290
        //print("Shot, Angle is " + playerPerspectiveAngle);
    }

    //Gets the rough angle from the player's perspective
    //It's not a perfect angle
    private float getPlayerPerspectiveAngle() {
        Vector3 direction = aim.targetLocation - transform.position;
        float angle = Mathf.Abs(Mathf.Atan(direction.z / direction.x) * Mathf.Rad2Deg);
        if (direction.x < 0)
        {
            if (direction.z > 0)
            {
                angle = 90 + (90 - angle);
            }
            else
            {
                angle += 180;
            }
        }
        else if (direction.z < 0)
        {
            angle = 270 + (90 - angle);
        }
        //print("Difference in positions is " + direction);
        float playerPerspectiveAngle = angle - 45;
        if (playerPerspectiveAngle < 0)
        {
            playerPerspectiveAngle = 360 + playerPerspectiveAngle;
        }
        //Due to our perspective, angles are bizarre
        //So the left 90* angle is at 65 and the right 90* angle is at 290
        //print("Shot, Angle is " + playerPerspectiveAngle);
        return playerPerspectiveAngle;
    }
}