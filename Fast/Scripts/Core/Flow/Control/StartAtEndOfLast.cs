using System;

public static class FastUIFlowStartAtEndOfLastExtension
{
    public static Fast.Flow.FlowContainer FlowNextStartAtEndOfLast(this Fast.Flow.FlowContainer container)
    {
        container.NextStartsAtEndOfLast = true;

        return container;
    }
}
