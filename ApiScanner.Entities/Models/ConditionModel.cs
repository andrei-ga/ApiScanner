using ApiScanner.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ApiScanner.Entities.Models
{
    public class ConditionModel
    {
        /// <summary>
        /// Id of condition.
        /// </summary>
        [Key]
        public Guid ConditionId { get; set; }

        /// <summary>
        /// Id of api that has the condition.
        /// </summary>
        public Guid ApiId { get; set; }

        /// <summary>
        /// Where to look for condition.
        /// </summary>
        public ConditionType MatchType { get; set; }

        /// <summary>
        /// Variable to look for.
        /// </summary>
        [MaxLength(100)]
        public string MatchVar { get; set; }

        /// <summary>
        /// How to compare matching variable.
        /// </summary>
        public CompareType CompareType { get; set; }

        /// <summary>
        /// With what value to compare the matching varaible.
        /// </summary>
        [MaxLength(100)]
        public string CompareValue { get; set; }

        /// <summary>
        /// Will log a success if true and condition passed. Else will log a fail.
        /// </summary>
        public bool ShouldPass { get; set; }

        /// <summary>
        /// Api that has the condition.
        /// </summary>
        public virtual ApiModel Api { get; set; }
    }
}
