using UnityEngine;
using System.Collections;

public class WeaponAnimation : MonoBehaviour
{
    public Animation anim;
    public AnimationClip fire;
    public AnimationClip reload;
    public AnimationClip draw;
    public KeyCode reloadKey = KeyCode.R;
    public KeyCode fireKey = KeyCode.Mouse0;
    public KeyCode drawKey = KeyCode.D;

    void Awake()
    {
        if (fire == null) Debug.LogError("Please assign a fire aimation in the inspector!");
        if (reload == null) Debug.LogError("Please assign a reload animation in the inspector!");
        if (draw == null) Debug.LogError("Please assign a draw animation in the inspector!");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(reloadKey))
        {
            if (reload != null) anim.Play(reload.name);
        }
        else if (Input.GetKeyDown(fireKey))
        {
            if (fire != null) anim.Play(fire.name);
        }
        else if (Input.GetKeyDown(drawKey))
        {
            if (draw != null) anim.Play(draw.name);
        }
    }
}
