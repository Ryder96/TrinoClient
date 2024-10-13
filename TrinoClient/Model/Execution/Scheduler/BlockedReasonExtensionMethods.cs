using System;

namespace TrinoClient.Model.Execution.Scheduler
{
    public static class BlockedReasonExtensionMethods
    {
        public static BlockedReason CombineWith(this BlockedReason reason, BlockedReason other)
        {
            return reason switch
            {
                BlockedReason.WRITER_SCALING => throw new ArgumentException("Cannot be combined"),
                BlockedReason.NO_ACTIVE_DRIVER_GROUP => other,
                BlockedReason.SPLIT_QUEUES_FULL => other == BlockedReason.SPLIT_QUEUES_FULL || other == BlockedReason.NO_ACTIVE_DRIVER_GROUP ?
                                        BlockedReason.SPLIT_QUEUES_FULL :
                                        BlockedReason.MIXED_SPLIT_QUEUES_FULL_AND_WAITING_FOR_SOURCE,
                BlockedReason.WAITING_FOR_SOURCE => other == BlockedReason.WAITING_FOR_SOURCE || other == BlockedReason.NO_ACTIVE_DRIVER_GROUP ?
                                        BlockedReason.WAITING_FOR_SOURCE :
                                        BlockedReason.MIXED_SPLIT_QUEUES_FULL_AND_WAITING_FOR_SOURCE,
                BlockedReason.MIXED_SPLIT_QUEUES_FULL_AND_WAITING_FOR_SOURCE => BlockedReason.MIXED_SPLIT_QUEUES_FULL_AND_WAITING_FOR_SOURCE,
                _ => throw new ArgumentException($"Unknown blocked reason: {other}."),
            };
        }
    }
}
