using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonAnimation : MonoBehaviour
{
    #region Serialized Fields
    [Header("Starting Values")]
    [SerializeField] private int animSpeed = 1;
    [SerializeField] private bool pop = false;
    [SerializeField] private bool unPop = false;
    [Header("Components")]
    [SerializeField] private Animator animator;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = animSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        pop = Input.GetKeyDown("return");
        animator.SetBool("Pop", pop);
            
        unPop = Input.GetKeyDown("space");
        animator.SetBool("Unpop", unPop);
    }
}
