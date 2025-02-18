﻿using TrinoClient.Model.Metadata;
using TrinoClient.Model.SPI.Predicate;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.TableScanNode.java
    /// </summary>
    public class TableScanNode : PlanNode
    {
        #region Public Properties

        public TableHandle Table { get; }

        public IEnumerable<Symbol> OutputSymbols { get; }

        /// <summary>
        /// TODO: Key is Symbol and Value is IColumnHandle
        /// </summary>
        public IDictionary<string, dynamic> Assignments { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public TableLayoutHandle Layout { get; }

        /// <summary>
        /// TODO: TupleDomain<IColumnHandle>
        /// </summary>
        public TupleDomainPlaceHolder<dynamic> CurrentConstraint { get; }

        /// <summary>
        /// TODO: Supposed to be Expression
        /// </summary>
        public dynamic OriginalConstraint { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TableScanNode(
            PlanNodeId id,
            TableHandle table,
            IEnumerable<Symbol> outputSymbols,
            IDictionary<string, dynamic> assignments,
            TableLayoutHandle layout,
            TupleDomainPlaceHolder<dynamic> currentConstraint,
            dynamic originalConstraint
            ) : base(id)
        {
            this.Table = table ?? throw new ArgumentNullException("table");
            this.OutputSymbols = outputSymbols ?? throw new ArgumentNullException("outputSymbols");
            this.Assignments = assignments ?? throw new ArgumentNullException("assignments");
            this.OriginalConstraint = originalConstraint;
            this.Layout = layout ?? throw new ArgumentNullException("layout");
            this.CurrentConstraint = currentConstraint ?? throw new ArgumentNullException("currentConstraint");

            ParameterCheck.Check(this.OutputSymbols.All(x => this.Assignments.Keys.Contains(x.ToString())), "Assignments does not cover all of outputs.");
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return this.OutputSymbols;
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            return new PlanNode[0];
        }

        #endregion
    }
}
