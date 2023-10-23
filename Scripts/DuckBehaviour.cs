using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckBehaviour : MonoBehaviour
{

    /// <summary>
    /// The material to use when this object is inactive (not being gazed at).
    /// </summary>
    public Mesh inactiveMesh;

    /// <summary>
    /// The material to use when this object is active (gazed at).
    /// </summary>
    public Mesh gazedAtMesh;

    AudioSource source;
    private bool gazed = false;
    public float impulse = 5.0f;
    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        cameraTransform = GameObject.FindWithTag("MainCamera").transform;
        SetMesh(false);
    }

    void FixedUpdate() {
        if (gazed && Input.GetAxis("Jump") > 0) { Impulse(); }
    }

    void OnCollisionEnter(Collision other) {
        if (other.collider.gameObject.tag == "Player") { return; }
        if (source != null) { source.Play(); }
        ScoreController.scoreValue++;
    }

    /// <summary>
    /// This method is called by the Main Camera when it starts gazing at this GameObject.
    /// </summary>
    public void OnPointerEnter() {
        Debug.Log($"Ha mirado a {name}");
        gazed = true;
        SetMesh(true);
    }

    /// <summary>
    /// This method is called by the Main Camera when it stops gazing at this GameObject.
    /// </summary>
    public void OnPointerExit() {
        Debug.Log($"Ha dejado de mirar a {name}");
        gazed = false;
        SetMesh(false);
    }

    /// <summary>
    /// This method is called by the Main Camera when it is gazing at this GameObject and the screen
    /// is touched.
    /// </summary>
    public void OnPointerClick() {
        Impulse();
    }

    private void Impulse() {
        GetComponent<Rigidbody>().AddForce(
            (cameraTransform.forward + cameraTransform.up) * impulse,
            ForceMode.Impulse
        );
    }

    /// <summary>
    /// Sets this instance's Mesh according to gazedAt status.
    /// </summary>
    ///
    /// <param name="gazedAt">
    /// Value `true` if this object is being gazed at, `false` otherwise.
    /// </param>
    private void SetMesh(bool gazedAt)
    {
        if (inactiveMesh != null && gazedAtMesh != null)
        {
            GetComponent<MeshFilter>().mesh = gazedAt ? gazedAtMesh : inactiveMesh;
        }
    }

}
