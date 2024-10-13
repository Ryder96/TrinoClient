using Newtonsoft.Json;
using TrinoClient.Serialization;

namespace TrinoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.Lifespan.java
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class Lifespan
    {
        #region Private Fields

        private static readonly Lifespan TASK_WIDE = new(false, 0);
        private static readonly string TASK_WIDE_STR = "TaskWide";

        #endregion

        #region Public Properties

        public bool Grouped { get; }

        public int GroupId { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public Lifespan(bool grouped, int groupId)
        {
            Grouped = grouped;
            GroupId = groupId;
        }

        public Lifespan(string value)
        {
            if (value.Equals(TASK_WIDE_STR))
            {
                GroupId = 0;
                Grouped = false;
            }
            else
            {
                if (value.StartsWith("Group"))
                {
                    Grouped = true;
                    GroupId = int.Parse(value.Substring(5));
                }
            }
        }

        #endregion

        #region Public Methods

        public static Lifespan TaskWide()
        {
            return TASK_WIDE;
        }

        public static Lifespan DriverGroup(int id)
        {
            return new Lifespan(true, id);
        }

        public bool IsTaskWide()
        {
            return !Grouped;
        }

        public override string ToString()
        {
            return Grouped ? $"Group{GroupId}" : TASK_WIDE_STR;
        }

        #endregion
    }
}
