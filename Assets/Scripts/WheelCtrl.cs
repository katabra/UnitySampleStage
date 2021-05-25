using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WheelCtrl : MonoBehaviour
{
    public enum WHEELSTATE
    {
        STOPPED,
        ROLLING,
        TRYEND,
        FINISHED,
    }


    public List<NumSlotCtrl> slotList;
    LinkedList<NumSlotCtrl> slotLink;

    public WHEELSTATE state = WHEELSTATE.STOPPED;

    public float speed = 1f;

    float randomSpeed = 0f;
    bool _isOp;

    public void StageReset(bool isOp)
    {
        state = WHEELSTATE.STOPPED;

        _isOp = isOp;

        randomSpeed = speed / Random.Range(10f, 20f);
        randomSpeed = ((Random.Range(0, 2) == 0) ? -1f : 1f) * randomSpeed;

        if (slotLink == null)
            slotLink = new LinkedList<NumSlotCtrl>();

        slotLink.Clear();

        for(int i = 0; i < slotList.Count; ++i)
        {
            slotLink.AddLast(slotList[i]);

            slotList[i].transform.position = transform.position + Vector3.up * 2f * i;

            slotList[i].numText.text = string.Empty;
        }
    }

    public void WheelStart()
    {
        state = WHEELSTATE.ROLLING;

        StartCoroutine(CoSetNum());
    }

    public void WheelStop()
    {
        state = WHEELSTATE.TRYEND;
    }

    IEnumerator CoSetNum()
    {
        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < slotList.Count; ++i)
        {
            if (!_isOp)
                slotList[i].Num = UnityEngine.Random.Range(1, 500);
            else
                slotList[i].numText.text = "+";

        }
    }

    private void Update()
    {
        switch (state)
        {
            case WHEELSTATE.STOPPED:
                break;
            case WHEELSTATE.ROLLING:
                {
                    LinkedListNode<NumSlotCtrl> node;
                    for (node = slotLink.First; node != null; node = node.Next)
                    {
                        node.Value.transform.position += Vector3.down * (speed + randomSpeed) * Time.deltaTime;
                    }
                    node = slotLink.First;
                    if(node.Value.transform.position.y < -2)
                    {
                        node.Value.transform.position = slotLink.Last.Value.transform.position + Vector3.up * 2f;
                        slotLink.RemoveFirst();
                        slotLink.AddLast(node);
                    }    
                }
                break;
            case WHEELSTATE.TRYEND:
                {
                    int i = 0;
                    for (LinkedListNode<NumSlotCtrl> node = slotLink.First; node != null; node = node.Next, ++i)
                    {
                        node.Value.transform.position = transform.position + Vector3.up * 2f * i;
                    }
                    state = WHEELSTATE.FINISHED;
                }
                break;
        }
    }

    public int GetResult()
    {
        return slotLink.First.Value.Num;
    }

}
