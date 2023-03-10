using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundUpAttackPS : MonoBehaviour
{
    public ParticleSystem particles;
    public void ps()
    {
        particles.Play();
    }
    public void unps()
    {
        particles.Stop();
    }
    public void ds()
    {
        Destroy(gameObject);
    }
}
