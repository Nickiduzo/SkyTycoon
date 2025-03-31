using UnityEngine;

public class Publisher : Building
{
    private int tyre = 0;

    private int maxTyre = 12;

    public override void SetNormal()
    {
        base.SetNormal();
        tyre = 1;
    }
}
