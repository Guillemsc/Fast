using System;

public static class FastUIFlowWithLastExtension
{
    public static Fast.Flow.FlowContainer FlowNextStartWithLast(this Fast.Flow.FlowContainer container)
    {
        container.NextStartsWithLast = true;

        return container;
    }
}