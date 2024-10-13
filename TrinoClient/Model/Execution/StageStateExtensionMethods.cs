using System.Linq;

namespace TrinoClient.Model.Execution
{
    public static class StageStateExtensionMethods
    {
        public static bool IsDone(this StageState state)
        {
            StageState[] DoneStates = [StageState.FINISHED, StageState.CANCELED, StageState.ABORTED, StageState.FAILED];

            return DoneStates.Contains(state);
        }
    }
}
