using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlankApiModel.Model.Entities
{
    /// <summary>
    /// Example of entity model
    /// </summary>
    [Table("jumps")]
    public class JumpModel
    {
        /// <summary>
        /// Key must be name id
        /// </summary>
        [Key]
        public int Id { get; set; }

        [Column("whatever")]
        public string Whatever { get; set; }

        /// <summary>
        /// can not contains _
        /// </summary>
        [ForeignKey("giveyourjumpid")]
        public int GiveYourJumpId { get; set; }
    }
}
