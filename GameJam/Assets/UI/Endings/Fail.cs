using UnityEngine;

[DefaultExecutionOrder(210)]
public sealed class Fail : EndingTrigger<Fail>
{
    public int fail = 6;
    protected override bool Reached => fail <= 0;
}
