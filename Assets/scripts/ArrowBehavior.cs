using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArrowBehavior : MonoBehaviour
{
    public GameObject self;
    public Material oldMat;
    public Material newMat;

    private BoardBehavior boardBehavior;

    void Start()
    {
        self = this.gameObject;
        boardBehavior = GameObject.Find("connect4board").GetComponent<BoardBehavior>();
    }

    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
        {
            self.GetComponentInChildren<Renderer>().material = newMat;
            if (Input.GetMouseButtonDown(0))
            {
                if(!boardBehavior.board.turn)
                {
                    boardBehavior.MakePlayerMove(self.name[self.name.Length - 1] - 48, boardBehavior.yellowCoinPrefab);
                }
                else
                {
                    boardBehavior.MakePlayerMove(self.name[self.name.Length - 1] - 48, boardBehavior.redCoinPrefab);
                }

            }
        } else
        {
            self.GetComponentInChildren<Renderer>().material = oldMat;
        }

        
    }
}
