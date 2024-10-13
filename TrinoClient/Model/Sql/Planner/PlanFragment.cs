using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TrinoClient.Model.Operator;
using TrinoClient.Model.Sql.Planner.Plan;

namespace TrinoClient.Model.Sql.Planner
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.PlanFragment.java
    /// </summary>
    public class PlanFragment
    {
        #region Public Properties

        public PlanFragmentId Id { get; }

        public PlanNode Root { get; }

        /// <summary>
        /// TODO: Should be <Symbol, string> Problem with Json.NET
        /// </summary>
        public IDictionary<string, string> Symbols { get; }

        public PartitioningHandle Partitioning { get; }

        public IEnumerable<PlanNodeId> PartitionedSources { get; }

        public PartitioningScheme PartitioningScheme { get; }

        public PipelineExecutionStrategy PipelineExecutionStrategy { get; }

        [JsonIgnore]
        public IEnumerable<string> Types { get; }

        [JsonIgnore]
        public HashSet<PlanNode> PartionedSourceNodes { get; }

        [JsonIgnore]
        public IEnumerable<RemoteSourceNode> RemoteSourceNodes { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public PlanFragment(
            PlanFragmentId id,
            PlanNode root,
            IDictionary<string, string> symbols,
            PartitioningHandle partitioning,
            IEnumerable<PlanNodeId> partitionedSources,
            PartitioningScheme partitioningScheme,
            PipelineExecutionStrategy pipelineExecutionStrategy
            )
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Root = root ?? throw new ArgumentNullException(nameof(root));
            Symbols = symbols ?? throw new ArgumentNullException(nameof(symbols));
            Partitioning = partitioning ?? throw new ArgumentNullException(nameof(partitioning));
            PartitionedSources = partitionedSources ?? throw new ArgumentNullException(nameof(partitionedSources));
            PipelineExecutionStrategy = pipelineExecutionStrategy;
            PartitioningScheme = partitioningScheme ?? throw new ArgumentNullException(nameof(partitioningScheme));

            ParameterCheck.Check(PartitionedSources.Distinct().Count() == PartitionedSources.Count(), "PartitionedSources contains duplicates.");

            Types = PartitioningScheme.OutputLayout.Select(x => x.ToString());
            // Materialize this during construction
            PartionedSourceNodes = new HashSet<PlanNode>(FindSources(Root, PartitionedSources));
            RemoteSourceNodes = FindRemoteSourceNodes(Root).ToList();
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("id", Id)
                .Add("partitioning", Partitioning)
                .Add("partitionedSource", PartitionedSources)
                .Add("partitionFunction", PartitioningScheme)
                .ToString();
        }

        #endregion

        #region Private Methods

        private static IEnumerable<PlanNode> FindSources(PlanNode node, IEnumerable<PlanNodeId> nodeIds)
        {
            if (nodeIds.Contains(node.Id))
            {
                yield return node;
            }

            foreach (PlanNode Source in node.GetSources())
            {
                foreach (PlanNode Item in FindSources(Source, nodeIds))
                {
                    yield return Item;
                }
            }
        }

        private static IEnumerable<RemoteSourceNode> FindRemoteSourceNodes(PlanNode node)
        {
            foreach (PlanNode Source in node.GetSources())
            {
                foreach (RemoteSourceNode Item in FindRemoteSourceNodes(Source))
                {
                    yield return Item;
                }
            }

            if (node is RemoteSourceNode)
            {
                yield return (RemoteSourceNode)node;
            }
        }

        #endregion
    }
}
