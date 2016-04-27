using UnityEngine;
using System.Collections;

public class TestMeca : MonoBehaviour
{

    Animator anim;
    private AnimatorStateInfo layer2CurrentState;
    static int ShootState = Animator.StringToHash("shoot.Shoot");

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }
	
    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        var goDir = Vector3.forward;
        if (v < -0.1f)
        {
            //anim.SetInteger("Action", 1);
            //anim.SetFloat("Direction", h);
            goDir = Vector3.back;
        } else if (v > 0.1f)
        {
            //anim.SetInteger("Action", 2);
            //anim.SetFloat("Direction", h);
            goDir = Vector3.forward;
        } else
        {
            //anim.SetInteger("Action", 0);
            goDir = Vector3.zero;
        }

        var goDir2 = Vector3.left;
        if (h < -0.1f)
        {
            goDir2 = Vector3.left;
        } else if (h > 0.1f)
        {
            goDir2 = Vector3.right;
        } else
        {
            goDir2 = Vector3.zero;
        }

        var mpos = Input.mousePosition;
        if (mpos.x > Screen.width / 2 + 20)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));
        } else if (mpos.x < Screen.width / 2 - 20)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, -90, 0));
        } else
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

        var totalDir = goDir + goDir2;
        if (totalDir.magnitude <= 0.1f)
        {
            anim.SetInteger("Action", 0);
        } else
        {
            anim.SetInteger("Action", 1);

            var faceDir = transform.forward;
            faceDir.Normalize();
            totalDir.Normalize();
            var cosV = Vector3.Dot(totalDir, faceDir);
            /*
            if (cosV > 0)
            {
                anim.SetInteger("Action", 2);
            } else
            {
                anim.SetInteger("Action", 1);
            }
            */
            //Vector3.Cross(totalDir, faceDir);
            float angle;
            Vector3 axis;
            Quaternion.FromToRotation(faceDir, totalDir).ToAngleAxis(out angle, out axis);
            var sa = Mathf.Sin(angle);
            anim.SetFloat("Direction", sa);
            anim.SetFloat("Speed", cosV);
        }

        layer2CurrentState = anim.GetCurrentAnimatorStateInfo(1);
        if(Input.GetKeyDown(KeyCode.Space)) {
            anim.SetBool("Shoot", true);
        }
        Debug.Log("fullPath: "+layer2CurrentState.fullPathHash);
        if(layer2CurrentState.fullPathHash == ShootState) {
            anim.SetBool("Shoot", false);
        }


            
                
    }
}
